using Application.Dtos.CreditNotes;

namespace Application.UseCases.CreditNotes.DetailsCreditNotes
{
    public record DetailsCreditNotesResponse : CreditNotesBase
    {
        public List<DocumentsLineCreditNotesDto>? DocumentLines { get; set; }
    }
}
