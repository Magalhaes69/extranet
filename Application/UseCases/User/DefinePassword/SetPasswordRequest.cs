
using Domain;
using MediatR;

namespace Application.UseCases.User.DefinePassword
{
    public sealed record SetPasswordRequest : IRequest<SetPasswordResponse>
    {
        public string Email { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
