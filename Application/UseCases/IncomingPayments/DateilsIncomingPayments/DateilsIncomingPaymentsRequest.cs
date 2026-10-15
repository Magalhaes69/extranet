
using MediatR;

namespace Application.UseCases.IncomingPayments.DateilsIncomingPayments
{
    public sealed record DateilsIncomingPaymentsRequest : IRequest<DateilsIncomingPaymentsResponse>
    {
        public string CardCode { get; set; }
        public int DocNum { get; set; }
        public string DocType { get; set; }
        public int Series { get; set; } = 0;
    }
}
