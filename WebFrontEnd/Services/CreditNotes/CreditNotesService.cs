using System.Net.Http.Json;
using WebFrontEnd.Services.CreditNotes.Dto;
using WebFrontEnd.Services.Quotations.Dto;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.CreditNotes
{
    public class CreditNotesService : ICreditNotesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/CreditNotes";
        private List<CreditNotesViewDto>? _cache;
        private CreditNotesDetailsDto? credit;

        public Action CreditNotesAction { get; set; }

        public CreditNotesService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }
        public async Task<CreditNotesDetailsDto> GetByDocEntryCreditNotesAsync(string cardCode, int DocEntry, bool forceRefresh = false)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/details?CardCode={cardCode}&DocEntry={DocEntry}");

            if ((!response.IsSuccessStatusCode))
            {
                return null!;
            }

            credit = await response.Content.ReadFromJsonAsync<CreditNotesDetailsDto>()
                     ?? new CreditNotesDetailsDto();

            return credit;
        }

        public async Task<List<CreditNotesViewDto>> GetCreditNotesAsync(CreditRequest request, bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

                if (!response.IsSuccessStatusCode)
                    return new List<CreditNotesViewDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<CreditNotesViewDto>>()
                         ?? new List<CreditNotesViewDto>();

                return _cache;
            }
            catch
            {
                return new List<CreditNotesViewDto>();
            }
        }

        
    }
}
