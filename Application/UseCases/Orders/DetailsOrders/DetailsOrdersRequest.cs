using Application.UseCases.Orders.GetAllOrders;
using MediatR;

namespace Application.UseCases.Orders.DetailsOrders
{
    public class DetailsOrdersRequest : IRequest<GetAllOrdersResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
