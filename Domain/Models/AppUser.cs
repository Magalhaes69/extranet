
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public sealed class AppUser : IdentityUser
    {
        [Required]
        [Column(TypeName = "nvarchar(120)")]
        public string? FirstName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(60)")]
        public string? LastName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Nuit { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string CardCode { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? Theme { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? Language { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
        
    }
}
