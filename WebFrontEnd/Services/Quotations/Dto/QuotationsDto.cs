using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.Quotations.Dto
{
    public record QuotationsDto
    {
        public bool Selected { get; set; }
        public int DocEntry { get; set; }
        [Filterable(LabelKey = "docmentNo")]
        public int DocNum { get; set; }
        public int? Series { get; set; }
        public string? DocType { get; set; }
        [Filterable(LabelKey = "launchDate")]
        public DateTime DocDate { get; set; }
        [Filterable(LabelKey = "validUntil")]
        public DateTime DocDueDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? Address { get; set; }
        public decimal DocTotal { get; set; }
        public string? DocCurrency { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public string? Comments { get; set; }
        public decimal? TotalDiscount { get; set; }
        public string? DocumentStatus { get; set; }
        public string? Cancelled { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? VatSum { get; set; }
        public string? ShipToCode { get; set; }
        public string? PayToCode { get; set; }
        public string? Address2 { get; set; }
        public string? NumAtCard { get; set; }

        public List<DocumentLinesDto>? DocumentLines { get; set; }
    }
}
