using Application.Dtos.Orders;
using MediatR;

namespace Application.UseCases.Orders.GetAllOrders
{
    public class GetAllOrdersRequest : IRequest<List<GetAllOrdersResponse>>
    {
        public string CardCode { get; set; }
    }
}
