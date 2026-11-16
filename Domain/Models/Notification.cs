namespace Domain.Models
{
    /// <summary>
    /// Representa uma notificação para o cliente
    /// </summary>
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Tipos: RequestUpdate, PaymentConfirmed, PaymentPending,
        /// LicenseExpiring, LicenseExpired, AdminAnnouncement
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Níveis: Info, Warning, Success, Error
        /// </summary>
        public string Level { get; set; } = "Info";

        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }

        // Link opcional para ação relacionada
        public string? ActionUrl { get; set; }
        public string? ActionText { get; set; }

        // Referência opcional ao objeto relacionado
        public string? RelatedEntityType { get; set; } // License, Request, Payment
        public string? RelatedEntityId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
