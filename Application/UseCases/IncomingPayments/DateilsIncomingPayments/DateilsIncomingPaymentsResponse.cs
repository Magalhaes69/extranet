
using Application.Dtos;
using Application.UseCases.IncomingPayments.GetAllIncomingPayments;

namespace Application.UseCases.IncomingPayments.DateilsIncomingPayments
{
    public sealed record DateilsIncomingPaymentsResponse : GetAllIncomingPaymentsBase
    {
        public List<GetAllPaymentInvoicesResponse>? PaymentInvoices { get; set; }
        public List<GetAllPaymentChecksResponse>? PaymentChecks { get; set; }
    }
}
