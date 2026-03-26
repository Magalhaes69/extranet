using MediatR;

namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public class GetAllIncomingPaymentsRequest : IRequest<List<GetAllIncomingPaymentsResponse>>
    {
        public string CardCode { get; set; }
        public DateTime? DocDate { get; set; }
        public string? Series { get; set; }
    }
}
