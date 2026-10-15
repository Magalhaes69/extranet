
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
                return new List<GetSeriesDynamicResult>();

            _cache = await response.Content.ReadFromJsonAsync<List<GetSeriesDynamicResult>>()
                     ?? new List<GetSeriesDynamicResult>();

            return _cache;
        }

        public async Task<ContactEmployeesResponse> ExecuteQueryContactAsync(
            ContactEmployeesRequest request)
        {
            var serialized = General.GennerateStringContent(General.SerializeObject(request));
            var response = await _httpClient.PostAsync($"{BaseUrl}/contact", serialized);

            if (!response.IsSuccessStatusCode)
                return new ContactEmployeesResponse();

            var result = await response.Content.ReadFromJsonAsync<ContactEmployeesResponse>()
                     ?? new ContactEmployeesResponse();

            return result;
        }
    }
}
