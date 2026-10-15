
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class AppRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
