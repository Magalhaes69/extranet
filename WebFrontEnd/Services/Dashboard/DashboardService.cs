using System.Net.Http.Json;
using WebFrontEnd.Services.Dashboard.Dtos;

namespace WebFrontEnd.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/client/dashboard";

        public DashboardService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ClientDashboardDto?> GetClientDashboardAsync(CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseUrl, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Dashboard failed ({(int)response.StatusCode}): {body}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<ClientDashboardDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar dashboard: {ex.Message}");
                return null;
            }
        }

        public async Task<DashboardMetricDto?> GetMetricAsync(string key, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/metrics/{key}", ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Dashboard metric '{key}' failed ({(int)response.StatusCode}): {body}");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<DashboardMetricDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar métrica '{key}': {ex.Message}");
                return null;
            }
        }
    }
}

