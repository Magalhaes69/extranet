namespace Application.UseCases.Quotations.DetailsQuotations
{
    public record GetAllDocumentLinesResponse
    {
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceAfterVAT { get; set; }
        public string? Currency { get; set; }
        public decimal? Rate { get; set; }
        public decimal? DiscountPercent { get; set; }
        public string? SerialNum { get; set; }
        public string? WarehouseCode { get; set; }
        public string? VatGroup { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? TaxPercentagePerRow { get; set; }
        public decimal? TaxTotal { get; set; }
        public decimal? TotalInclTax { get; set; }
        public decimal? RowTotalFC { get; set; }
        public decimal? RowTotalSC { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? LineStatus { get; set; }
    }
}
