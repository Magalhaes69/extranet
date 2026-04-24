namespace WebFrontEnd.Services.OrdersCarts.Dto
{
    public record UpdateOrdersCartLineDto
    {
        public Guid OrderLineCartId { get; set; }
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
