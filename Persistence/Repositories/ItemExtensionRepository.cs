
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class ItemExtensionRepository : BaseRepository<ItemExtension>, IItemExtensionRepository
    {
        public ItemExtensionRepository(AppDbContext context) : base(context)
        {}

        public async Task DeleteManyAsync(List<string> ItemId)
        {
            var items = await _context.ItemExtension.Where(x => ItemId.Contains(x.ItemCode)).ToListAsync();
            _context.ItemExtension.RemoveRange(items);
        }

        public async Task<List<ItemExtension>> GetAllByListItemCodeAsync(List<string> ItemCode)
        {
            return await _context.ItemExtension
                .Where(x => ItemCode.Contains(x.ItemCode))
                .ToListAsync();
        }

        public async Task<ItemExtension> GetByItemCodeAsync(string ItemCode)
        {
            return await _context.ItemExtension.Where(x => x.ItemCode == ItemCode).FirstOrDefaultAsync();
        }

        public async Task<ItemExtension> GetByItemIdAsync(string ItemId)
        {
            return await _context.ItemExtension.Where(x => x.ItemId == Guid.Parse(ItemId)).FirstOrDefaultAsync();
        }
    }
}
