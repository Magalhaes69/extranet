using Domain.Models;

namespace Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user);
    }
}
