using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Seeders
{
    public static class AppConfigurationSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            var defaultsSettings = DefaultAppConfigurations.GetAppSettings();


            var manager = await userManager.FindByEmailAsync("manager@app.co.mz");
            foreach (var setting in defaultsSettings)
            {
                var existingSetting = await context.AppSetting
                    .AnyAsync(s => s.Key == setting.Key);
                if (!existingSetting)
                {
                    setting.UserId = manager?.Id ?? string.Empty;
                    context.AppSetting.Add(setting);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
