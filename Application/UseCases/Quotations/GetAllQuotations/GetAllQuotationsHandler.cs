using Application.Dtos;
using Application.Shared.Helps;
using Application.UseCases.IncomingPayments.GetAllIncomingPayments;
using Domain.Interfaces;
using MediatR;


namespace Application.UseCases.Quotations.GetAllQuotations
{
    public class GetAllQuotationsHandler : IRequestHandler<GetAllQuotationsRequest, PagedResult<GetAllQuotationsResponse, TotalResult>>
    {
        private readonly ISapClient _sapClient;
        public GetAllQuotationsHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<PagedResult<GetAllQuotationsResponse, TotalResult>> Handle(GetAllQuotationsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = ODataFilterBuilder.Build(request);
                var queryParams = new Dictionary<string, string>
                {
                    ["$select"] = "DocEntry,DocNum,DocType,Series,DocDate,DocDueDate,CardCode,CardName,Address,DocTotal,DocCurrency,Reference1,Reference2,Comments,TotalDiscount,DocumentStatus,Cancelled,DiscountPercent,VatSum,ShipToCode,PayToCode,Address2",
                    ["$orderby"] = "DocEntry desc"
                };

                var (data, total) = await _sapClient.GetPagedBL1Async<GetAllQuotationsResponse>(
                    "Quotations",
                    request.Page,
                    request.PageSize,
                    filter,
                    queryParams,
                    cancellationToken);

                var sumTask = _sapClient.SumAsync("Quotations", new[] { "DocTotal" }, filter, cancellationToken);

                return new PagedResult<GetAllQuotationsResponse, TotalResult>
                {
                    Items = data ?? new(),
                    TotalCount = total,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = new TotalResult
                    {
                        Total =(await sumTask)[0],
                        Currency = await _sapClient.GetLocalCurrencyAsync(cancellationToken)
                    }
                };


            }
            catch(Exception ex)
            {
                return null!;
            }
        }
    }
}
