using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.Auth
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginHandler(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = _userManager.FindByEmailAsync(request.Email).Result;
            if(user is null) 
                throw new Exception("Invalid credentials");

            var validadPassword =await _userManager.CheckPasswordAsync(user, request.Password);

            if(!validadPassword)
                throw new Exception("Invalid credentials");

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new LoginResponse
            {
                Email = user.Email!,
                UserId = user.Id,
                Token = token,
                CardCode = user.CardCode,
                Name = user.FirstName!,
                UniqueName = user.Email!,
            };
        }
    }
}
