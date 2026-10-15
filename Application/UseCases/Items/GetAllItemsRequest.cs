using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Items
{
    public class GetAllItemsRequest : IRequest<GetAllItemsResult>, IFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Valid { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }

}
