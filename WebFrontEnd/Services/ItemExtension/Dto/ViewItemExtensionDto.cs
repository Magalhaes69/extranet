using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.ItemExtension.Dto
{
    public class ViewItemExtensionDto
    {
        public bool Selected { get; set; }
        public Guid ItemId { get; set; }
        [Filterable(LabelKey = "ItemCode")]
        public string ItemCode { get; set; }
        [Filterable(LabelKey = "ItemName")]
        public string? ItemName { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserId { get; set; }

    }
}
