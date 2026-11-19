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
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Notification>(e =>
            {
                e.ToTable(nameof(Notification));
                e.HasKey(x => x.Id);
                e.Property(x => x.UserId).IsRequired().HasMaxLength(450);
                e.Property(x => x.CardCode).HasMaxLength(50);
                e.Property(x => x.Title).IsRequired().HasMaxLength(150);
                e.Property(x => x.Message).IsRequired().HasMaxLength(500);
                e.Property(x => x.Type).IsRequired().HasMaxLength(50);
                e.Property(x => x.Level).IsRequired().HasMaxLength(20);
                e.Property(x => x.ActionUrl).HasMaxLength(300);
                e.Property(x => x.ActionText).HasMaxLength(50);
                e.Property(x => x.RelatedEntityType).HasMaxLength(50);
                e.Property(x => x.RelatedEntityId).HasMaxLength(100);
                e.Property(x => x.DedupeKey).HasMaxLength(200);
                e.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedAt });
                // Um evento nunca notifica duas vezes o mesmo utilizador.
                e.HasIndex(x => new { x.UserId, x.DedupeKey })
                    .IsUnique()
                    .HasFilter("[DedupeKey] IS NOT NULL");
            });

            builder.Entity<UserActivity>(e =>
            {
                e.ToTable(nameof(UserActivity));
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.UserId, x.CreatedAt });
            });
        }

    }
}
