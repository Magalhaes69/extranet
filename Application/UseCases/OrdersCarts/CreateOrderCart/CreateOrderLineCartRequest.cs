
using Domain.Models;

namespace Application.UseCases.OrdersCarts.CreateOrderCart
{
    public sealed record CreateOrderLineCartRequest
    {
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string WarehouseCode { get; set; }
        public string? ItemDescription { get; set; }
        public string UserId { get; set; }
        public decimal LineTotal { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string? VatGroup { get; set; }
    }
}
