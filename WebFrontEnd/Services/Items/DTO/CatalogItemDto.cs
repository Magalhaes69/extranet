namespace WebFrontEnd.Services.Items.DTO
{
    public sealed record CatalogItemDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public string? ImageUrl { get; set; }
        public decimal ItemPrices { get; set; }
        public  string? Currency { get; set; } = string.Empty;
    }
}
