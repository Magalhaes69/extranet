

namespace Application.UseCases.Warehouses.GetAllWarehouses
{
    public record GetAllWarehousesResult
    {
        public List<GetAllWarehousesResponse> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
