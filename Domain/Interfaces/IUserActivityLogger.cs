using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUserActivityLogger
    {
        /// <summary>Regista uma acção. Nunca lança: uma falha no registo não pode partir a operação principal.</summary>
        Task LogAsync(string userId, string? cardCode, string action, string? details = null);

        /// <summary>Actividade do utilizador, da mais recente para a mais antiga.</summary>
        Task<(List<UserActivity> Items, int Total)> GetAsync(string userId, int page, int pageSize, CancellationToken cancellationToken);
    }
}
