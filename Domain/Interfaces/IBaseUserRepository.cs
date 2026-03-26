using System;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces
{
    public interface IBaseUserRepository
    {
        Task<IdentityResult> CreateAsync(AppUser user, string password);

        Task<IdentityResult> UpdateAsync(AppUser user);

        Task<IdentityResult> DeleteAsync(AppUser user);

        Task<AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task<List<AppUser>> GetAllAsync(CancellationToken cancellationToken);
        
    }
}
