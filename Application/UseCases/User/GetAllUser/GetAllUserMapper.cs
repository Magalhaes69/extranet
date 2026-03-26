using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.User.GetAllUser
{
    public sealed class GetAllUserMapper : Profile
    {
        public GetAllUserMapper()
        {
            CreateMap<AppUser, UserDto>();
        }
    }
}
