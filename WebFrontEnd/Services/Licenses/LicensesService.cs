using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebFrontEnd.Services.Licenses
{
    // ------------------------------------------------------------------ DTOs (espelham a API)

    public class LicenseModuleDto
    {
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool ModuleAvailable { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int MaxUsers { get; set; }
    }

    public class LicenseDto
    {
        public Guid Id { get; set; }
        public string MaskedKey { get; set; } = string.Empty;
        public int LicenseType { get; set; }
        public int ApprovalStatus { get; set; }
        public bool IsInactive { get; set; }
        public DateTime? IssuedAt { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public List<LicenseModuleDto> Modules { get; set; } = new();
    }

    public class LicenseOverviewDto
    {
        public string Code { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string NUIT { get; set; } = string.Empty;
        public bool IsTrial { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public List<LicenseDto> Licenses { get; set; } = new();
    }

    public class LicenseRequestDto
    {
        public Guid Id { get; set; }
        /// <summary>1 = Nova licença, 2 = Renovação.</summary>
        public int Type { get; set; }
        /// <summary>1 Submetido, 2 Em análise, 3 Aprovado, 4 Rejeitado, 5 Concluído, 6 Cancelado.</summary>
        public int Status { get; set; }
        public Guid? LicenseKeyId { get; set; }
        public string? MaskedKey { get; set; }
        public string? ProductName { get; set; }
        public int? Users { get; set; }
        public int? Months { get; set; }
        public string? Notes { get; set; }
        public string RequestedByName { get; set; } = string.Empty;
        public string RequestedByEmail { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ReviewNotes { get; set; }
        public string? RejectionReason { get; set; }
    }

    // ------------------------------------------------------------------ formulários

    public class RequestLicenseForm
    {
        [Required(ErrorMessage = "Indique o produto.")]
        [StringLength(150, ErrorMessage = "Máximo de 150 caracteres.")]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, 10000, ErrorMessage = "O número de utilizadores deve estar entre 1 e 10000.")]
        public int Users { get; set; } = 1;

        [StringLength(500, ErrorMessage = "Máximo de 500 caracteres.")]
        public string? Notes { get; set; }
    }

    public class RenewLicenseForm
    {
        [Range(3, 24, ErrorMessage = "Escolha o período de renovação.")]
        public int Months { get; set; } = 12;

        [StringLength(500, ErrorMessage = "Máximo de 500 caracteres.")]
        public string? Notes { get; set; }
    }

    public record LicenseActionResult(bool Succeeded, string? Message = null);

    // ------------------------------------------------------------------ estado calculado

    public enum LicenseStatus
    {
        Active,
        Expiring,
        Expired,
        PendingIssue,
        PendingActivation,
        Inactive,
        Revoked
    }

    /// <summary>Regras de apresentação partilhadas pelas páginas de licenças.</summary>
    public static class LicenseRules
    {
        public const int ExpiringDays = 30;
        public static readonly int[] RenewalMonths = { 3, 6, 12, 24 };

        /// <summary>Módulos activos (habilitados e disponíveis) da licença.</summary>
        private static IEnumerable<LicenseModuleDto> ActiveModules(LicenseDto l) =>
            l.Modules.Where(m => m.IsEnabled && m.ModuleAvailable);

        /// <summary>Data de expiração mais próxima entre os módulos activos.</summary>
        public static DateTime? NextExpiry(LicenseDto l) =>
            ActiveModules(l).Where(m => m.ExpiryDate.HasValue).Select(m => m.ExpiryDate).Min();

        public static int? DaysLeft(DateTime? date) =>
            date.HasValue ? (int)Math.Ceiling((date.Value.ToUniversalTime() - DateTime.UtcNow).TotalDays) : null;

        public static LicenseStatus StateOf(LicenseDto l)
        {
            if (l.RevokedAt.HasValue) return LicenseStatus.Revoked;
            if (l.IsInactive) return LicenseStatus.Inactive;
            if (!l.IssuedAt.HasValue) return LicenseStatus.PendingIssue;
            if (!l.ActivatedAt.HasValue) return LicenseStatus.PendingActivation;

            var withExpiry = ActiveModules(l).Where(m => m.ExpiryDate.HasValue).ToList();
            if (withExpiry.Count > 0)
            {
                if (withExpiry.All(m => DaysLeft(m.ExpiryDate) <= 0)) return LicenseStatus.Expired;
                var next = DaysLeft(withExpiry.Min(m => m.ExpiryDate));
                if (next <= ExpiringDays) return LicenseStatus.Expiring;
            }

            return LicenseStatus.Active;
        }

        public static bool CanRenew(LicenseDto l) =>
            StateOf(l) is LicenseStatus.Active or LicenseStatus.Expiring or LicenseStatus.Expired;

        public static string BadgeClass(LicenseStatus s) => s switch
        {
            LicenseStatus.Active => "text-bg-success",
            LicenseStatus.Expiring => "text-bg-warning",
            LicenseStatus.Expired => "text-bg-danger",
            LicenseStatus.Revoked => "text-bg-dark",
            LicenseStatus.Inactive => "text-bg-secondary",
            _ => "text-bg-info"
        };

        public static string StateKey(LicenseStatus s) => "state" + s;

        // Pedidos
        public static string RequestBadgeClass(int status) => status switch
        {
            1 => "text-bg-primary",
            2 => "text-bg-info",
            3 => "text-bg-success",
            4 => "text-bg-danger",
            5 => "text-bg-success",
            _ => "text-bg-secondary"
        };

        public static string RequestStatusKey(int status) => "reqStatus" + status;
        public static string TypeKey(int licenseType) => "licenseType" + licenseType;
        public static string ApprovalKey(int approvalStatus) => "approval" + approvalStatus;

        /// <summary>Pedido ainda em tratamento (não finalizado).</summary>
        public static bool IsOpen(LicenseRequestDto r) => r.Status is 1 or 2 or 3;
    }

    // ------------------------------------------------------------------ serviço

    public interface ILicensesService
    {
        /// <summary>null = serviço de licenciamento indisponível.</summary>
        Task<LicenseOverviewDto?> GetOverviewAsync();
        Task<LicenseDto?> GetByIdAsync(Guid id);
        Task<List<LicenseRequestDto>?> GetRequestsAsync();
        Task<List<LicenseRequestDto>?> GetHistoryAsync(Guid licenseId);
        Task<LicenseActionResult> RequestAsync(RequestLicenseForm form);
        Task<LicenseActionResult> RenewAsync(Guid licenseId, RenewLicenseForm form);
    }

    public class LicensesService : ILicensesService
    {
        private const string BaseUrl = "api/client/licenses";
        private readonly HttpClient _http;

        public LicensesService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public Task<LicenseOverviewDto?> GetOverviewAsync() => GetAsync<LicenseOverviewDto>(BaseUrl);

        public Task<LicenseDto?> GetByIdAsync(Guid id) => GetAsync<LicenseDto>($"{BaseUrl}/{id}");

        public Task<List<LicenseRequestDto>?> GetRequestsAsync() =>
            GetAsync<List<LicenseRequestDto>>($"{BaseUrl}/requests");

        public Task<List<LicenseRequestDto>?> GetHistoryAsync(Guid licenseId) =>
            GetAsync<List<LicenseRequestDto>>($"{BaseUrl}/{licenseId}/history");

        public Task<LicenseActionResult> RequestAsync(RequestLicenseForm form) =>
            PostAsync($"{BaseUrl}/request", new { form.ProductName, form.Users, form.Notes });

        public Task<LicenseActionResult> RenewAsync(Guid licenseId, RenewLicenseForm form) =>
            PostAsync($"{BaseUrl}/{licenseId}/renew", new { form.Months, form.Notes });

        private async Task<T?> GetAsync<T>(string url) where T : class
        {
            try
            {
                var response = await _http.GetAsync(url);
                return response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<T>()
                    : null;
            }
            catch (Exception ex) when (ex is HttpRequestException or JsonException)
            {
                return null;
            }
        }

        private async Task<LicenseActionResult> PostAsync(string url, object payload)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, payload);
                if (response.IsSuccessStatusCode)
                    return new LicenseActionResult(true);

                string? message = null;
                try
                {
                    using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                    if (doc.RootElement.TryGetProperty("message", out var m))
                        message = m.GetString();
                }
                catch (JsonException) { /* corpo sem JSON */ }

                return new LicenseActionResult(false, message);
            }
            catch (Exception ex) when (ex is HttpRequestException or JsonException)
            {
                return new LicenseActionResult(false);
            }
        }
    }
}
