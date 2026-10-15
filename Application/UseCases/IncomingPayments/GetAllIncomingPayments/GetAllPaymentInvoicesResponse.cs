
namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public record GetAllPaymentInvoicesResponse
    {
        public int LineNum { get; set; }
        public int DocEntry { get; set; }
        public int? DocNum { get; set; }
        public decimal? SumApplied { get; set; }
        public decimal? AppliedFC { get; set; }
        public decimal? DocRate { get; set; }
        public int? DocLine { get; set; }
        public string? InvoiceType { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? PaidSum { get; set; }
        public int? InstallmentId { get; set; }
        public decimal? WitholdingTaxApplied { get; set; }
        public decimal? WitholdingTaxAppliedFC { get; set; }
        public decimal? WitholdingTaxAppliedSC { get; set; }
        public DateTime? LinkDate { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalDiscountFC { get; set; }
        public decimal? TotalDiscountSC { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public decimal? DocTotal { get; set; }
        public decimal? PaidToDate { get; set; }
        public string? Comments { get; set; }
        public string? DocCurrency { get; set; }
        public string? Address { get; set; }
        public string? PayToCode { get; set; }
    }
}
