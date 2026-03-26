namespace WebFrontEnd.Services.Items.DTO
{
    public record ItemsDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public bool Selected {  get; set; }
        public decimal Quatity { get;set; }
        public List<ItemPricesDto>? ItemPrices { get; set; }
    }
}
