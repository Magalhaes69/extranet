using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    /// <summary>
    /// Registo de uma acção relevante feita por um utilizador (login, gestão de utilizadores, licenças).
    /// O texto mostrado ao utilizador vem do código <see cref="Action"/> (traduzido no frontend);
    /// <see cref="Details"/> guarda só o dado variável (ex.: email do utilizador criado, produto pedido).
    /// </summary>
    public class UserActivity
    {
        public Guid Id { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? CardCode { get; set; }

        [Required, MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Details { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    /// <summary>Códigos de acção guardados em <see cref="UserActivity.Action"/>.</summary>
    public static class UserActivityActions
    {
        public const string Login = "login";
        public const string PasswordChanged = "password.changed";
        public const string ProfileUpdated = "profile.updated";
        public const string UserCreated = "user.created";
        public const string UserUpdated = "user.updated";
        public const string UserDeactivated = "user.deactivated";
        public const string UserActivated = "user.activated";
        public const string LicenseRequested = "license.requested";
        public const string LicenseRenewalRequested = "license.renewal";
    }
}
