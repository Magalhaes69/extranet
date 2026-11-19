using Domain.Models;
using Domain.Security;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    /// <summary>Calcula roles e processos de um utilizador para o colocar no JWT.</summary>
    public static class UserAccessResolver
    {
        public static async Task<(IList<string> Roles, IList<string> Processes)> ResolveAsync(
            UserManager<AppUser> userManager, AppUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            // Contas criadas no registo público não recebem role. O primeiro utilizador de cada
            // empresa (CardCode) é o dono e passa a gestor, para poder gerir os restantes.
            if (roles.Count == 0)
            {
                var firstUserId = userManager.Users
                    .Where(u => u.CardCode == user.CardCode)
                    .OrderBy(u => u.DateCreated)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                if (firstUserId == user.Id)
                {
                    await userManager.AddToRoleAsync(user, AppRoleNames.Manager);
                    roles = await userManager.GetRolesAsync(user);
                }
            }

            var isManager = roles.Contains(AppRoleNames.Manager) || roles.Contains(AppRoleNames.Administrator);

            IList<string> processes = isManager
                ? AppProcesses.All.ToList()
                : (await userManager.GetClaimsAsync(user))
                    .Where(c => c.Type == AppProcesses.ClaimType && AppProcesses.IsValid(c.Value))
                    .Select(c => c.Value)
                    .Distinct()
                    .ToList();

            return (roles, processes);
        }
    }
}
