using MediatR;

namespace Application.UseCases.OrdersCarts.UpdateOrdersCarts
{
    public sealed record UpdateOrdersCartsRequest : IRequest<UpdateOrdersCardResponse>
    {
        public Guid OrderId { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
    }
}
