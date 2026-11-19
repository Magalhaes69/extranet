using WebFrontEnd.Services.ItemGroups.Dto;

namespace WebFrontEnd.Services.ItemGroups
{
    public interface IItemGroupsService
    {
        Task<List<ItemGroupsDto>> GetItemsGroupsAsync();

        Action ItemGroupsDto { get; set; }
    }
}
