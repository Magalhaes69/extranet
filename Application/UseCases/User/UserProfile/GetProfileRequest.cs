using Application.Dtos;
using MediatR;

namespace Application.UseCases.User.UserProfile
{
    public class GetProfileRequest : IRequest<UserDto>
    {
        public Guid userId { get; set; }
    }
}
