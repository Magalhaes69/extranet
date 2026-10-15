using System.Net.Http.Json;
using WebFrontEnd.Services.AppSettings.Dtos;

namespace WebFrontEnd.Services.AppSettings
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/AppSettings";
        private List<AppSettingViewDto>? _cache;

        public AppSettingsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public async Task<List<AppSettingViewDto>> GetAllAppSettings()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/list");

                if (!response.IsSuccessStatusCode)
                    return new List<AppSettingViewDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<AppSettingViewDto>>()
                         ?? new List<AppSettingViewDto>();

                return _cache;
            }
            catch
            {
                return new List<AppSettingViewDto>();
            }
        }
    }
}
