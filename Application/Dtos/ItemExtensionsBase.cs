
namespace Application.Dtos
{
    public abstract record ItemExtensionsBase
    {
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserId { get; set; }
    }
}
