namespace WebFrontEnd.Services.Auth
{
    public class AuthResponse
    {
        public string token { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string CardCode { get; set; }
        public string UniqueName { get; set; }
        public bool Requires2FA { get; set; }
        public string? Theme { get; set; }
        public string? Language { get; set; }

        /// <summary>
        /// Indica se o email já foi confirmado. A API ainda não envia este campo:
        /// ausente chega como <c>null</c> e é tratado como confirmado, para o login
        /// se manter como está. Quando o backend passar a enviá-lo, um <c>false</c>
        /// encaminha o utilizador para /confirm-email em vez de o autenticar.
        /// </summary>
        public bool? EmailConfirmed { get; set; }
    }
}
