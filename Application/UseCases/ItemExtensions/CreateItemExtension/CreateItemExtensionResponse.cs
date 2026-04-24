
namespace Application.UseCases.ItemExtensions.CreateItemExtension
{
    public sealed record CreateItemExtensionResponse
    {
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserId { get; set; }
    }
}
