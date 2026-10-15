
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Context;
using Persistence.Seeders;

namespace Persistence.Extensions
{
    public static class DatabaseInitializerExtension
    {
        public static async Task InitializeDatabaseAsync(
            this IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
           using var scope = serviceProvider.CreateScope();
            
            var provider = scope.ServiceProvider;

            var context = provider
                .GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            


            var userManager = provider
                .GetRequiredService<UserManager<AppUser>>();


            var roleManager = provider
                .GetRequiredService<RoleManager<AppRole>>();

            var settings = provider
                .GetRequiredService<IOptions<SeedSettings>>()
                .Value;

            await IdentitySeeder.SeedAsync(
                userManager, 
                roleManager, 
                settings);

            await AppConfigurationSeeder.SeedAsync(
                context, 
                userManager);
        }
    }
}
