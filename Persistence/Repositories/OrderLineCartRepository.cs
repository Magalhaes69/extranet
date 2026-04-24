
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;


namespace Persistence.Repositories
{
    public class OrderLineCartRepository : BaseRepository<OrderLineCart>, IOrderLineCartRepository
    {
        public OrderLineCartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<OrderLineCart>> GetAll(Guid OrderId)
        {
            return await _context.OrderLineCart.Where(x=>x.OrderId == OrderId).ToListAsync();
        }
    }
}
