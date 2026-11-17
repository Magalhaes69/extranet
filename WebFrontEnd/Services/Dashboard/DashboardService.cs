
using System.Net.Http.Json;
using WebFrontEnd.Services.Dashboard.Dtos;

namespace WebFrontEnd.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Dashboard";

        public DashboardService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public async Task<GetCountDto> GetCountOrderAsync(string CardCode, string Status)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/summary");

            if (!response.IsSuccessStatusCode)
                return new GetCountDto();
           var get= await response.Content.ReadFromJsonAsync<GetCountDto>()
                                 ?? new GetCountDto();

            return get;
        }

        public async Task<DashboardMetricDto?> GetMetricAsync(string key, CancellationToken ct = default)
        {
            using var response = await _httpClient.GetAsync($"{BaseUrl}/metrics/{key}", ct);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                Console.WriteLine($"Dashboard metric '{key}' failed ({(int)response.StatusCode}): {body}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<DashboardMetricDto>(cancellationToken: ct);
        }
    }
}
