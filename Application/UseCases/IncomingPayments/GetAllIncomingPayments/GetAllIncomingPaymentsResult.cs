
namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public record GetAllIncomingPaymentsResult
    {
        public List<GetAllIncomingPaymentsResponse>? Items { get; set; }
        public int TotalCount { get; set; }
        public GetAllIncomingPaymentsTotalResult? Total { get; set; }
    }
}
