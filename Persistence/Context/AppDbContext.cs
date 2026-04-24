using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<OrdersCart> OrdersCart { get; set; }
        public DbSet<OrderLineCart> OrderLineCart { get; set; }
        public DbSet<ItemExtension> ItemExtension { get; set; }

    }
}
