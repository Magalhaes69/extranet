using WebFrontEnd.Services.DeliveryNotes.Dto;
using WebFrontEnd.Shared.Dtos;


namespace WebFrontEnd.Services.DeliveryNotes
{
    public interface IDeliveryNotesService
    {
        Task<List<DeliveryNotesViewDto>> GetDeliveryNotesAsync(DeliveryNotesRequest request, bool forceRefresh = false);
        Task<DeliveryNotesDetailsDto> GetByDocEntryDeliveryNotesAsync(string cardCode, int DocEntry, bool forceRefresh = false);
        Action DeliveryNotesAction { get; set; }
    }
}
