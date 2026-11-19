using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebFrontEnd.Services.CompanyUsers
{
    /// <summary>Processos que o gestor pode atribuir. As chaves têm de coincidir com Domain.Security.AppProcesses.</summary>
    public static class CompanyProcesses
    {
        public record Info(string Key, string Icon, string LabelKey, string DescriptionKey);

        public static readonly IReadOnlyList<Info> All = new List<Info>
        {
            new("dashboard", "bi-speedometer2",       "processDashboard", "processDashboardDesc"),
            new("requests",  "bi-file-earmark-text",  "processRequests",  "processRequestsDesc"),
            new("payments",  "bi-credit-card",        "processPayments",  "processPaymentsDesc"),
            new("licenses",  "bi-patch-check",        "processLicenses",  "processLicensesDesc"),
        };

        public static Info? Find(string key) => All.FirstOrDefault(p => p.Key == key);
    }

    public class CompanyUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsManager { get; set; }
        public List<string> Processes { get; set; } = new();
        public DateTimeOffset DateCreated { get; set; }
    }

    /// <summary>Modelo único dos formulários de criar e editar.</summary>
    public class CompanyUserFormModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(120, ErrorMessage = "Máximo de 120 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O apelido é obrigatório.")]
        [StringLength(60, ErrorMessage = "Máximo de 60 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "Máximo de 30 caracteres.")]
        public string? PhoneNumber { get; set; }

        /// <summary>Na criação é obrigatória; na edição só redefine se preenchida.</summary>
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public bool IsActive { get; set; } = true;
        public HashSet<string> Processes { get; set; } = new();

        public static CompanyUserFormModel From(CompanyUserDto dto) => new()
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsActive = dto.IsActive,
            Processes = dto.Processes.ToHashSet()
        };
    }

    public record ServiceResult(bool Succeeded, string? Message = null);

    public interface ICompanyUsersService
    {
        Task<List<CompanyUserDto>?> GetAllAsync();
        Task<CompanyUserDto?> GetByIdAsync(string id);
        Task<ServiceResult> CreateAsync(CompanyUserFormModel model);
        Task<ServiceResult> UpdateAsync(CompanyUserFormModel model);
    }

    public class CompanyUsersService : ICompanyUsersService
    {
        private const string BaseUrl = "api/client/company-users";
        private readonly HttpClient _http;

        public CompanyUsersService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API");
        }

        public async Task<List<CompanyUserDto>?> GetAllAsync()
        {
            try
            {
                var response = await _http.GetAsync(BaseUrl);
                if (!response.IsSuccessStatusCode)
                    return null;
                return await response.Content.ReadFromJsonAsync<List<CompanyUserDto>>() ?? new();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<CompanyUserDto?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _http.GetAsync($"{BaseUrl}/{Uri.EscapeDataString(id)}");
                if (!response.IsSuccessStatusCode)
                    return null;
                return await response.Content.ReadFromJsonAsync<CompanyUserDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<ServiceResult> CreateAsync(CompanyUserFormModel model)
        {
            var payload = new
            {
                model.FirstName,
                model.LastName,
                model.Email,
                model.PhoneNumber,
                Password = model.Password ?? string.Empty,
                Processes = model.Processes.ToList()
            };
            return await SendAsync(() => _http.PostAsJsonAsync(BaseUrl, payload));
        }

        public async Task<ServiceResult> UpdateAsync(CompanyUserFormModel model)
        {
            var payload = new
            {
                model.FirstName,
                model.LastName,
                model.PhoneNumber,
                model.IsActive,
                Processes = model.Processes.ToList(),
                NewPassword = string.IsNullOrWhiteSpace(model.Password) ? null : model.Password
            };
            return await SendAsync(() => _http.PutAsJsonAsync($"{BaseUrl}/{Uri.EscapeDataString(model.Id)}", payload));
        }

        private static async Task<ServiceResult> SendAsync(Func<Task<HttpResponseMessage>> send)
        {
            try
            {
                var response = await send();
                if (response.IsSuccessStatusCode)
                    return new ServiceResult(true);

                string? message = null;
                try
                {
                    using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                    if (doc.RootElement.TryGetProperty("message", out var m))
                        message = m.GetString();
                }
                catch (JsonException) { /* corpo sem JSON: usa a mensagem por omissão */ }

                return new ServiceResult(false, message);
            }
            catch (HttpRequestException)
            {
                return new ServiceResult(false, null);
            }
        }
    }
}
