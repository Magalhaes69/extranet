
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.AppSettings.CreateAppSetting
{
    public sealed class CreateAppSettingHandler : IRequestHandler<CreateAppSettingRequest, CreateAppSettingResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppSettingRepository _repository;
        private readonly IMapper _mapper;

        public CreateAppSettingHandler(IUnitOfWork unitOfWork,
                                IAppSettingRepository repository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateAppSettingResponse> Handle(CreateAppSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var item = _mapper.Map<AppSetting>(request);
                await _repository.CreateAsync(item);
                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<CreateAppSettingResponse>(item);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
