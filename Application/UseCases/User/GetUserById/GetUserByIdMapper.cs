
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.User.GetUserById
{
    public class GetUserByIdMapper : Profile
    {
        public GetUserByIdMapper()
        {
            CreateMap<AppUser, GetUserByIdResponse>();
        }
    }
}
