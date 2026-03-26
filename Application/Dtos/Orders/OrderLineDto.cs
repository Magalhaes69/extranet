
namespace Application.Dtos.Orders
{
    public record OrderLineDto
    {
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string WarehouseCode { get; set; }
        public string? ItemDescription { get; set; }

        public decimal LineTotal { get; set; }
        //public int Line { get; set; }
        //public bool Selected { get; set; }
    }
}
