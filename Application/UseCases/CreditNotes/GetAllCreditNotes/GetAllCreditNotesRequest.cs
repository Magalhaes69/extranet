using MediatR;

namespace Application.UseCases.CreditNotes.GetAllCreditNotes
{
    public class GetAllCreditNotesRequest : IRequest<List<GetAllCreditNotesResponse>>
    {
        public string CardCode { get; set; }
    }
}
