namespace WebFrontEnd.Services.IncomingPayments.Dto
{
    public record IncomingPaymentsTotalsDto
    {
        public decimal TotalCashSum { get; set; } = 0.0M;
        public decimal TotalTransferSum { get; set; } = 0.0M;
        public decimal TotalPaymentChecks { get; set; } = 0.0M;
    }
}
