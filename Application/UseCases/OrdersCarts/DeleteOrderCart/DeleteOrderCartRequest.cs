
using MediatR;

namespace Application.UseCases.OrdersCarts.DeleteOrderCart
{
    public sealed record DeleteOrderCartRequest : IRequest<DeleteOrderCartResponse>
    {
        public Guid OrderId { get; set; }
    }
}
