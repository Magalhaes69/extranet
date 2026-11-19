using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.User.UpdateUser
{
    public class UpdateUserHandler
        : IRequestHandler<UpdateUserRequest, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateUserHandler(
            IUnitOfWork unitOfWork,
            IBaseUserRepository appUserRepository,
            IMapper mapper,
            IFileService file)
        {
            _unitOfWork = unitOfWork;
            _userRepository = appUserRepository;
            _mapper = mapper;
            _fileService = file;
        }

        public async Task<UserDto> Handle(
            UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            // 1. Obter utilizador
            var user = await _userRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (user == null)
                throw new Exception("Utilizador não encontrado.");

            var oldImage = user.ImageUrl;
            string? newImageUrl = null;

            // 2. Actualizar fotografia, se fornecida
            if (request.ImageStream != null)
            {
                if (string.IsNullOrWhiteSpace(request.FileName))
                    throw new Exception("Nome do ficheiro não informado.");

                newImageUrl = await _fileService.SaveAsync(
                    request.ImageStream,
                    request.FileName,
                    "users");

                user.ImageUrl = newImageUrl;
            }

            // 3. Actualizar apenas os dados permitidos
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Theme = request.Theme ?? user.Theme;
            user.Language = request.Language ?? user.Language;

            // Não permitir alteração de Email, Nuit, CardCode
            // nem TwoFactorEnabled através deste endpoint.

            user.DateUpdated = DateTimeOffset.UtcNow;

            // 4. Actualizar telefone e utilizador
            var phoneNumber = request.PhoneNumber
                ?? user.PhoneNumber
                ?? string.Empty;

            var updateResult = await _userRepository.UpdateAsync(
                user,
                phoneNumber);

            if (updateResult == null || !updateResult.Succeeded)
            {
                throw new Exception(
                    updateResult == null
                        ? "Erro ao actualizar utilizador."
                        : string.Join("; ", updateResult.Errors.Select(x => x.Description)));
            }

            // 5. Commit
            await _unitOfWork.Commit(cancellationToken);

            // 6. Remover fotografia anterior depois de guardar
            if (!string.IsNullOrWhiteSpace(newImageUrl) &&
                !string.IsNullOrWhiteSpace(oldImage) &&
                oldImage != newImageUrl)
            {
                _fileService.Delete(oldImage);
            }

            // 7. Resultado
            return _mapper.Map<UserDto>(user);
        }
    }
}

//using Application.Dtos;
//using AutoMapper;
//using Domain.Interfaces;
//using Domain.Models;
//using MediatR;
//namespace Application.UseCases.User.UpdateUser
//{
//    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UserDto>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IBaseUserRepository _userRepository;
//        private readonly IMapper _mapper;
//        private IFileService _fileService;

//        public UpdateUserHandler(IUnitOfWork unitOfWork,
//                                IBaseUserRepository appUserRepository,
//                                IMapper mapper,
//                                IFileService file)
//        {
//            _unitOfWork = unitOfWork;
//            _userRepository = appUserRepository;
//            _mapper = mapper;
//            _fileService = file;
//        }
//        public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
//        {
//            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
//            if (user == null)
//            {
//                return null!;
//            }
//            else {

//                    var oldImage = user.ImageUrl;
//                if (request.ImageStream != null)
//                {
//                    var newImageUrl = await _fileService.SaveAsync(
//                        request.ImageStream,
//                        request.FileName!,
//                        "users"
//                        );

//                    request.ImageUrl = newImageUrl;
//                    if (!string.IsNullOrEmpty(oldImage) && oldImage != newImageUrl)
//                    {
//                        _fileService.Delete(oldImage);
//                    }
//                }-
//                user.Email = request.Email;
//                user.FirstName = request.FirstName;
//                user.LastName = request.LastName;
//                user.Nuit = request.Nuit;
//                user.CardCode = request.CardCode;
//                user.DateUpdated = DateTime.UtcNow;
//                user.UserName = request.Email;
//                user.TwoFactorEnabled = request.TwoFactorEnabled;
//                user.Language = request.Language;
//                user.Theme = request.Theme;
//                user.ImageUrl = request.ImageUrl;
//            }

//            var result = _mapper.Map<AppUser>(user);
//            await _userRepository.UpdateAsync(result, request.PhoneNumber!);

//            await _unitOfWork.Commit(cancellationToken);

//            return _mapper.Map<UserDto>(user);
//        }
//    }
//}
