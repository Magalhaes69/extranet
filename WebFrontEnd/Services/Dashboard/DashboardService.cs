
using System.Net.Http.Json;
using WebFrontEnd.Services.Dashboard.Dtos;

namespace WebFrontEnd.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Dashboard";

        public DashboardService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
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
    }
}
