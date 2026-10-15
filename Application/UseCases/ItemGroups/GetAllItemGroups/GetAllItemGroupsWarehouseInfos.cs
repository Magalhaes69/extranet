

namespace Application.UseCases.ItemGroups.GetAllItemGroups
{
    public sealed record GetAllItemGroupsWarehouseInfos
    {
        public int ItmsGrpCod { get; set; }
        public string? WarehouseCode { get; set; }
        public string? DefaultBin { get; set; }
        public string? DefaultBinEnforced { get; set; }
    }
}
