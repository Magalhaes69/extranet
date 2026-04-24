using Application.Dtos;
using MediatR;

namespace Application.UseCases.User.UpdateUser
{
    public sealed record UpdateUserRequest : IRequest<UserDto>
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Nuit { get; set; }
        public string CardCode { get; set; }
        public string Theme { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Language { get; set; }
        public string? ImageUrl { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public Stream? ImageStream { get; set; }
        public string? FileName {  get; set; }
    }
}
