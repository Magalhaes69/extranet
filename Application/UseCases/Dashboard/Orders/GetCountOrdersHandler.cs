

using Application.Dtos.Orders;
using Application.UseCases.Orders.GetAllOrders;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Dashboard.Orders
{
    public class GetCountOrdersHandler : IRequestHandler<GetCountOrdersRequest, GetCountResponse>
    {
        private readonly IB1DashboardService _sapClient;
        public GetCountOrdersHandler(IB1DashboardService sapClient)
        {
            _sapClient = sapClient;
        }
        public async Task<GetCountResponse> Handle(GetCountOrdersRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            //filter = "";
            if (!string.IsNullOrEmpty(request.Status))
                filter += $"";

            if (!string.IsNullOrEmpty(request.CardCode))
                filter += $"CardCode eq '{request.CardCode}'";

            var sapOrders = await _sapClient.GetListAsync<DashboardOrderDto>("Orders?$select=DocEntry", filter, cancellationToken);


            return new GetCountResponse
            {
                Total = sapOrders.Count,
            };
        }
    }
}
