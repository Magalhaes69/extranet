using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Returns.GetAll
{
    public sealed class GetAllReturnsRequest : IRequest<GetAllReturnsResult>, IFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? CardCode { get; set; }
        public List<string>? Cancelled { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
