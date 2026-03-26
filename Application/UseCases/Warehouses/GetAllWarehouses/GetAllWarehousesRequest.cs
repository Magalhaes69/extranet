using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.UseCases.Warehouses.GetAllWarehouses
{
    public class GetAllWarehousesRequest : IRequest<List<GetAllWarehousesResponse>>
    {
        public string Inactive { get; set; }
    }
}
