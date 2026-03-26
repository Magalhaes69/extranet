using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.User.GetAllUser
{
    public sealed class GetAllUserHandler : IRequestHandler<GetAllUserRequest, List<UserDto>>
    {
        private readonly IBaseUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetAllUserHandler(IBaseUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
        