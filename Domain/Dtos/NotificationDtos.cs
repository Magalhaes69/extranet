namespace Domain.Dtos
{
    /// <summary>
    /// DTO para notificação
    /// </summary>
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = "Info";
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? ActionUrl { get; set; }
        public string? ActionText { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO para marcar notificação como lida
    /// </summary>
    public class MarkNotificationReadDto
    {
        public Guid NotificationId { get; set; }
    }
}
