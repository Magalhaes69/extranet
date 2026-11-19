using Application.Services;
using Domain.Dtos;
using Domain.Interfaces;
using Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Notificações do utilizador autenticado (o sino do cabeçalho). Qualquer utilizador tem as suas;
/// quem tem o processo "licenses" recebe também os avisos de licenças e pedidos.
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationsController : ControllerBase
{
    private const int MaxTake = 100;

    private readonly INotificationService _notifications;
    private readonly LicenseNotificationSync _licenseSync;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(
        INotificationService notifications,
        LicenseNotificationSync licenseSync,
        ILogger<NotificationsController> logger)
    {
        _notifications = notifications;
        _licenseSync = licenseSync;
        _logger = logger;
    }

    private string? UserId => User.FindFirst("UserId")?.Value;
    private string? CardCode => User.FindFirst("CardCode")?.Value;

    /// <summary>Notificações mais recentes e número de não lidas.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? onlyUnread,
        [FromQuery] int take = 30,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(UserId))
            return Forbid();

        // Avisos de licenças: só para quem pode ver licenças (a sincronização é limitada no tempo e nunca falha).
        if (!string.IsNullOrWhiteSpace(CardCode) && AppProcesses.HasAccess(User, AppProcesses.Licenses))
            await _licenseSync.SyncAsync(UserId, CardCode, cancellationToken);

        var (items, unread) = await _notifications.GetAsync(
            UserId, onlyUnread == true, Math.Clamp(take, 1, MaxTake), cancellationToken);

        return Ok(new
        {
            unread,
            items = items.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                Level = n.Level,
                IsRead = n.IsRead,
                ReadAt = n.ReadAt,
                ActionUrl = n.ActionUrl,
                ActionText = n.ActionText,
                // UTC explícito para o browser converter para a hora local.
                CreatedAt = DateTime.SpecifyKind(n.CreatedAt, DateTimeKind.Utc)
            })
        });
    }

    /// <summary>Contador de notificações não lidas.</summary>
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(UserId))
            return Forbid();

        return Ok(new { count = await _notifications.CountUnreadAsync(UserId, cancellationToken) });
    }

    /// <summary>Marca uma notificação como lida.</summary>
    [HttpPost("{id:guid}/mark-read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(UserId))
            return Forbid();

        await _notifications.MarkReadAsync(UserId, id, cancellationToken);
        return Ok(new { flag = true });
    }

    /// <summary>Marca todas as notificações como lidas.</summary>
    [HttpPost("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(UserId))
            return Forbid();

        await _notifications.MarkAllReadAsync(UserId, cancellationToken);
        return Ok(new { flag = true });
    }
}
