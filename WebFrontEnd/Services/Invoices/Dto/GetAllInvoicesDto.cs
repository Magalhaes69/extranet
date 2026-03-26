namespace WebFrontEnd.Services.Invoices.Dto
{
    public record GetAllInvoicesDto
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string? DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? Address { get; set; }
        public decimal? DocTotal { get; set; }
        public string? DocCurrency { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public string? Comments { get; set; }
        public decimal? TotalDiscount { get; set; }
        public List<GetAllDocumentLinesDto>? DocumentLines { get; set; }
    }
}
