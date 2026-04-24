
namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public sealed record GetAllIncomingPaymentsPayChecksResponse
    {
        public List<GetAllPaymentChecksResponse>? PaymentChecks { get; set; }
    }
}
