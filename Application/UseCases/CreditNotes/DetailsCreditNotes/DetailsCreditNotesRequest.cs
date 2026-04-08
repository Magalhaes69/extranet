using MediatR;

namespace Application.UseCases.CreditNotes.DetailsCreditNotes
{
    public class DetailsCreditNotesRequest : IRequest<DetailsCreditNotesResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
