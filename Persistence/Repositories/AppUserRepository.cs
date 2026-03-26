using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class AppUserRepository : IBaseUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public AppUserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<List<AppUser>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _userManager.Users.ToListAsync(cancellationToken);
        }
    }
}
