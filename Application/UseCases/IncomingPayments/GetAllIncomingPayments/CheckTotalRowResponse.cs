
namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public sealed record CheckTotalRowResponse
    {
        public decimal? TotalCheckSum { get; set; }
    }
}
