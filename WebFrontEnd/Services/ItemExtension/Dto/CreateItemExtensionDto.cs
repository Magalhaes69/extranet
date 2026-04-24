using System.ComponentModel.DataAnnotations;

namespace WebFrontEnd.Services.ItemExtension.Dto
{
    public class CreateItemExtensionDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(50, ErrorMessage = "Número máximo de caracteres 50")]
        public string ItemCode { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(200, ErrorMessage = "Número máximo de caracteres 200")]
        public string ItemName { get; set; }
        [MaxLength(50, ErrorMessage = "Número máximo de caracteres 200")]
        public string? ImageUrl { get; set; }
        public string? UserId { get; set; }
    }
}
