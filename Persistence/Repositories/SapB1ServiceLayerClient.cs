using Application.Dtos.Orders;
using Application.Shared.Behavior;
using Domain.Helps;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Application.Shared.Behavior.SapB1Exception;
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


        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // don't send nulls to SAP
        };

        public async Task<T> AddAsync<T>(string objectType, object obj, CancellationToken cancellationToken)
        {
            using var content = new StringContent(
                JsonSerializer.Serialize(obj, JsonOptions), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(objectType, content, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw CreateSapException(json, (int)response.StatusCode);

            return JsonSerializer.Deserialize<T>(json, JsonOptions)!;
        }

        private static SapB1Exception CreateSapException(string json, int statusCode)
        {
            try
            {
                var error = JsonSerializer.Deserialize<SapErrorResponse>(json, JsonOptions)?.Error;
                if (error is not null)
                    return new SapB1Exception(
                        error.Code,
                        error.Message?.Value ?? "Unknown SAP B1 error",
                        statusCode);
            }
            catch (JsonException)
            {
                // body wasn't SAP JSON (e.g. proxy/HTML error page)
            }

            return new SapB1Exception(0, string.IsNullOrWhiteSpace(json) ? $"HTTP {statusCode}" : json, statusCode);
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
            await LogoutAsync();    
            return wrapper?.Value?.FirstOrDefault()!;
        }

        //public async Task<bool> LoginAsync(CancellationToken cancellationToken = default)
        //{
        //    var payload = new
        //    {
        //        UserName = _settings.UserName,
        //        Password = _settings.Password,
        //        CompanyDB = _settings.CompanyDB
        //    };

        //    var response = await _httpClient.PostAsync(
        //        "Login",
        //        new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"),
        //        cancellationToken);

        //    if (!response.IsSuccessStatusCode) return false;

        //    if (response.Headers.TryGetValues("B1SESSION", out var values))
        //        _sessionId = values.First();

        //    _httpClient.DefaultRequestHeaders.Remove("B1SESSION");
        //    _httpClient.DefaultRequestHeaders.Add("B1SESSION", _sessionId);

        //    return true;
        //}
        public Task<bool> LoginAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
        //public async Task LogoutAsync()
        //{
        //    await _httpClient.PostAsync("Logout", null);
        //    _sessionId = null!;
        //}
        public Task LogoutAsync() => Task.CompletedTask;
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
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<(List<T> Data, int TotalCount)> GetPagedBL1Async<T>(
        string objectType,
        int page,
        int pageSize = 100,
        string? filter = null,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
        {
            //await LoginAsync(cancellationToken);

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

        public async Task<List<T>> ExecuteQueryAsync<T>(string queryName, string objectCode, string field, CancellationToken cancellationToken)
        {
            await LoginAsync(cancellationToken);
            string url = $"SQLQueries('{queryName}')/List?{field}={objectCode}";

            var response = await _httpClient.GetAsync(url,cancellationToken);
            if (!response.IsSuccessStatusCode)
                return default!;

            var result = await response.Content
                .ReadFromJsonAsync<SapIEnumerableDto<T>>(cancellationToken: cancellationToken);

            await LogoutAsync();
            return result?.Value ?? new List<T>();
        }

        //public async Task<T> ExecuteQueryObjAsync<T>(
        //    string queryName, 
        //    Dictionary<string, object>? parameters, CancellationToken cancellationToken) where T : class
        //{
        //    await LoginAsync(cancellationToken);
        //    var queryParams = new List<string>();
        //    if(parameters != null)
        //    {
        //        foreach (var param in parameters)
        //        {
        //            string value = param.Value is string str ? $"'{str}'" : param.Value.ToString()!;
        //            queryParams.Add($"{param.Key}={value}");
        //        }
        //    }
        //    var queryString = string.Join("&", queryParams);
        //    string url = $"SQLQueries('{queryName}')/List?{queryString}";

        //    var response = await _httpClient.GetAsync(url, cancellationToken);
        //    if (!response.IsSuccessStatusCode)
        //        return default!;

        //    var result = await response.Content
        //        .ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

        //    await LogoutAsync();

        //    return result?.Value?.FirstOrDefault()!;
        //}
        public async Task<T> ExecuteQueryObjAsync<T>(
                string queryName,
                Dictionary<string, object>? parameters,
                CancellationToken cancellationToken) where T : class
        {
            try
            {
                var queryParams = new List<string>();

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        var value = param.Value switch
                        {
                            null => "''",
                            string str => $"'{str.Replace("'", "''")}'",
                            DateTime dt => $"'{dt:yyyy-MM-dd}'",
                            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                            _ => $"'{param.Value.ToString()!.Replace("'", "''")}'"
                        };

                        // Encode so '%', '&', spaces etc. reach SAP intact
                        queryParams.Add($"{param.Key}={Uri.EscapeDataString(value)}");
                    }
                }

                var url = $"SQLQueries('{queryName}')/List";
                if (queryParams.Count > 0)
                    url += "?" + string.Join("&", queryParams);

                using var response = await _httpClient.GetAsync(url, cancellationToken);
                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                    throw CreateSapException(json, (int)response.StatusCode);

                var result = JsonSerializer.Deserialize<SapListDto<T>>(json, JsonOptions);
                return result?.Value?.FirstOrDefault()!;
            }
            finally
            {
                await LogoutAsync();
            }
        }
        public async Task<T?> TryGetAsync<T>(string path, CancellationToken cancellationToken) where T : class
        {
            using var response = await _httpClient.GetAsync(path, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw CreateSapException(json, (int)response.StatusCode);

            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }

        public async Task PatchAsync(string path, object obj, CancellationToken cancellationToken)
        {
            using var content = new StringContent(
                JsonSerializer.Serialize(obj, JsonOptions), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PatchAsync(path, content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                throw CreateSapException(json, (int)response.StatusCode);
            }
        }

        private static string? _localCurrency;
        public async Task<string> GetLocalCurrencyAsync(CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_localCurrency))
                return _localCurrency;

           
            try
            {
                using var response = await _httpClient.PostAsync("CompanyService_GetAdminInfo", null, cancellationToken);
                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                    throw CreateSapException(json, (int)response.StatusCode);

                var info = JsonSerializer.Deserialize<SapAdminInfoDto>(json, JsonOptions);
                _localCurrency = info?.LocalCurrency ?? string.Empty;
                return _localCurrency;
            }
            finally
            {
                await LogoutAsync();
            }
        }

        public async Task<int> CountAsync(string entity, string? filter, CancellationToken ct)
        {
            var url = $"{entity}/$count";
            if (!string.IsNullOrWhiteSpace(filter))
                url += $"?$filter={Uri.EscapeDataString(filter)}";

            using var response = await _httpClient.GetAsync(url, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw CreateSapException(body, (int)response.StatusCode);

            return int.TryParse(body.Trim(), out var count) ? count : 0;
        }
        //public async Task<int> CountAsync(string entity, string? filter, CancellationToken ct)
        //{
        //    var url = $"{entity}?$select=DocEntry&$top=0&$inlinecount=allpages";
        //    if (!string.IsNullOrWhiteSpace(filter))
        //        url += $"&$filter={Uri.EscapeDataString(filter)}";

        //    using var response = await _httpClient.GetAsync(url, ct);
        //    var body = await response.Content.ReadAsStringAsync(ct);

        //    if (!response.IsSuccessStatusCode)
        //        throw CreateSapException(body, (int)response.StatusCode);

        //    using var doc = JsonDocument.Parse(body);
        //    return doc.RootElement.TryGetProperty("odata.count", out var count)
        //        ? int.Parse(count.GetString() ?? count.GetRawText())
        //        : 0;
        //}
        public async Task<decimal[]> SumAsync(string entity, IReadOnlyList<string> fields, string? filter, CancellationToken ct)
        {
            var aggregate = string.Join(", ", fields.Select((f, i) => $"{f} with sum as S{i}"));
            var url = $"{entity}?$apply=aggregate({aggregate})";
            if (!string.IsNullOrWhiteSpace(filter))
                url += $"&$filter={Uri.EscapeDataString(filter)}";

            using var response = await _httpClient.GetAsync(url, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
                throw CreateSapException(body, (int)response.StatusCode);

            using var doc = JsonDocument.Parse(body);
            var row = doc.RootElement.GetProperty("value").EnumerateArray().FirstOrDefault();

            return fields.Select((_, i) =>
                row.ValueKind == JsonValueKind.Object &&
                row.TryGetProperty($"S{i}", out var v) &&
                v.ValueKind == JsonValueKind.Number
                    ? v.GetDecimal()
                    : 0m).ToArray();
        }

        public async Task<List<T>> ExecuteQueryListAsync<T>(
                     string queryName,
                     Dictionary<string, object>? parameters,
                     CancellationToken cancellationToken) where T : class
        {
            var queryParams = new List<string>();

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    var value = param.Value switch
                    {
                        null => "''",
                        string str => $"'{str.Replace("'", "''")}'",
                        DateTime dt => $"'{dt:yyyy-MM-dd}'",
                        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                        _ => $"'{param.Value.ToString()!.Replace("'", "''")}'"
                    };
                    queryParams.Add($"{param.Key}={Uri.EscapeDataString(value)}");
                }
            }

            var url = $"SQLQueries('{queryName}')/List";
            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw CreateSapException(json, (int)response.StatusCode);

            var result = JsonSerializer.Deserialize<SapListDto<T>>(json, JsonOptions);
            return result?.Value ?? new List<T>();
        }
    }
}
