using System.ComponentModel.DataAnnotations;

namespace WebFrontEnd.Services.Auth
{
    public class LoginDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
