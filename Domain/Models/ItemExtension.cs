using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public sealed class ItemExtension : BaseEntity
    {
        [Key]
        public Guid ItemId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemCode { get; set; }
        [MaxLength(200)]
        public string? ItemName { get; set; }
        [MaxLength(200)]
        public string? ImageUrl { get; set; }
    }
}
