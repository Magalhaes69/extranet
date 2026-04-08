namespace Application.Dtos
{
    public abstract record DocumentLineDto
    {
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Price { get; set; }
        public decimal? UnitPrice { get; set; }

        public decimal? DiscountPercent { get; set; }
        public decimal? LineTotal { get; set; }

        public string? Currency { get; set; }

        public string? WarehouseCode { get; set; }

        public string? VatGroup { get; set; }
        public decimal? PriceAfterVAT { get; set; }
        public decimal? Rate { get; set; }
    }
}
