using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebFrontEnd.Services.Auth
{
    /// <summary>
    /// Permissões do utilizador autenticado (role de gestor e processos atribuídos), lidas das
    /// claims do JWT. É só para esconder menus e páginas; a API volta a validar tudo.
    /// </summary>
    public class AccessService
    {
        private readonly AuthenticationStateProvider _auth;

        public AccessService(AuthenticationStateProvider auth)
        {
            _auth = auth;
        }

        private async Task<ClaimsPrincipal> UserAsync() =>
            (await _auth.GetAuthenticationStateAsync()).User;

        public async Task<bool> IsAuthenticatedAsync() =>
            (await UserAsync()).Identity?.IsAuthenticated == true;

        public async Task<bool> IsManagerAsync()
        {
            var user = await UserAsync();
            return user.Claims.Any(c =>
                (c.Type == ClaimTypes.Role || c.Type == "role") &&
                (c.Value == "Manager" || c.Value == "Administrator"));
        }

        public async Task<bool> HasProcessAsync(string process)
        {
            if (await IsManagerAsync())
                return true;
            var user = await UserAsync();
            return user.Claims.Any(c => c.Type == "process" && c.Value == process);
        }

        /// <summary>Processo exigido por uma rota (null = sem restrição de processo).</summary>
        public static string? RequiredProcess(string path)
        {
            var p = Normalize(path);
            if (p.Length == 0) return "dashboard";

            string[] requests =
            {
                "orders", "orders-details", "add-orders", "catalog-item", "quotations", "quotation-details",
                "returns", "delivery", "delivery-details", "invoices", "invoices-details", "credits", "credit-details"
            };
            if (requests.Any(r => p == r || p.StartsWith(r + "/"))) return "requests";
            if (p == "payments" || p.StartsWith("incomingpayments-details")) return "payments";
            if (p == "license" || p.StartsWith("license/")) return "licenses";
            return null;
        }

        public static bool RequiresManager(string path)
        {
            var p = Normalize(path);
            return p.StartsWith("company-users") || p.StartsWith("configure-app");
        }

        public async Task<bool> CanAccessAsync(string path)
        {
            if (!await IsAuthenticatedAsync())
                return true; // o login é tratado pelo AuthorizeRouteView

            if (RequiresManager(path) && !await IsManagerAsync())
                return false;

            var process = RequiredProcess(path);
            return process is null || await HasProcessAsync(process);
        }

        /// <summary>Primeira página a que o utilizador tem acesso (destino quando não pode ver o painel).</summary>
        public async Task<string?> FirstAllowedRouteAsync()
        {
            if (await HasProcessAsync("dashboard")) return "/";
            if (await HasProcessAsync("requests")) return "/orders";
            if (await HasProcessAsync("payments")) return "/payments";
            if (await HasProcessAsync("licenses")) return "/license";
            return null;
        }

        private static string Normalize(string path)
        {
            var p = path.Split('?', '#')[0].Trim('/').ToLowerInvariant();
            return p;
        }
    }
}
