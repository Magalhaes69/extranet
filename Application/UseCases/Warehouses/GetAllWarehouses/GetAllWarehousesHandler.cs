
using Application.Shared.Helps;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Warehouses.GetAllWarehouses
{
    public class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesRequest, GetAllWarehousesResult>
    {
        private readonly ISapClient _sapClient;

        public GetAllWarehousesHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<GetAllWarehousesResult> Handle(GetAllWarehousesRequest request, CancellationToken cancellationToken)
        {
            var filter = ODataFilterBuilder.Build(request);
            var queryParams = new Dictionary<string, string>
            {
                ["$select"] = "WarehouseName,WarehouseCode,Location,Inactive"
            };

            var (getItems, totalCount) = await _sapClient.GetPagedBL1Async<GetAllWarehousesResponse>(
                "Warehouses",
                request.Page,
                request.PageSize,
                filter, 
                queryParams,
                cancellationToken);


            return new GetAllWarehousesResult
            {
                Items = getItems,
                TotalCount = totalCount
            };
        }
    }
}
