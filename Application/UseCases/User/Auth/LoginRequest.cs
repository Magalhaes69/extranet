using MediatR;

namespace Application.UseCases.User.Auth
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
