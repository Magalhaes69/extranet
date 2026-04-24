
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public abstract class BaseEntity
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }

    }
}
