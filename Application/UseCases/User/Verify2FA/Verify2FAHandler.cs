
using Application.UseCases.User.Auth;
using Application.Services;
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
        private readonly IUserActivityLogger _activity;
        public Verify2FAHandler(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator, IUserActivityLogger activity)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _activity = activity;
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

            var (roles, processes) = await UserAccessResolver.ResolveAsync(_userManager, user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles, processes);
            await _activity.LogAsync(user.Id, user.CardCode, UserActivityActions.Login);

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
