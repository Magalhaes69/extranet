using Application.Dtos;
using AutoMapper;
using Domain;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

using System.Text;

namespace Application.UseCases.User.DefinePassword
{
    public sealed class SetPasswordHandler : IRequestHandler<SetPasswordRequest, SetPasswordResponse>
    {
        private const string InvalidLinkMessage = "Link inválido ou expirado.";

        private readonly IBaseUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SetPasswordHandler(IBaseUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SetPasswordResponse> Handle(SetPasswordRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.Password))
                return Fail(InvalidLinkMessage);

            var user = await _userRepository.GetByEmailAsync(request.Email.Trim(), cancellationToken);
            if (user is null)
                return Fail(InvalidLinkMessage);

            if (!TryDecodeToken(request.Token, out var token))
                return Fail(InvalidLinkMessage);

            var result = await _userRepository.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
            {
                // An expired or reused link comes back as "InvalidToken"
                var invalidToken = result.Errors.Any(e => e.Code == "InvalidToken");
                return Fail(invalidToken
                    ? InvalidLinkMessage
                    : string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            user.EmailConfirmed = true; // the e-mail is proven by the link
            user.DateUpdated = DateTime.UtcNow;

            await _userRepository.SedUpdateAsync(user);
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<SetPasswordResponse>(user);
        }

        private static bool TryDecodeToken(string encoded, out string token)
        {
            try
            {
                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encoded));
                return true;
            }
            catch (FormatException)
            {
                token = string.Empty;
                return false;
            }
        }

        private static SetPasswordResponse Fail(string message) => new() { Token = message };
    }
}
