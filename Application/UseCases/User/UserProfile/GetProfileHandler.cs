using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.User.UserProfile
{
    public class GetProfileHandler : IRequestHandler<GetProfileRequest, UserDto>
    {
       private readonly UserManager<AppUser> _userManager;

        public GetProfileHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserDto> Handle(GetProfileRequest request, CancellationToken cancellationToken)
        {
           var user = await _userManager.FindByIdAsync(request.userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Nuit = user.Nuit,
                CardCode = user.CardCode,
            };
           
        }
    }
}
