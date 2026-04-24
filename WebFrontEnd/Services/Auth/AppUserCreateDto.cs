using System.ComponentModel.DataAnnotations;
using WebFrontEnd.Services.Enums;

namespace WebFrontEnd.Services.Auth
{
    public class AppUserCreateDto
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(120, ErrorMessage = "Número máximo de caracteres(120)")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(120, ErrorMessage = "Número máximo de caracteres(60)")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(120, ErrorMessage = "Número máximo de caracteres(9)")]
        public string Nuit { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string CardCode { get; set; }
        public string Theme { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Language { get; set; }
        public string? ImageUrl { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
