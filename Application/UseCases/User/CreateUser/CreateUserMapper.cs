using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.User.CreateUser
{
    public sealed class CreateUserMapper : Profile
    {
        public CreateUserMapper()
        {
            CreateMap<CreateUserRequest, AppUser>();
            CreateMap<AppUser, UserDto>();
        }
    }
}
