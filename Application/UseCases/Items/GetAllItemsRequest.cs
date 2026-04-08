using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Items
{
    public class GetAllItemsRequest : IRequest<List<GetAllItemsResponse>>, IFilterRequest
    {
        public string Valid { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }

}
