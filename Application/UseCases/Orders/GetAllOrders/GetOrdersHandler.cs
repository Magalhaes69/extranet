using Application.Dtos.Orders;
using Application.UseCases.Orders.CreateOrder;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Orders.GetAllOrders
{
    public class GetOrdersHandler : IRequestHandler<GetAllOrdersRequest, List<GetAllOrdersResponse>>
    {
        private readonly ISapClient _sapClient;
        public GetOrdersHandler(ISapClient sapClient)
        {
            _sapClient = sapClient;
        }

        public async Task<List<GetAllOrdersResponse>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.CardCode))
                filter = $"CardCode eq '{request.CardCode}' &$orderby=DocEntry desc";

            var sapOrders = await _sapClient.GetAllAsync<GetAllOrdersResponse>("Orders", filter, cancellationToken);

            return sapOrders.ToList()!;
        }
    }
}
