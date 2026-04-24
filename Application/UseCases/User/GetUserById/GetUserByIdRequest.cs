using MediatR;

namespace Application.UseCases.User.GetUserById
{
    public sealed record GetUserByIdRequest : IRequest<GetUserByIdResponse>
    {
        public string Id { get; set; }
    }
}
