using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        //public DbSet<AppUser> AppUsers { get; set; }
        //public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<OrdersCart> OrdersCart { get; set; }
        public DbSet<OrderLineCart> OrderLineCart { get; set; }
        public DbSet<ItemExtension> ItemExtension { get; set; }
        public DbSet<AppSetting> AppSetting { get;set; }

    }
}
