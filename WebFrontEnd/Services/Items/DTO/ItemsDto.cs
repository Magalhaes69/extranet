using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.Items.DTO
{
    public record ItemsDto
    {
        [Filterable(LabelKey = "itemCode")]
        public string ItemCode { get; set; }
        [Filterable(LabelKey = "itemDescription")]
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public string? ItemsGroupName { get; set; }
        public bool Selected {  get; set; }
        public decimal Quatity { get;set; }
        public string? SalesVATGroup { get;set; }
        public decimal? Rate { get; set; }
        public string? DefaultWarehouse { get; set; }
        public List<ItemPricesDto>? ItemPrices { get; set; }
    }
}
