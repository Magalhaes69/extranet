
using System.IO;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading;
using Application.Dtos.Orders;
using Application.UseCases.Attachments2.GetAttachment;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

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

        //public async Task<List<T>> GetAllAsync<T>(string objectType, CancellationToken cancellationToken)
        //{
        //    var response = await _httpClient.GetAsync($"{objectType}", cancellationToken);
        //    response.EnsureSuccessStatusCode();
        //    var json = await response.Content.ReadAsStringAsync(cancellationToken);
        //    return JsonSerializer.Deserialize<List<T>>(json)!;
        //}

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
    }
}
