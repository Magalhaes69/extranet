
using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Invoices.GetAllInvoices
{
    public class GetAllInvoicesRequest : IRequest<List<GetAllInvoicesResponse>>, IFilterRequest
    {
        public string? CardCode { get; set; }
        public List<string>? DocumentStatus { get; set; }
        public List<string>? Cancelled { get; set; }

        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
