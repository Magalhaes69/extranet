using Domain.Models;

namespace Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user);

        /// <summary>Gera o token incluindo os roles e os processos autorizados do utilizador.</summary>
        string GenerateToken(AppUser user, IEnumerable<string> roles, IEnumerable<string> processes);
    }
}
