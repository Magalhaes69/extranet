

using AutoMapper;
using Domain.Models;

namespace Application.UseCases.User.DefinePassword
{
    public sealed class SetPasswordProfile : Profile
    {
        public SetPasswordProfile()
        {
            CreateMap<AppUser, SetPasswordResponse>();
        }
    }
}
