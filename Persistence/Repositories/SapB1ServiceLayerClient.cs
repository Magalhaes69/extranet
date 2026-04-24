using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Application.Dtos.Orders;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence.Repositories
{
    public class SapB1ServiceLayerClient : ISapClient
    {
        private HttpClient _httpClient;
        private readonly SapSettings _settings;
        private string _sessionId = string.Empty;

        public SapB1ServiceLayerClient(IHttpClientFactory factory,
            IOptions<SapSettings> options)
        {
            _httpClient = factory.CreateClient("SAP");
            _settings = options.Value;
        }
        public async Task<T> AddAsync<T>(string objectType, object obj, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync(
            $"{objectType}",
            new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json"),
            cancellationToken);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(json)!;
        }

        
        public async Task<T> GetAsync<T>(string objectType, string odataFilter, CancellationToken cancellationToken) where T : class
        {
            await LoginAsync(cancellationToken);

            string url = objectType;
            if (!string.IsNullOrEmpty(odataFilter))
                url += $"?$filter={odataFilter}&$top=1";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();


            var wrapper = await response.Content.ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

            return wrapper?.Value?.FirstOrDefault()!;
        }

        public async Task<bool> LoginAsync(CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                UserName = _settings.UserName,
                Password = _settings.Password,
                CompanyDB = _settings.CompanyDB
            };

            var response = await _httpClient.PostAsync(
                "Login",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"),
                cancellationToken);

            if (!response.IsSuccessStatusCode) return false;

            if (response.Headers.TryGetValues("B1SESSION", out var values))
                _sessionId = values.First();

            _httpClient.DefaultRequestHeaders.Remove("B1SESSION");
            _httpClient.DefaultRequestHeaders.Add("B1SESSION", _sessionId);

            return true;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("Logout", null);
            _sessionId = null!;
        }

        public async Task<List<T>> GetAllAsync<T>(string objectType, string? odataFilter = null, CancellationToken cancellationToken = default)
        {
            var result = new List<T>();
            int skip = 0;
            int top = 20;
            try
            {
                await LoginAsync(cancellationToken);

                while (true)
                {
                    string url = $"{objectType}?$top={top}&$skip={skip}";

                    if (!string.IsNullOrEmpty(odataFilter))
                        url += $"&$filter={odataFilter}";

                    var response = await _httpClient.GetAsync(url, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var wrapper = await response.Content
                        .ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

                    var data = wrapper?.Value ?? new List<T>();

                    if (!data.Any())
                        break;

                    result.AddRange(data);

                    if (data.Count < top)
                        break;

                    skip += top;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<T>> GetAllB1Async<T>(
                                    string objectType,
                                    string? filter = null,
                                    Dictionary<string, string>? queryParams = null,
                                    CancellationToken cancellationToken = default)
        {
            var result = new List<T>();
            int skip = 0;
            int top = 20;

            await LoginAsync(cancellationToken);

            while (true)
            {
                var query = new List<string>
                                    {
                                        $"$top={top}",
                                        $"$skip={skip}"
                                    };

                if (!string.IsNullOrEmpty(filter))
                    query.Add($"$filter={filter}");

                if (queryParams != null)
                {
                    foreach (var param in queryParams)
                        query.Add($"{param.Key}={param.Value}");
                }

                string url = $"{objectType}?{string.Join("&", query)}";

                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var wrapper = await response.Content
                    .ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

                var data = wrapper?.Value ?? new List<T>();

                if (!data.Any())
                    break;

                result.AddRange(data);

                if (data.Count < top)
                    break;

                skip += top;
            }

            return result;
        }

        public async Task<GetAttachments2> GetAttachments2Async(string url, CancellationToken cancellationToken)
        {
            await LoginAsync(cancellationToken);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var originalStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            // ✅ FIX: copy to MemoryStream
            var memoryStream = new MemoryStream();
            await originalStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            return new GetAttachments2
            {
                Stream = memoryStream,
                ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream",
                FileName = $"attachment_{DateTime.Now.Ticks}.pdf"
            };
        }

        public async Task<T> GetB1Async<T>(string objectType, string? filter, Dictionary<string, string>? queryParams, CancellationToken cancellationToken)
        {
            await LoginAsync(cancellationToken);
            int skip = 0;
            int top = 20;
            var query = new List<string>
                                    {
                                        $"$top={top}",
                                        $"$skip={skip}"
                                    };

            if (!string.IsNullOrEmpty(filter))
                query.Add($"$filter={filter}");

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                    query.Add($"{param.Key}={param.Value}");
            }

            string url = $"{objectType}?{string.Join("&", query)}";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var wrapper = await response.Content
                .ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

            var data = wrapper!.Value.FirstOrDefault()!;

            return data;
        }

        public async Task<List<T>> GetPagedB1Async<T>(string objectType, 
            int page, 
            int pageSize = 20, 
            string? filter = null, 
            Dictionary<string, 
            string>? queryParams = null, 
            CancellationToken cancellationToken = default)
        {
            await LoginAsync(cancellationToken);

            int skip = (page - 1) * pageSize;

            var query = new List<string>
            {
                $"$top={pageSize}",
                $"$skip={skip}"
            };

            if (!string.IsNullOrEmpty(filter))
                query.Add($"$filter={filter}");

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                    query.Add($"{param.Key}={param.Value}");
            }

            string url = $"{objectType}?{string.Join("&", query)}";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var wrapper = await response.Content
                .ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

            return wrapper?.Value ?? new List<T>();
        }
        public async Task<(List<T> Data, int TotalCount)> GetPagedBL1Async<T>(
        string objectType,
        int page,
        int pageSize = 100,
        string? filter = null,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
        {
            await LoginAsync(cancellationToken);

            var result = new List<T>();

            int sapPageSize = 20;
            int targetCount = pageSize;

            int skip = (page - 1) * pageSize;

            int total = 0;
            bool firstCall = true;

            while (result.Count < targetCount)
            {
                var query = new List<string>
                {
                    $"$top={sapPageSize}",
                    $"$skip={skip}"
                };

                if (firstCall)
                    query.Add("$inlinecount=allpages");

                if (!string.IsNullOrEmpty(filter))
                    query.Add($"$filter={filter}");

                if (queryParams != null)
                {
                    foreach (var param in queryParams)
                        query.Add($"{param.Key}={param.Value}");
                }

                string url = $"{objectType}?{string.Join("&", query)}";

                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var wrapper = await response.Content
                    .ReadFromJsonAsync<SapIEnumerableDto<T>>(cancellationToken: cancellationToken);

                var data = wrapper?.Value ?? new List<T>();

                if (firstCall)
                {
                    total = wrapper?.Count ?? 0;
                    firstCall = false;
                }

                if (!data.Any())
                    break;

                result.AddRange(data);

                skip += sapPageSize;

                if (data.Count < sapPageSize)
                    break;
            }


            return (result, total);
        }

        public async Task<T> GetTotalsAsync<T>(
            string objectType,
            string filter,
            CancellationToken cancellationToken)
        {
            
                var url = $"{objectType}?$apply=aggregate(CashSum with sum as TotalCashSum, TransferSum with sum as TotalTransferSum)";

                if (!string.IsNullOrEmpty(filter))
                    url += $"&$filter={filter}";

                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var wrapper = await response.Content
                    .ReadFromJsonAsync<SapIEnumerableDto<T>>(cancellationToken: cancellationToken);

                var data = wrapper!.Value.FirstOrDefault()!;

               
            return data;
        }
        public async Task GetBackgroundB1Async<T>(string objectType, Action<List<T>> onPageLoaded, string? filter = null, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
        {
            int page = 1;
            int pageSize = 20;

            await LoginAsync(cancellationToken);

            while (true)
            {
                var data = await GetPagedB1Async<T>(
                    objectType,
                    page,
                    pageSize,
                    filter,
                    queryParams,
                    cancellationToken);

                if (data == null || data.Count == 0)
                    break;

                // return data progressively
                onPageLoaded?.Invoke(data);

                if (data.Count < pageSize)
                    break;

                page++;
            }
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string queryName, string objectCode, CancellationToken cancellationToken)
        {
            await LoginAsync(cancellationToken);
            string url = $"SQLQueries('{queryName}')/List?ObjectCode={objectCode}";

            var response = await _httpClient.GetAsync(url,cancellationToken);
            if (!response.IsSuccessStatusCode)
                return default!;

            var result = await response.Content
                .ReadFromJsonAsync<SapIEnumerableDto<T>>(cancellationToken: cancellationToken);

            return result?.Value ?? new List<T>();
        }
    }
}
