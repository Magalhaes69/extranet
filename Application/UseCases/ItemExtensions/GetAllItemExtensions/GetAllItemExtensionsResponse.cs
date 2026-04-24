
namespace Application.UseCases.ItemExtensions.GetAllItemExtensions
{
    public sealed record GetAllItemExtensionsResponse
    {
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ImageUrl { get; set; }

    }
}
