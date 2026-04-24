namespace WebFrontEnd.Services.IncomingPayments.Dto
{
    public record PaymentChecksViewDto
    {
        public int LineNum { get; set; }
        public DateTime DueDate { get; set; }
        public int CheckNumber { get; set; }
        public string? BankCode { get; set; }
        public string? Branch { get; set; }
        public string? Details { get; set; }
        public string? Trnsfrable { get; set; }
        public decimal CheckSum { get; set; } = 0;
        public string? Currency { get; set; }
        public string? CountryCode { get; set; }
        public int? CheckAbsEntry { get; set; }
    }
}
