
namespace Application.UseCases.OrdersCarts.UpdateOrderCartLine
{
    public sealed record UpdateOrderCartLineResponse
    {
        public Guid OrderLineCartId { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? WarehouseCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal LineTotal { get; set; }
    }
}
