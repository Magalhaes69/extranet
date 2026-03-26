using Application.Dtos;
using MediatR;

namespace Application.UseCases.User.GetAllUser
{
    public sealed record GetAllUserRequest : IRequest<List<UserDto>>;
    
}
