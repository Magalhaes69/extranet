namespace WebFrontEnd.Services.Warehouses.Dto
{
    public class WarehousesResultDto
    {
        public List<WarehousesDto>? Items { get; set; }
        public int TotalCount { get; set; } = 0;
    }
}
