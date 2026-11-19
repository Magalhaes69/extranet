namespace WebFrontEnd.Services.Leads
{
    using System.Net.Http.Json;
    using Microsoft.Extensions.Configuration;
    using WebFrontEnd.Services.Leads.Dto;

    public interface ILeadsService
    {
        Task<bool> SubmitLeadAsync(LeadFormDto lead);
        Task<LeadResponseDto> CreateAsync(LeadFormDto lead);
    }

    public class LeadsService : ILeadsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public LeadsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("ApiSettings:BaseUrl not configured");
        }

        public Task<bool> SubmitLeadAsync(LeadFormDto lead)
        {
            return Task.FromResult(true);
        }

        public async Task<LeadResponseDto> CreateAsync(LeadFormDto lead)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}api/Leads", new
                {
                    companyName = lead.CompanyName,
                    nuit = lead.Nuit,
                    contactName = lead.ContactName,
                    email = lead.Email,
                    phoneNumber = lead.PhoneNumber,
                    notes = lead.Notes,
                    password = lead.Password
                });

                var contentString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(contentString))
                    {
                        return new LeadResponseDto
                        {
                            Flag = true,
                            Message = "Conta criada com sucesso."
                        };
                    }

                    try
                    {
                        var result = System.Text.Json.JsonSerializer.Deserialize<LeadResponseDto>(
                            contentString,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return result ?? new LeadResponseDto
                        {
                            Flag = true,
                            Message = "Conta criada com sucesso."
                        };
                    }
                    catch
                    {
                        return new LeadResponseDto
                        {
                            Flag = true,
                            Message = "Conta criada com sucesso."
                        };
                    }
                }

                // Erro - tentar ler a mensagem
                if (!string.IsNullOrWhiteSpace(contentString))
                {
                    try
                    {
                        var errorResponse = System.Text.Json.JsonSerializer.Deserialize<LeadResponseDto>(
                            contentString,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (errorResponse != null)
                        {
                            return errorResponse;
                        }
                    }
                    catch
                    {
                        // Se não for JSON válido, retornar mensagem genérica
                    }
                }

                return new LeadResponseDto
                {
                    Flag = false,
                    Message = $"Não foi possível criar a conta. Status: {response.StatusCode}"
                };
            }
            catch (HttpRequestException ex)
            {
                return new LeadResponseDto
                {
                    Flag = false,
                    Message = "Erro de conexão com a API. Verifique se o servidor está a funcionar."
                };
            }
            catch (Exception ex)
            {
                return new LeadResponseDto
                {
                    Flag = false,
                    Message = $"Erro ao criar conta: {ex.Message}"
                };
            }
        }
    }
}

namespace WebFrontEnd.Services.Leads.Dto
{
    public class LeadFormDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Nuit { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
