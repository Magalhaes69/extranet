using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task<T> GetAsync(Guid Id, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(Guid Id, CancellationToken cancellationToken);
    }
}
