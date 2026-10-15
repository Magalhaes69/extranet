using MediatR;

namespace Application.UseCases.OrdersCarts.CreateOrderCart
{
    public sealed record CreateOrdersCardRequest : IRequest<CreateOrdersCardResponse>
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string? Referencia { get; set; }
        public string UserId { get; set; }
        public List<CreateOrderLineCartRequest> OrderLineCarts { get; set; }

    }
}
