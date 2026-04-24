
using MediatR;

namespace Application.UseCases.OrdersCarts.DeleteOrderCartLine
{
    public sealed record DeleteOrderCartLineRequest : IRequest<List<DeleteOrderCartLineResponse>>
    {
        public Guid OrderLineCartId { get; set; }
        public Guid OrderId { get; set; }
    }
}
