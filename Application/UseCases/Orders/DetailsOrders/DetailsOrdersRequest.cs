
using MediatR;

namespace Application.UseCases.Orders.DetailsOrders
{
    public class DetailsOrdersRequest : IRequest<DetailsOrdersResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
