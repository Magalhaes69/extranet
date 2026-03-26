using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task Commit(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
