using System.Net.Http.Json;

using WebFrontEnd.Services.Quotations.Dto;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.Quotations
{
    public class QuotationsService : IQuotationsService
    {
        public Action QuotationsAction { get; set; }

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Quotations";
        private List<QuotationsDto>? _cache;
        private QuotationsDto quotations;

        public QuotationsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public async Task<List<QuotationsDto>> GetQuotationsAsync(
            QuotationRequest quotation, 
            bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                var serialized = General.GennerateStringContent(General.SerializeObject(quotation));
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

                if (!response.IsSuccessStatusCode)
                    return new List<QuotationsDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<QuotationsDto>>()
                         ?? new List<QuotationsDto>();

                return _cache;
            }
            catch
            {
                return new List<QuotationsDto>();
            }
        }

        public async Task<QuotationsDto> GetByDocEntryQuotationsAsync(string cardCode, int DocEntry, bool forceRefresh = false)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/details?CardCode={cardCode}&DocEntry={DocEntry}");

            if (!response.IsSuccessStatusCode)
                return new QuotationsDto();

            quotations = await response.Content.ReadFromJsonAsync<QuotationsDto>()
                     ?? new QuotationsDto();

            return quotations;
        }
    }
}
