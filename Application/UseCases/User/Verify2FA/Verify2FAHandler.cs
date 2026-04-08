
using Application.UseCases.User.Auth;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.Verify2FA
{
    public class Verify2FAHandler : IRequestHandler<Verify2FARequest, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public Verify2FAHandler(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponse> Handle(Verify2FARequest request, CancellationToken cancellationToken)
        {
           var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApplicationException("Invalid user");
            }
                var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user,
                    TokenOptions.DefaultEmailProvider,
                    request.Code);


            if (!isValid)
                throw new Exception("Invalid or expired code");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponse
            {
                Email = user.Email!,
                UserId = user.Id,
                Token = token,
                CardCode = user.CardCode,
                Name = user.FirstName!,
                UniqueName = user.Email!,
                Theme = user.Theme!,
                Language = user.Language!
            };
        }
    }
}
