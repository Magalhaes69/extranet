using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.User.UpdateUser
{
    internal class UpdateUserMapper : Profile
    {
        public UpdateUserMapper()
        {
            CreateMap<UpdateUserRequest, AppUser>();
            CreateMap<AppUser, UserDto>();
        }
    }
}
