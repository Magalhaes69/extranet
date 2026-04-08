

using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUnitOfWork unitOfWork,
                                IBaseUserRepository appUserRepository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = appUserRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return null!;
            }
            else {
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Nuit = request.Nuit;
                user.CardCode = request.CardCode;
                user.DateUpdated = DateTime.UtcNow;
                user.UserName = request.Email;
                user.TwoFactorEnabled = request.TwoFactorEnabled;
                user.Language = request.Language;
                user.Theme = request.Theme;
            }

            var result = _mapper.Map<AppUser>(user);
            await _userRepository.UpdateAsync(result, request.PhoneNumber!);
            
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
