using Application.UseCases.User.Auth;
using MediatR;

namespace Application.UseCases.User.Verify2FA
{
    public class Verify2FARequest : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
