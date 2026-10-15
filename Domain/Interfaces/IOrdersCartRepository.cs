
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IOrdersCartRepository : IBaseRepository<OrdersCart>
    {
        Task<OrdersCart> GetLastOrderPendAsync(string CardCode, string UserId, CancellationToken cancellationToken);
        /// <summary>
        /// Gets an order cart by order ID.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The corresponding <see cref="OrdersCart"/> if found; otherwise, null.</returns>
        Task<OrdersCart> GetByOrderIdAsync(Guid OrderId, CancellationToken cancellationToken);
    }
}
