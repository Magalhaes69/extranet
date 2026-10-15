using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Warehouses.GetAllWarehouses
{
    public class GetAllWarehousesRequest : IRequest<GetAllWarehousesResult>, IFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Inactive { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
