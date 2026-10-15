namespace Application.UseCases.ItemGroups.GetAllItemGroups
{
    public record GetAllItemGroupsResponse
    {
        public int Number { get; set; }
        public string GroupName { get; set; }
        public List<GetAllItemGroupsWarehouseInfos>? ItemGroupsWarehouseInfos { get; set; }
    }
}
