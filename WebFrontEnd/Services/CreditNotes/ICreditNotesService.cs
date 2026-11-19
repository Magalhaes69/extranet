using WebFrontEnd.Services.CreditNotes.Dto;
using WebFrontEnd.Shared.Dtos;


namespace WebFrontEnd.Services.CreditNotes
{
    public interface ICreditNotesService
    {
        Task<List<CreditNotesViewDto>> GetCreditNotesAsync(CreditRequest request, bool forceRefresh = false);
        Task<CreditNotesDetailsDto> GetByDocEntryCreditNotesAsync(string cardCode, int DocEntry, bool forceRefresh = false);
        Action CreditNotesAction { get; set; }
    }
}
