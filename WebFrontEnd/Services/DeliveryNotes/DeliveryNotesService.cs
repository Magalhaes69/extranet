using System.Net.Http.Json;
using WebFrontEnd.Services.DeliveryNotes.Dto;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.DeliveryNotes
{
    public class DeliveryNotesService : IDeliveryNotesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/DeliveryNotes";
        private List<DeliveryNotesViewDto>? _cache;
        private DeliveryNotesDetailsDto delivery;
        

        public Action DeliveryNotesAction { get; set; }

        public DeliveryNotesService(IHttpClientFactory factory)
        {
            this._httpClient = factory.CreateClient("API");
        }
        public async Task<List<DeliveryNotesViewDto>> GetDeliveryNotesAsync(DeliveryNotesRequest request, bool forceRefresh = false)
        {
            try
            {
                if (_cache is not null && !forceRefresh)
                    return _cache;

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync($"{BaseUrl}/list", serialized);

                if (!response.IsSuccessStatusCode)
                    return new List<DeliveryNotesViewDto>();

                _cache = await response.Content.ReadFromJsonAsync<List<DeliveryNotesViewDto>>()
                         ?? new List<DeliveryNotesViewDto>();

                return _cache;
            }
            catch
            {
                return new List<DeliveryNotesViewDto>();
            }
            
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
