namespace WebFrontEnd.Services.IncomingPayments.Dto
{
    public record GetIncomingPaymentsResult
    {
        public List<GetAllViewIncomingPaymentsDto>? Items { get; set; }
        public int TotalCount { get; set; }
        public IncomingPaymentsTotalsDto? Total { get; set; }
    }
}
