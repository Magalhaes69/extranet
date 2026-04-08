using System.Net.Http.Json;
using WebFrontEnd.Services.CreditNotes.Dto;

namespace WebFrontEnd.Services.CreditNotes
{
    public class CreditNotesService : ICreditNotesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/CreditNotes";
        private List<CreditNotesViewDto>? _cache;
        private CreditNotesDetailsDto? credit;

        public Action CreditNotesAction { get; set; }

        public CreditNotesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<List<CreditNotesViewDto>> GetCreditNotesAsync(string cardCode, bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                var response = await _httpClient.GetAsync($"{BaseUrl}/list?CardCode={cardCode}");

                if (!response.IsSuccessStatusCode)
                    return new List<CreditNotesViewDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<CreditNotesViewDto>>()
                         ?? new List<CreditNotesViewDto>();

                return _cache;
            }
            catch (Exception ex)
            {
                _httpClient.Dispose();

                return new List<CreditNotesViewDto>();
            }
        }

        
    }
}
