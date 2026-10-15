namespace WebFrontEnd.Services.ItemGroups.Dto
{
    public sealed record ItemGroupsDto
    {
        public int Number { get; set; }
        public string GroupName { get; set; }
        public string? ItemGroupsWarehouseInfosDto { get; set; }
    }
}
