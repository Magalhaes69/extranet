namespace Application.UseCases.User.Auth
{
    public sealed record LoginResponse
    {
        public bool Success { get; set; } = true;
        public string Token { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string CardCode { get; set; }
        public string UniqueName { get; set; }
        public bool Requires2FA { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public string Message { get; set; }
        public string Theme { get; set; } = "light";
        public string Language { get; set; } = "pt";
    }
}
