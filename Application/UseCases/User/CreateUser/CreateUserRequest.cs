using Application.Dtos;
using MediatR;

namespace Application.UseCases.User.CreateUser
{
    public sealed record CreateUserRequest(string Email, 
                                            string Password, 
                                            string FirstName, 
                                            string LastName, 
                                            string Nuit, 
                                            string CardCode) :
                                            IRequest<UserDto>;

}
