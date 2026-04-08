using WebFrontEnd.Services.DeliveryNotes.Dto;
using WebFrontEnd.Services.Invoices.Dto;


namespace WebFrontEnd.Services.DeliveryNotes
{
    public interface IDeliveryNotesService
    {
        Task<List<DeliveryNotesViewDto>> GetDeliveryNotesAsync(string cardCode, bool forceRefresh = false);
        Task<DeliveryNotesDetailsDto> GetByDocEntryDeliveryNotesAsync(string cardCode, int DocEntry, bool forceRefresh = false);
        Action DeliveryNotesAction { get; set; }
    }
}
