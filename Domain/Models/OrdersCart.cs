

using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public sealed class OrdersCart : BaseEntity
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();
        [MaxLength(15)]
        public string CardCode { get; set; }
        [MaxLength(100)]
        public string CardName { get; set; }
        [MaxLength(100)]
        public string? Referencia { get; set; }
        public bool Status { get; set; } = false;
        public List<OrderLineCart> OrderLineCarts { get; set; }
    }
}
