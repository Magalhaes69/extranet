using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        Task CreateAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    }
}
