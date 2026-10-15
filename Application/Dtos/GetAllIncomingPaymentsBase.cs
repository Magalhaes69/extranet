
namespace Application.Dtos
{
    public abstract record GetAllIncomingPaymentsBase
    {
        public int DocNum { get; set; }
        public string? DocType { get; set; }
        public string? DocDate { get; set; }
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

        public string? PayToCode { get; set; }
        public int? ContactPersonCode { get; set; }
    }
}
