using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Application.Dtos.Orders;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories
{
    public class B1DashboardService : IB1DashboardService
    {
        private HttpClient _httpClient;
        private readonly SapSettings _settings;

        public B1DashboardService(IHttpClientFactory factory,
            IOptions<SapSettings> options)
        {
            _httpClient = factory.CreateClient("SAP");
            _settings = options.Value;
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


            return true;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("Logout", null);
        }

        public async Task<List<T>> GetListAsync<T>(string endpoint, string filter, CancellationToken cancellationToken)
        {
            try
            {
                await LoginAsync(cancellationToken);

                string url = endpoint;
                if (!string.IsNullOrEmpty(filter))
                    url += $"&$filter={filter}";

                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var wrapper = await response.Content.ReadFromJsonAsync<SapListDto<T>>(cancellationToken: cancellationToken);

                await LogoutAsync();

                return wrapper?.Value ?? new List<T>();

            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }
    }
}
