
namespace Application.Dtos.OrdersCards
{
    public sealed record CatalogItemDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public string? ImageUrl { get; set; }
        public decimal ItemPrices { get; set; }
        public string? DefaultWarehouse { get; set; }
        public string? Currency { get; set; } = string.Empty;
        public string? SalesVATGroup { get; set; }
        public decimal? Rate { get; set; }
    }
}
