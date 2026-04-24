using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByIdHandler(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.FindByIdAsync(request.Id);
                if (result == null)
                    return null!;
                        
                return new GetUserByIdResponse
                {
                    Id = Guid.Parse(result.Id),
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Nuit = result.Nuit,
                    CardCode = result.CardCode,
                    Theme = result.Theme!,
                    PhoneNumber = result.PhoneNumber,
                    Language = result.Language,
                    TwoFactorEnabled = result.TwoFactorEnabled,
                    ImageUrl = result.ImageUrl,
                };
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
