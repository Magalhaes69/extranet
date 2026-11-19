namespace WebFrontEnd.Services.IncomingPayments.Dto
{
    public record ViewDetailsIncomingPaymentsDto : IncomingPaymentsBAse
    {
        public List<GetAllPaymentInvoicesDto>? PaymentInvoices { get; set; }
        public List<PaymentChecksViewDto>? PaymentChecks { get; set; }
    }
}
