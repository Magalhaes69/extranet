
using System.ComponentModel.DataAnnotations;
using Domain.Helps.Enum;

namespace Domain.Models
{
    public class AppSetting : BaseEntity
    {
        public Guid Id { get; set; }
        [MaxLength(25)]
        public string Key { get; set; } = default!;
        [MaxLength(100)]
        public string? Value { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        [MaxLength(100)]
        public string? Description { get;set;  }
        public ConfigDataType DataType { get; set; }
        public bool IsEncrypted { get; set; }
        public bool IsVisible { get; set; }

    }
}
