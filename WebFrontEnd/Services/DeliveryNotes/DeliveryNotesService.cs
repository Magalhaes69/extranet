using System.Net.Http.Json;
using WebFrontEnd.Services.DeliveryNotes.Dto;

namespace WebFrontEnd.Services.DeliveryNotes
{
    public class DeliveryNotesService : IDeliveryNotesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/DeliveryNotes";
        private List<DeliveryNotesViewDto>? _cache;
        private DeliveryNotesDetailsDto delivery;
        

        public Action DeliveryNotesAction { get; set; }

        public DeliveryNotesService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<List<DeliveryNotesViewDto>> GetDeliveryNotesAsync(string cardCode, bool forceRefresh = false)
        {
            if (_cache is not null && !forceRefresh)
                return _cache;

            var response = await _httpClient.GetAsync($"{BaseUrl}/list?CardCode={cardCode}");

            if (!response.IsSuccessStatusCode)
                return new List<DeliveryNotesViewDto>();

            _cache = await response.Content.ReadFromJsonAsync<List<DeliveryNotesViewDto>>()
                     ?? new List<DeliveryNotesViewDto>();

            return _cache;
        }

        public async Task<DeliveryNotesDetailsDto> GetByDocEntryDeliveryNotesAsync(string cardCode, int DocEntry, bool forceRefresh = false)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/details?CardCode={cardCode}&DocEntry={DocEntry}");

            if ((!response.IsSuccessStatusCode))
            {
                return null!;
            }

            delivery = await response.Content.ReadFromJsonAsync<DeliveryNotesDetailsDto>()
                     ?? new DeliveryNotesDetailsDto();

            return delivery;
        }
    }
}
