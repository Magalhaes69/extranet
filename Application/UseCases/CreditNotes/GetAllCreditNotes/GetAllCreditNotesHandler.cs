using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.CreditNotes.GetAllCreditNotes
{
    public class GetAllCreditNotesHandler : IRequestHandler<GetAllCreditNotesRequest, List<GetAllCreditNotesResponse>>
    {
        private readonly ISapClient _client;
        public GetAllCreditNotesHandler(ISapClient client)
        {
            this._client = client;
        }
        public async Task<List<GetAllCreditNotesResponse>> Handle(GetAllCreditNotesRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

            var sapOrders = await _client.GetAllAsync<GetAllCreditNotesResponse>("CreditNotes", filter, cancellationToken);

            return sapOrders;
        }
    }
}
