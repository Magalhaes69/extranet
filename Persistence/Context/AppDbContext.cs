using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<AppUser> AppUsers { get; set; }

    }
}
