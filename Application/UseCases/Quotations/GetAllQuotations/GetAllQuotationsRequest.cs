using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Quotations.GetAllQuotations
{
    public record GetAllQuotationsRequest : IRequest<List<GetAllQuotationsResponse>>, IFilterRequest
    {
        public string? CardCode { get; set; }
        public List<string>? DocumentStatus { get; set; }
        public List<string>? Cancelled { get; set; }

        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
