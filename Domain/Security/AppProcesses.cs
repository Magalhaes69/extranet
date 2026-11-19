using System.Security.Claims;

namespace Domain.Security
{
    /// <summary>
    /// Processos (áreas funcionais) a que o gestor da empresa pode dar acesso a cada utilizador.
    /// São guardados como claims do tipo <see cref="ClaimType"/> (tabela AspNetUserClaims) e
    /// incluídos no JWT no login.
    /// </summary>
    public static class AppProcesses
    {
        public const string ClaimType = "process";

        public const string Dashboard = "dashboard";
        public const string Requests = "requests";
        public const string Payments = "payments";
        public const string Licenses = "licenses";

        public static readonly IReadOnlyList<string> All = new[]
        {
            Dashboard, Requests, Payments, Licenses
        };

        public static bool IsValid(string? process) =>
            !string.IsNullOrWhiteSpace(process) && All.Contains(process);

        public static string PolicyName(string process) => $"process:{process}";

        /// <summary>Gestores têm acesso a todos os processos; os restantes só aos atribuídos.</summary>
        public static bool HasAccess(ClaimsPrincipal user, string process) =>
            IsCompanyManager(user) || user.HasClaim(ClaimType, process);

        public static bool IsCompanyManager(ClaimsPrincipal user) =>
            user.IsInRole(AppRoleNames.Manager) || user.IsInRole(AppRoleNames.Administrator);
    }

    public static class AppRoleNames
    {
        public const string Manager = "Manager";
        public const string Administrator = "Administrator";
        public const string User = "User";

        /// <summary>Valor para <c>[Authorize(Roles = ...)]</c>.</summary>
        public const string ManagerRoles = "Manager,Administrator";
    }
}
