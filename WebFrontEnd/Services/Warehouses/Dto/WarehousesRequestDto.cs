using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Services.Warehouses.Dto
{
    public class WarehousesRequestDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Inactive { get; set; }
        public List<DynamicFilter>? DynamicFilters { get; set; }
    }
}
