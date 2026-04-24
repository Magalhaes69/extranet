using MediatR;

namespace Application.UseCases.OrdersCarts.CreateOrderCartLine
{
    public sealed record CreateOrderCartLineRequest : IRequest<CreateOrderCartLineResponse>
    {
        public Guid OrderId { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? WarehouseCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal LineTotal { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string UserId { get; set; }
    }
}
