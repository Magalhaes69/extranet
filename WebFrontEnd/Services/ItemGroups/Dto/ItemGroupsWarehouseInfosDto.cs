namespace WebFrontEnd.Services.ItemGroups.Dto
{
    public sealed record ItemGroupsWarehouseInfosDto
    {
        public int ItmsGrpCod { get; set; }
        public string? WarehouseCode { get; set; }
        public string? DefaultBin { get; set; }
        public string? DefaultBinEnforced { get; set; }
    }
}
