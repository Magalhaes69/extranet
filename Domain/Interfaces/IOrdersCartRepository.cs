
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IOrdersCartRepository : IBaseRepository<OrdersCart>
    {
        Task<OrdersCart> GetLastOrderPendAsync(string CardCode, string UserId, CancellationToken cancellationToken);
    }
}
