using System.Net.Http.Json;
using WebFrontEnd.Services.Items.DTO;
using WebFrontEnd.Services.VatGroups.DTO;

namespace WebFrontEnd.Services.VatGroups
{
    public class VatGroupsService : IVatGroupsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/VatGroups";

        private List<VatGroupsDto> _cache;
        public VatGroupsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public Action IVatGroupsAction { get; set; }

        public Task<VatGroupsDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VatGroupsDto>> GetVatGroupsAsync()
        {
            if (_cache is not null)
                return _cache;

            var inativo = "tNO";
            var response = await _httpClient.GetAsync($"{BaseUrl}/list?Inactive={inativo}");

            if (!response.IsSuccessStatusCode)
                return new List<VatGroupsDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<VatGroupsDto>>()
                     ?? new List<VatGroupsDto>();

            return _cache;
        }
    }
}
