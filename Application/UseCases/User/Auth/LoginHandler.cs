using System.Text;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.UseCases.User.Auth
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;

        public LoginHandler(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
        }
        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = _userManager.FindByEmailAsync(request.Email).Result;
            if(user is null) 
                throw new Exception("Invalid credentials");

            var validadPassword =await _userManager.CheckPasswordAsync(user, request.Password);

            if(!validadPassword)
                throw new Exception("Invalid credentials");

            if (user.TwoFactorEnabled)
            {
                var twoFactor = await _userManager.GenerateTwoFactorTokenAsync(
                    user,
                    TokenOptions.DefaultEmailProvider
                    );

                var getBody = BuildOtpEmail(twoFactor, "Extranet", 5);
                await _emailService.SendAsync(user.Email!, "Verificação do email", getBody);

                return new LoginResponse
                {
                    Requires2FA = true,
                    Email = user.Email!
                };
            }

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
                Language = user.Language!,
            };
        }

        private string BuildOtpEmail(string code, string appName, int expiryMinutes)
        {
            var sb = new StringBuilder();

            sb.Append("<!DOCTYPE html>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<meta charset='UTF-8'>");
            sb.Append("</head>");
            sb.Append("<body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:20px;'>");

            sb.Append("<div style='max-width:600px; margin:auto; background:#ffffff; padding:20px; border-radius:8px;'>");

            sb.Append($"<h2 style='color:#333;'>{appName} - Verificação de segurança</h2>");

            sb.Append("<p style='font-size:14px; color:#555;'>");
            sb.Append("Recebemos uma solicitação para acessar sua conta.");
            sb.Append("</p>");

            sb.Append("<p style='font-size:14px; color:#555;'>");
            sb.Append("Utilize o código de verificação abaixo para concluir seu login: ");
            sb.Append("</p>");

            sb.Append($"<div style='text-align:center; margin:30px 0;'>");
            sb.Append($"<span style='font-size:32px; letter-spacing:5px; font-weight:bold; color:#2c3e50;'>");
            sb.Append(code);
            sb.Append("</span>");
            sb.Append("</div>");

            sb.Append($"<p style='font-size:13px; color:#999;'>");
            sb.Append($"Este código expirará em {expiryMinutes} minutos.");
            sb.Append("</p>");

            sb.Append("<p style='font-size:13px; color:#999;'>");
            sb.Append("Caso não tenha solicitado isso, ignore este e-mail.");
            sb.Append("</p>");

            sb.Append("<hr style='margin:20px 0;'/>");

            sb.Append($"<p style='font-size:12px; color:#aaa;'>© {DateTime.UtcNow.Year} {appName}. All rights reserved.</p>");

            sb.Append("</div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
