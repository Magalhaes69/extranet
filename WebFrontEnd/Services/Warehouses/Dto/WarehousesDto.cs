namespace WebFrontEnd.Services.Warehouses.Dto
{
    public record WarehousesDto
    {
        public string? WarehouseName { get; set; }
        public string? WarehouseCode { get; set; }
        public string? Location { get; set; }
        public string? Inactive { get; set; }
    }
}
