using System.Net.Http.Json;

using WebFrontEnd.Services.Quotations.Dto;
using WebFrontEnd.Shared.Dtos;
using WebFrontEnd.Shared.Results;

namespace WebFrontEnd.Services.Quotations
{
    public class QuotationsService : IQuotationsService
    {
        public Action QuotationsAction { get; set; }

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Quotations";
        //private List<QuotationsDto>? _cache;
        private QuotationsDto quotations;
        private readonly Dictionary<string, PagedResult<QuotationsDto, TotalResultDto>> _cache = new();
        public QuotationsService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public async Task<PagedResult<QuotationsDto, TotalResultDto>> GetQuotationsAsync(
                QuotationRequest quotation,
                bool forceRefresh = false)
        {
            var body = General.SerializeObject(quotation);   // also used as the cache key

            if (!forceRefresh && _cache.TryGetValue(body, out var cached))
                return cached;

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", General.GennerateStringContent(body));

                if (!response.IsSuccessStatusCode)
                    return PagedResult<QuotationsDto, TotalResultDto>.Empty(quotation.Page, quotation.PageSize);

                var result = await response.Content.ReadFromJsonAsync<PagedResult<QuotationsDto, TotalResultDto>>()
                             ?? PagedResult<QuotationsDto, TotalResultDto>.Empty(quotation.Page, quotation.PageSize);

                _cache[body] = result;
                return result;
            }
            catch
            {
                return PagedResult<QuotationsDto, TotalResultDto>.Empty(quotation.Page, quotation.PageSize);
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
