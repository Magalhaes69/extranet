
using System.Net.Http.Json;
using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Services.SQLQueries.Dto;

namespace WebFrontEnd.Services.SQLQueries
{
    public class SQLQueriesService : ISQLQueriesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/SQLQueries";
        private List<GetSeriesDynamicResult> _cache;
        public SQLQueriesService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }

        public async Task<List<GetSeriesDynamicResult>> ExecuteQueryAsync(
            GetSeriesDynamicRequest request)
        {
            var serialized = General.GennerateStringContent(General.SerializeObject(request));
            var response = await _httpClient.PostAsync($"{BaseUrl}/series", serialized);

            if (!response.IsSuccessStatusCode)
                return null!;

            _cache = await response.Content.ReadFromJsonAsync<List<GetSeriesDynamicResult>>()
                     ?? null!;

            return _cache;
        }
    }
}
