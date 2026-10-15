
using Application.Dtos;

namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public sealed record GetAllIncomingPaymentsResponse : GetAllIncomingPaymentsBase
    {
        public List<GetAllPaymentInvoicesResponse>? PaymentInvoices { get; set; }
        public List<GetAllPaymentChecksResponse>? PaymentChecks { get; set; }
    }
}
