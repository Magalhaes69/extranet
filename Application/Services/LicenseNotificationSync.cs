using System.Collections.Concurrent;
using Application.Interfaces;
using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    /// <summary>
    /// Gera notificações a partir do estado das licenças e dos pedidos no LicenseKey (que é quem trata as licenças):
    /// licenças a expirar/expiradas, fim do período de teste e mudanças de estado dos pedidos.
    /// Corre quando o utilizador pede as notificações, no máximo uma vez por intervalo, e nunca lança.
    /// </summary>
    public sealed class LicenseNotificationSync
    {
        public const int ExpiringDays = 30;
        public const int UrgentDays = 7;
        private const int RecentRequestDays = 30;
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

        // Última sincronização por utilizador (evita ir ao LicenseKey em cada pedido do sino).
        private static readonly ConcurrentDictionary<string, DateTime> LastSync = new();

        private readonly IClientLicenseApiClient _licenses;
        private readonly INotificationService _notifications;
        private readonly ILogger<LicenseNotificationSync> _logger;

        public LicenseNotificationSync(
            IClientLicenseApiClient licenses,
            INotificationService notifications,
            ILogger<LicenseNotificationSync> logger)
        {
            _licenses = licenses;
            _notifications = notifications;
            _logger = logger;
        }

        public async Task SyncAsync(string userId, string cardCode, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            if (LastSync.TryGetValue(userId, out var last) && now - last < Interval)
                return;

            // Marca já, para que uma falha do LicenseKey não faça um pedido por cada refresco do sino.
            LastSync[userId] = now;

            try
            {
                var overview = await _licenses.GetByCardCodeAsync(cardCode, cancellationToken);
                if (overview is not null)
                {
                    await NotifyTrialAsync(userId, cardCode, overview, now);
                    foreach (var license in overview.Licenses)
                        await NotifyLicenseAsync(userId, cardCode, license, now);
                }

                var requests = await _licenses.GetRequestsAsync(cardCode, null, cancellationToken);
                foreach (var request in requests)
                    await NotifyRequestAsync(userId, cardCode, request, now);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Não foi possível sincronizar as notificações de licenças de {CardCode}", cardCode);
            }
        }

        // ------------------------------------------------------------------ período de teste

        private async Task NotifyTrialAsync(string userId, string cardCode, ClientLicenseOverviewDto overview, DateTime now)
        {
            if (!overview.IsTrial || !overview.TrialEndDate.HasValue)
                return;

            var end = overview.TrialEndDate.Value;
            var days = DaysLeft(end, now);
            var date = end.ToLocalTime().ToString("dd/MM/yyyy");

            if (days <= 0)
            {
                await _notifications.AddAsync(userId, cardCode, NotificationTypes.TrialEnding, NotificationLevels.Error,
                    "Período de teste terminado",
                    $"O período de teste terminou a {date}. Requisite uma licença para continuar a utilizar a aplicação.",
                    "/licenses/request", "Requisitar licença",
                    dedupeKey: $"trial-ended:{end:yyyyMMdd}", relatedEntityType: "Trial");
            }
            else if (days <= ExpiringDays)
            {
                await _notifications.AddAsync(userId, cardCode, NotificationTypes.TrialEnding, NotificationLevels.Warning,
                    "Período de teste a terminar",
                    $"O período de teste termina a {date} (faltam {days} dia(s)).",
                    "/licenses", "Ver licenças",
                    dedupeKey: $"trial-ending:{end:yyyyMMdd}", relatedEntityType: "Trial");
            }
        }

        // ------------------------------------------------------------------ licenças

        private async Task NotifyLicenseAsync(string userId, string cardCode, ClientLicenseDto license, DateTime now)
        {
            // Só licenças em uso: revogadas, inactivas ou ainda por emitir/activar não expiram.
            if (license.RevokedAt.HasValue || license.IsInactive || !license.IssuedAt.HasValue || !license.ActivatedAt.HasValue)
                return;

            var expiries = license.Modules
                .Where(m => m.IsEnabled && m.ModuleAvailable && m.ExpiryDate.HasValue)
                .Select(m => m.ExpiryDate!.Value)
                .ToList();
            if (expiries.Count == 0)
                return;

            var id = license.Id.ToString();
            var url = $"/licenses/details/{id}";

            // Todos os módulos expirados: licença expirada (notifica uma vez por data de expiração).
            if (expiries.All(e => DaysLeft(e, now) <= 0))
            {
                var last = expiries.Max();
                await _notifications.AddAsync(userId, cardCode, NotificationTypes.LicenseExpired, NotificationLevels.Error,
                    "Licença expirada",
                    $"A licença {license.MaskedKey} expirou a {last.ToLocalTime():dd/MM/yyyy}. Peça a renovação para evitar interrupções.",
                    url, "Ver licença",
                    dedupeKey: $"lic-expired:{id}:{last:yyyyMMdd}", relatedEntityType: "License", relatedEntityId: id);
                return;
            }

            // Próximo módulo a expirar (entre os que ainda não expiraram).
            var next = expiries.Where(e => DaysLeft(e, now) > 0).Min();
            var days = DaysLeft(next, now);
            if (days > ExpiringDays)
                return;

            // Dois avisos por data de expiração: aos 30 dias e quando faltam 7.
            await _notifications.AddAsync(userId, cardCode, NotificationTypes.LicenseExpiring, NotificationLevels.Warning,
                "Licença a expirar",
                $"A licença {license.MaskedKey} expira a {next.ToLocalTime():dd/MM/yyyy} (faltam {days} dia(s)).",
                url, "Ver licença",
                dedupeKey: $"lic-expiring:{id}:{next:yyyyMMdd}", relatedEntityType: "License", relatedEntityId: id);

            if (days <= UrgentDays)
            {
                await _notifications.AddAsync(userId, cardCode, NotificationTypes.LicenseExpiring, NotificationLevels.Error,
                    "Licença a expirar em breve",
                    $"A licença {license.MaskedKey} expira a {next.ToLocalTime():dd/MM/yyyy} (faltam {days} dia(s)). Peça já a renovação.",
                    url, "Renovar",
                    dedupeKey: $"lic-urgent:{id}:{next:yyyyMMdd}", relatedEntityType: "License", relatedEntityId: id);
            }
        }

        // ------------------------------------------------------------------ pedidos

        private async Task NotifyRequestAsync(string userId, string cardCode, ClientLicenseRequestDto request, DateTime now)
        {
            // Submetido é o estado inicial (já o conhece quem o fez). Só mudanças recentes: o histórico antigo não notifica.
            if (request.Status == 1)
                return;

            var when = request.CompletedAt ?? request.ReviewedAt ?? request.SubmittedAt;
            if (now - when > TimeSpan.FromDays(RecentRequestDays))
                return;

            var isRenewal = request.Type == 2;
            var subject = isRenewal ? "renovação" : "licença";
            var what = isRenewal
                ? $"O pedido de renovação da licença {request.MaskedKey}"
                : $"O pedido de licença \"{request.ProductName}\"";

            (string Title, string Message, string Level)? content = request.Status switch
            {
                2 => ($"Pedido de {subject} em análise", $"{what} está a ser analisado pela nossa equipa.", NotificationLevels.Info),
                3 => ($"Pedido de {subject} aprovado", $"{what} foi aprovado.", NotificationLevels.Success),
                4 => ($"Pedido de {subject} rejeitado",
                      $"{what} foi rejeitado.{(string.IsNullOrWhiteSpace(request.RejectionReason) ? "" : " Motivo: " + request.RejectionReason)}",
                      NotificationLevels.Error),
                5 => ($"Pedido de {subject} concluído", $"{what} foi concluído.", NotificationLevels.Success),
                6 => ($"Pedido de {subject} cancelado", $"{what} foi cancelado.", NotificationLevels.Warning),
                _ => null
            };

            if (content is null)
                return;

            await _notifications.AddAsync(userId, cardCode, NotificationTypes.RequestUpdate, content.Value.Level,
                content.Value.Title, content.Value.Message,
                "/license-requests", "Ver pedidos",
                dedupeKey: $"req:{request.Id}:{request.Status}",
                relatedEntityType: "LicenseRequest", relatedEntityId: request.Id.ToString(),
                createdAt: when.ToUniversalTime());
        }

        private static int DaysLeft(DateTime date, DateTime now) =>
            (int)Math.Ceiling((date.ToUniversalTime() - now).TotalDays);
    }
}
