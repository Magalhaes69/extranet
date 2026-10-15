using Application.Dtos;
using Application.Shared.Helps;
using Domain.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Orders.GetAllOrders
{
    public class GetAllOrdersRequest : IRequest<PagedResult<GetAllOrdersResponse, TotalResult>>, IFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string? CardCode { get; set; }
        public List<string>? DocumentStatus { get; set; }
        public List<string>? Cancelled { get; set; }

        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
