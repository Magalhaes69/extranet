using System.Net.Http.Json;
using WebFrontEnd.Services.VatGroups.DTO;
using WebFrontEnd.Services.Warehouses.Dto;

namespace WebFrontEnd.Services.Warehouses
{
    public class WarehousesService : IWarehousesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Warehouses";

        private WarehousesResultDto _cache;
        public Action WarehousesDtoAction { get; set; }

        public WarehousesService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public Task<WarehousesDto> GetByCodeAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<WarehousesResultDto> GetWarehousesServiceAsync(
            WarehousesRequestDto requestDto,
            bool forceRefresh = false
            )
        {
            if (!forceRefresh)
                return _cache;

            var serialized = General.GennerateStringContent(General.SerializeObject(requestDto));
            var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

            if (!response.IsSuccessStatusCode)
                return new WarehousesResultDto();
            _cache = await response.Content.ReadFromJsonAsync<WarehousesResultDto>()
                     ?? new WarehousesResultDto();

            return _cache;
        }
    }
}
