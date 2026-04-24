

using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.ItemGroups.GetAllItemGroups
{
    public class GetAllItemGroupsRequest : IRequest<List<GetAllItemGroupsResponse>>, IFilterRequest
    {
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
