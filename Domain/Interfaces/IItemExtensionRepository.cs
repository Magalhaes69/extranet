
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IItemExtensionRepository : IBaseRepository<ItemExtension>
    {
        Task<ItemExtension> GetByItemCodeAsync(string ItemCode);
        Task<ItemExtension> GetByItemIdAsync(string ItemId);
        Task DeleteManyAsync(List<string> ItemCode);
        Task<List<ItemExtension>> GetAllByListItemCodeAsync(List<string> ItemCode);
    }
}
