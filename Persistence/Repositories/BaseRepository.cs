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
        public void CreateAsync(T entity)
        {
            entity.DateCreated = DateTime.UtcNow;

            _context.Add(entity);
        }

        public void DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync(Guid Id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync(Guid Id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x=> x.Id == Id, cancellationToken);
        }

        public void UpdateAsync(T entity)
        {
            entity.DateUpdated = DateTime.UtcNow;
            _context.Update(entity);
        }
    }
}
