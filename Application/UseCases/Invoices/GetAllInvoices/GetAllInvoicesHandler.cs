using Application.UseCases.Invoices.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Invoices.GetAllInvoices
{
    public class GetAllInvoicesHandler : IRequestHandler<GetAllInvoicesRequest, List<GetAllInvoicesResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetAllInvoicesHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }

        public async Task<List<GetAllInvoicesResponse>> Handle(GetAllInvoicesRequest request, CancellationToken cancellationToken)
        {
            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "DocEntry,DocNum,DocType,Series,DocDate,DocDueDate,CardCode,CardName,Address,DocTotal,DocCurrency,Reference1,Reference2,Comments,TotalDiscount,DocumentStatus,Cancelled,DiscountPercent,VatSum,ShipToCode,PayToCode,Address2",
                ["$orderby"] = "DocEntry desc"
            };

            var sapOrders = await _sapClient.GetAllB1Async<GetAllInvoicesResponse>(
                "Invoices", 
                filter,
                queryParams,
                cancellationToken);

            return sapOrders;
        }
    }
}
