
namespace Application.UseCases.Returns.GetAll
{
    public sealed record GetAllReturnsResponse
    {
        
            // Identification
            public int DocEntry { get; set; }
            public int DocNum { get; set; }
            public int? Series { get; set; }
            public string? DocType { get; set; }

            // Dates
            public DateTime? DocDate { get; set; }
            public DateTime? DocDueDate { get; set; }
            public DateTime? TaxDate { get; set; }

            // Business partner
            public string? CardCode { get; set; }
            public string? CardName { get; set; }
            public string? NumAtCard { get; set; }
            public string? Address { get; set; }
            public string? Address2 { get; set; }
            public string? ShipToCode { get; set; }
            public string? PayToCode { get; set; }
            public int? ContactPersonCode { get; set; }
            public int? SalesPersonCode { get; set; }

            // Amounts
            public decimal? DocTotal { get; set; }
            public decimal? DocTotalFc { get; set; }
            public decimal? VatSum { get; set; }
            public decimal? TotalDiscount { get; set; }
            public decimal? DiscountPercent { get; set; }
            public decimal? PaidToDate { get; set; }
            public string? DocCurrency { get; set; }
            public decimal? DocRate { get; set; }
            public int? PriceListNum { get; set; }

            // References / notes
            public string? Reference1 { get; set; }
            public string? Reference2 { get; set; }
            public string? Comments { get; set; }
            public string? JournalMemo { get; set; }
            public string? Project { get; set; }

            // Status
            public string? DocumentStatus { get; set; }
            public string? Cancelled { get; set; }
            public string? CancelStatus { get; set; }
        
    }
}
