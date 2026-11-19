using Domain.Models;

namespace Domain.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Cria a notificação. Se <paramref name="dedupeKey"/> já existir para o utilizador, não faz nada
        /// (devolve false). Nunca lança: uma falha aqui não pode partir a operação principal.
        /// </summary>
        Task<bool> AddAsync(
            string userId,
            string? cardCode,
            string type,
            string level,
            string title,
            string message,
            string? actionUrl = null,
            string? actionText = null,
            string? dedupeKey = null,
            string? relatedEntityType = null,
            string? relatedEntityId = null,
            DateTime? createdAt = null);

        Task<(List<Notification> Items, int Unread)> GetAsync(
            string userId, bool onlyUnread, int take, CancellationToken cancellationToken);

        Task<int> CountUnreadAsync(string userId, CancellationToken cancellationToken);

        Task MarkReadAsync(string userId, Guid id, CancellationToken cancellationToken);

        Task MarkAllReadAsync(string userId, CancellationToken cancellationToken);
    }
}
