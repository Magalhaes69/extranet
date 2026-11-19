using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

namespace Persistence.Repositories
{
    public sealed class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(AppDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(
            string userId, string? cardCode, string type, string level, string title, string message,
            string? actionUrl = null, string? actionText = null, string? dedupeKey = null,
            string? relatedEntityType = null, string? relatedEntityId = null, DateTime? createdAt = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            try
            {
                if (dedupeKey is not null &&
                    await _context.Set<Notification>().AnyAsync(n => n.UserId == userId && n.DedupeKey == dedupeKey))
                    return false;

                _context.Set<Notification>().Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CardCode = cardCode ?? string.Empty,
                    Type = type,
                    Level = level,
                    Title = Cut(title, 150),
                    Message = Cut(message, 500),
                    ActionUrl = actionUrl,
                    ActionText = actionText,
                    DedupeKey = dedupeKey,
                    RelatedEntityType = relatedEntityType,
                    RelatedEntityId = relatedEntityId,
                    CreatedAt = createdAt ?? DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Corrida entre dois pedidos com a mesma chave (índice único) ou falha de BD: ignora.
                _logger.LogWarning(ex, "Não foi possível criar a notificação {Type} para {UserId}", type, userId);
                _context.ChangeTracker.Clear();
                return false;
            }
        }

        public async Task<(List<Notification> Items, int Unread)> GetAsync(
            string userId, bool onlyUnread, int take, CancellationToken cancellationToken)
        {
            var query = _context.Set<Notification>().AsNoTracking().Where(n => n.UserId == userId);
            var unread = await query.CountAsync(n => !n.IsRead, cancellationToken);

            if (onlyUnread)
                query = query.Where(n => !n.IsRead);

            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Take(take)
                .ToListAsync(cancellationToken);

            return (items, unread);
        }

        public Task<int> CountUnreadAsync(string userId, CancellationToken cancellationToken) =>
            _context.Set<Notification>().CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        public async Task MarkReadAsync(string userId, Guid id, CancellationToken cancellationToken)
        {
            var notification = await _context.Set<Notification>()
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId, cancellationToken);

            if (notification is null || notification.IsRead)
                return;

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkAllReadAsync(string userId, CancellationToken cancellationToken)
        {
            await _context.Set<Notification>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(n => n.IsRead, true)
                    .SetProperty(n => n.ReadAt, DateTime.UtcNow), cancellationToken);
        }

        private static string Cut(string value, int max) => value.Length <= max ? value : value[..max];
    }
}
