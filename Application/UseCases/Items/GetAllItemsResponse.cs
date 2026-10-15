

namespace Application.UseCases.Items
{
    public sealed record GetAllItemsResponse
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public string? ItemsGroupName { get; set; }
        public string? ImageUrl { get; set; }
        public string? SalesVATGroup { get; set; }
        public decimal? Rate { get; set; }
        public string? DefaultWarehouse { get; set; }
        public List<ItemPrices>? ItemPrices { get; set; }
    }
}
