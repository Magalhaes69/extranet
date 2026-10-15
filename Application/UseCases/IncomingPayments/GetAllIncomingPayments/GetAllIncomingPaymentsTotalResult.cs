
namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public sealed class GetAllIncomingPaymentsTotalResult
    {
        public decimal TotalCashSum { get; set; } = 0.0M;
        public decimal TotalTransferSum { get; set; } = 0.0M;
        public decimal TotalPaymentChecks { get; set; } = 0.0M;
        public string Currency { get; set; } = string.Empty;
    }
}
