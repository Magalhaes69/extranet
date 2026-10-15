using Application.Dtos;
using Application.Shared.Helps;
using Domain.Interfaces;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.UseCases.Orders.GetAllOrders
{
    public class GetOrdersHandler : IRequestHandler<GetAllOrdersRequest, PagedResult<GetAllOrdersResponse, TotalResult>>
    {
        private readonly ISapClient _sapClient;
        public GetOrdersHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }

        public async Task<PagedResult<GetAllOrdersResponse, TotalResult>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
        {
            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "DocEntry,DocNum,DocType,Series,DocDate,DocDueDate,CardCode,CardName,Address,DocTotal,DocCurrency,Reference1,Reference2,Comments,TotalDiscount,DocumentStatus,Cancelled,DiscountPercent,VatSum,ShipToCode,PayToCode,Address2",
                ["$orderby"] = "DocEntry desc"
            };

            var (data, totalPage) = await _sapClient.GetPagedBL1Async<GetAllOrdersResponse>(
                "Orders",
                request.Page,
                request.PageSize,
                filter,
                queryParams,
                cancellationToken);

            var sumTask = _sapClient.SumAsync("Orders", new[] { "DocTotal" }, filter, cancellationToken);

            return new PagedResult<GetAllOrdersResponse, TotalResult>
            {
                Items = data ?? new(),
                TotalCount = totalPage,
                Page = request.Page,
                PageSize = request.PageSize,
                Total = new TotalResult
                {
                    Total = (await sumTask)[0],
                    Currency = await _sapClient.GetLocalCurrencyAsync(cancellationToken)
                }
            };
        }
    }
}
