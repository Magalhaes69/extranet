
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IOrderLineCartRepository : IBaseRepository<OrderLineCart>
    {
        Task<List<OrderLineCart>> GetAll(Guid OrderId);
    }
}
