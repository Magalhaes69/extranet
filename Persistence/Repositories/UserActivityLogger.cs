using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

namespace Persistence.Repositories
{
    public sealed class UserActivityLogger : IUserActivityLogger
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserActivityLogger> _logger;

        public UserActivityLogger(AppDbContext context, ILogger<UserActivityLogger> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAsync(string userId, string? cardCode, string action, string? details = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return;

            try
            {
                _context.Set<UserActivity>().Add(new UserActivity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CardCode = cardCode,
                    Action = action,
                    Details = details is { Length: > 300 } ? details[..300] : details,
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Não foi possível registar a actividade {Action} do utilizador {UserId}", action, userId);
            }
        }

        public async Task<(List<UserActivity> Items, int Total)> GetAsync(
            string userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Set<UserActivity>().AsNoTracking().Where(x => x.UserId == userId);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }
    }
}
