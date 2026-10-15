using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class OrderLineCart : BaseEntity
    {
        [Key]
        public Guid OrderLineCartId { get; set; }
        public Guid OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public OrdersCart OrdersCart { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string WarehouseCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal LineTotal { get; set; }
        public decimal DiscPrcnt { get; set; }
        [MaxLength(100)]
        public string? VatGroup { get; set; }
        
    }
}
