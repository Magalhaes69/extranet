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

        /// <summary>
        /// Chave que identifica o evento que originou a notificação (ex.: "lic-expiring:{id}:{data}").
        /// Garante que o mesmo evento nunca notifica duas vezes o mesmo utilizador.
        /// </summary>
        public string? DedupeKey { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    /// <summary>Tipos e níveis usados nas notificações.</summary>
    public static class NotificationTypes
    {
        public const string LicenseExpiring = "LicenseExpiring";
        public const string LicenseExpired = "LicenseExpired";
        public const string TrialEnding = "TrialEnding";
        public const string RequestUpdate = "RequestUpdate";
        public const string AccountAccess = "AccountAccess";
    }

    public static class NotificationLevels
    {
        public const string Info = "Info";
        public const string Warning = "Warning";
        public const string Success = "Success";
        public const string Error = "Error";
    }
}
