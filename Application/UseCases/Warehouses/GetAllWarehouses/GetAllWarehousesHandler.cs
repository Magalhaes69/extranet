using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Items;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Warehouses.GetAllWarehouses
{
    public class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesRequest, List<GetAllWarehousesResponse>>
    {
        private readonly ISapClient _sapClient;

        public GetAllWarehousesHandler(ISapClient sapClient)
        {
            this._sapClient = sapClient;
        }
        public async Task<List<GetAllWarehousesResponse>> Handle(GetAllWarehousesRequest request, CancellationToken cancellationToken)
        {
            string? filter = null;
            if (!string.IsNullOrEmpty(request.Inactive))
                filter = $"Inactive eq '{request.Inactive}'";

            var getItems = await _sapClient.GetAllAsync<GetAllWarehousesResponse>("Warehouses", filter, cancellationToken);

            return getItems.Select(x => new GetAllWarehousesResponse
            {
                WarehouseCode = x.WarehouseCode,
                WarehouseName = x.WarehouseName,
                Location = x.Location
            }).ToList();
        }
    }
}
