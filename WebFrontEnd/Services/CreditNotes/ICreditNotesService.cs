using WebFrontEnd.Services.CreditNotes.Dto;


namespace WebFrontEnd.Services.CreditNotes
{
    public interface ICreditNotesService
    {
        Task<List<CreditNotesViewDto>> GetCreditNotesAsync(string cardCode, bool forceRefresh = false);
        Task<CreditNotesDetailsDto> GetByDocEntryCreditNotesAsync(string cardCode, int DocEntry, bool forceRefresh = false);
        Action CreditNotesAction { get; set; }
    }
}
