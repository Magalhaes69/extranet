using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Warehouses.GetAllWarehouses
{
    public record GetAllWarehousesResponse
    {
        public string? WarehouseName { get; set; }
        public string? WarehouseCode { get; set; }
        public string? Location { get; set; }
        public string? Inactive { get; set; }
    }
}
