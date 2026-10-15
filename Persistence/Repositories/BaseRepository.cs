using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(T entity)
        {
            try
            {
                entity.DateCreated = DateTime.UtcNow;

                await _context.AddAsync(entity);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void DeleteAsync(T entity)
        {
            _context.Remove(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(new object[] { Id }, cancellationToken);
        }

        public void UpdateAsync(T entity)
        {
            entity.DateUpdated = DateTime.UtcNow;
            _context.Update(entity);
        }
    }
}
