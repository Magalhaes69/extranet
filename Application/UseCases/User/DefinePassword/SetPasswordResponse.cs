

namespace Application.UseCases.User.DefinePassword
{
    public sealed record SetPasswordResponse
    {
        public string Email { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
