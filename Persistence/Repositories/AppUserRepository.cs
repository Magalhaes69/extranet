using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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

        public async Task<IdentityResult> UpdateAsync(AppUser user, string phoneNumber)
        {
            try
            {
                await UpdatePhoneNumberAsync(user, phoneNumber);
                return await _userManager.UpdateAsync(user);
            }
            catch(Exception ex)
            {
                return null!;
            }
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

        public async Task<IdentityResult> UpdatePhoneNumberAsync(AppUser user, string phoneNumber)
        {
            try
            {
                var result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);

                return result;
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Unexpected error while updating phone number"
                });
            }
        }

        public Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword) =>
                _userManager.ResetPasswordAsync(user, token, newPassword);

        public Task<IdentityResult> SedUpdateAsync(AppUser user) =>
            _userManager.UpdateAsync(user);

        
    }
}
