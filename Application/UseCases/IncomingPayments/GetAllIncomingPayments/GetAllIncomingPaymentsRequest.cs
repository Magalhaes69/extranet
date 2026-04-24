using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.IncomingPayments.GetAllIncomingPayments
{
    public class GetAllIncomingPaymentsRequest : IRequest<GetAllIncomingPaymentsResult>, IFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? CardCode { get; set; }
        public List<string>? Cancelled { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
