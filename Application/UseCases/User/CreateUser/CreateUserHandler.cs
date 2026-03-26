using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UseCases.User.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUnitOfWork unitOfWork,
                                IBaseUserRepository appUserRepository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = appUserRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            AppUser appUser = new AppUser()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Nuit = request.Nuit,
                CardCode = request.CardCode,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UserName = request.Email
            };

            var user = _mapper.Map<AppUser>(appUser);
            await _userRepository.CreateAsync(user, request.Password);
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
