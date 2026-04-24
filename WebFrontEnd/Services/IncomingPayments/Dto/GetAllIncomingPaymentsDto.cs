using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.IncomingPayments.Dto
{
    public record GetAllIncomingPaymentsDto
    {
        public bool Selected { get; set; }
        [Filterable(LabelKey = "docmentNo")]
        public int DocNum { get; set; }
        public string? DocType { get; set; }
        public string? HandWritten { get; set; }
        public string? Printed { get; set; }
        [Filterable(LabelKey = "launchDate")]
        public DateTime? DocDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? Address { get; set; }
        public string? DocCurrency { get; set; }
        public decimal? CashSum { get; set; }
        public string? CheckAccount { get; set; }
        public string? TransferAccount { get; set; }
        public decimal? TransferSum { get; set; }
        public DateTime? TransferDate { get; set; }
        public string? LocalCurrency { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public string? Remarks { get; set; }
        public DateTime? TaxDate { get; set; }
        public int? Series { get; set; }
        public string? ProjectCode { get; set; }
        public decimal? DeductionPercent { get; set; }
        public decimal? DeductionSum { get; set; }
        public decimal? CashSumFC { get; set; }
        public decimal? CashSumSys { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Cancelled { get; set; }

        public List<GetAllPaymentInvoicesDto>? PaymentInvoices { get; set; }
    }
}
