namespace WebFrontEnd.Services.Invoices.Dto
{
    public class GetAllDocumentLinesDto
    {
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceAfterVAT { get; set; }
        public string? Currency { get; set; }
        public decimal? DiscountPercent { get; set; }
        public string? SerialNum { get; set; }
        public string? WarehouseCode { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal { get; set; }
    }
}
