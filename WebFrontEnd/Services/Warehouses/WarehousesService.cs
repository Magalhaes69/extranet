using System.Net.Http.Json;
using WebFrontEnd.Services.VatGroups.DTO;
using WebFrontEnd.Services.Warehouses.Dto;

namespace WebFrontEnd.Services.Warehouses
{
    public class WarehousesService : IWarehousesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Warehouses";

        private List<WarehousesDto> _cache;
        public Action WarehousesDtoAction { get; set; }

        public WarehousesService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public Task<WarehousesDto> GetByCodeAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WarehousesDto>> GetWarehousesServiceAsync(string Inactive)
        {
            if (_cache is not null)
                return _cache;
            var response = await _httpClient.GetAsync($"{BaseUrl}/list?Inactive={Inactive}");

            if (!response.IsSuccessStatusCode)
                return new List<WarehousesDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<WarehousesDto>>()
                     ?? new List<WarehousesDto>();

            return _cache;
        }
    }
}
