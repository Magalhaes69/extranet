
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seeders
{
    public static class IdentitySeeder
    {

        public static async Task SeedAsync(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SeedSettings seedSettings)
        {
            await SeedRolesAsync(roleManager);
            await SeedManagerUserAsync(userManager, seedSettings.ManagerPassword);
        }

        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            var roles = new[] { 
                "Manager", 
                "Administrator",
                "User" 
            };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole= new AppRole
                    {
                        Name = role,
                        Description = $"{role} role"
                    };
                    await roleManager.CreateAsync(newRole);
                }
            }
        }

        private static async Task SeedManagerUserAsync(
            UserManager<AppUser> userManager,
            string password)
        {
            var managerEmail = "manager@app.co.mz";

            var user = await userManager.FindByEmailAsync(managerEmail);
            if (user is not null)
                return;

            user = new AppUser
            {
                UserName = managerEmail,
                Email = managerEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Manager",
                Nuit = "123456789",
                Theme = "dark",
                Language = "pt-PT",
                TwoFactorEnabled = false,
                CardCode = "C0000001"
            };


            var result = await userManager.CreateAsync(
                user,
                password);

            if(!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, "Manager");
        }

    }
}
