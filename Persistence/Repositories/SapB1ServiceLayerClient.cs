
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Application.Dtos.Orders;
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
            if (!string.IsNullOrEmpty(_sessionId))
            {
                await _httpClient.PostAsync("Logout", null);
                _sessionId = null!;
            }
        }

        public async Task<List<T>> GetAllAsync<T>(string objectType, string? odataFilter = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await LoginAsync(cancellationToken);

                string url = objectType;
                if (!string.IsNullOrEmpty(odataFilter))
                    url += $"?$filter={odataFilter}";

                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();


                var wrapper = await response.Content.ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

                return wrapper?.Value ?? new List<T>();
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }
    }
}
