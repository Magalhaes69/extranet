using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.VatGroups.GetAllVatGroups
{
    public class GetAllVatGroupsRequest : IRequest<GetAllIVatGroupsResult>, IFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Inactive { get; set; }
        public List<DynamicFilter>? DynamicFilters{ get; set; }
        
    }
}
