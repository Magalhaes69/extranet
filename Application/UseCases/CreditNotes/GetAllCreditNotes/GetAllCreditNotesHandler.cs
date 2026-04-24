using Application.UseCases.Invoices.Helps;
using Application.UseCases.Quotations.GetAllQuotations;
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

            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "DocEntry,DocNum,DocType,Series,DocDate,DocDueDate,CardCode,CardName,Address,DocTotal,DocCurrency,Reference1,Reference2,Comments,TotalDiscount,DocumentStatus,Cancelled,DiscountPercent,VatSum,ShipToCode,PayToCode,Address2",
                ["$orderby"] = "DocEntry desc"
            };

            var sapOrders = await _client.GetAllB1Async<GetAllCreditNotesResponse>(
                "CreditNotes",
                filter,
                queryParams,
                cancellationToken);

            return sapOrders;
        }
    }
}
