
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class OrdersCartRepository : BaseRepository<OrdersCart>, IOrdersCartRepository
    {
        
        public OrdersCartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<OrdersCart> GetByOrderIdAsync(Guid OrderId, CancellationToken cancellationToken)
        {
            return await _context.OrdersCart
                 .Include(x => x.OrderLineCarts)
                 .FirstOrDefaultAsync(
                 x => x.OrderId == OrderId, cancellationToken);
        }

        public async Task<OrdersCart> GetLastOrderPendAsync(string CardCode, string UserId, CancellationToken cancellationToken)
        {
            return await _context.OrdersCart
                .Include(x => x.OrderLineCarts)
                .FirstOrDefaultAsync(
                x => x.Status == false && 
                x.CardCode.ToLower().Equals(CardCode.ToLower()) &&
                x.UserId == UserId, cancellationToken);
        }
    }
}
