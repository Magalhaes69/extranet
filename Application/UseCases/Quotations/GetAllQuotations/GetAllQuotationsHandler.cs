using Application.UseCases.Invoices.Helps;
using Application.UseCases.Orders.GetAllOrders;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Quotations.GetAllQuotations
{
    public class GetAllQuotationsHandler : IRequestHandler<GetAllQuotationsRequest, List<GetAllQuotationsResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetAllQuotationsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<List<GetAllQuotationsResponse>> Handle(GetAllQuotationsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = ODataFilterBuilder.Build(request);
                var queryParams = new Dictionary<string, string>
                {
                    ["$select"] = "DocEntry,DocNum,DocType,Series,DocDate,DocDueDate,CardCode,CardName,Address,DocTotal,DocCurrency,Reference1,Reference2,Comments,TotalDiscount,DocumentStatus,Cancelled,DiscountPercent,VatSum,ShipToCode,PayToCode,Address2",
                    ["$orderby"] = "DocEntry desc"
                };

                var sapOrders = await _sapClient.GetAllB1Async<GetAllQuotationsResponse>(
                    "Quotations",
                    filter,
                    queryParams,
                    cancellationToken);

                return sapOrders;

                
            }
            catch(Exception ex)
            {
                return null!;
            }
        }
    }
}
