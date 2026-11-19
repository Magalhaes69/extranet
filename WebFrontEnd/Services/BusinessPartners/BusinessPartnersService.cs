using System.Net.Http.Json;
using WebFrontEnd.Services.BusinessPartners.Dto;


namespace WebFrontEnd.Services.BusinessPartners
{
    public class BusinessPartnersService : IBusinessPartnersService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/BusinessPartners";

        private BusinessPartnersDto _cache;
        public Action BusinessPartnersAction { get; set; }

        public BusinessPartnersService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }

        public async Task<BusinessPartnersDto> GetBusinessPartnersAsync(string CardCode, string CardType, bool forceRefresh = false)
        {
            if (_cache is not null && !forceRefresh)
                return _cache;

            var response = await _httpClient.GetAsync($"{BaseUrl}/list?CardCode={CardCode}&CardType={CardType}");

            if (!response.IsSuccessStatusCode)
                return new BusinessPartnersDto();

            _cache = await response.Content.ReadFromJsonAsync<BusinessPartnersDto>()
                     ?? new BusinessPartnersDto();

            return _cache;
        }
    }
}
