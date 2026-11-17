using System.Net.Http.Json;
using WebFrontEnd.Services.Items.DTO;
using WebFrontEnd.Services.VatGroups.DTO;

namespace WebFrontEnd.Services.VatGroups
{
    public class VatGroupsService : IVatGroupsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/VatGroups";

        private VatGroupsResultDto _cache;
        public VatGroupsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public Action IVatGroupsAction { get; set; }

        public Task<VatGroupsDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<VatGroupsResultDto> GetVatGroupsAsync(
            VatGroupsRequestDto request,
            bool forceRefresh = false)
        {
            if (!forceRefresh)
                return _cache;
            var serialized = General.GennerateStringContent(General.SerializeObject(request));
            var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

            if (!response.IsSuccessStatusCode)
                return new VatGroupsResultDto { 
                    Items = new List<VatGroupsDto>(),
                    TotalCount = 0 
                };

            _cache = await response.Content.ReadFromJsonAsync<VatGroupsResultDto>()
                     ?? new VatGroupsResultDto { 
                         Items = new List<VatGroupsDto>(), 
                         TotalCount = 0 
                     };

            return _cache;
        }
    }
}
