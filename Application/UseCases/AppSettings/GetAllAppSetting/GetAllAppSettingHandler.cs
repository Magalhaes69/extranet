
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.AppSettings.GetAllAppSetting
{
    public sealed class GetAllAppSettingHandler : IRequestHandler<GetAllAppSettingRequest, IEnumerable<GetAllAppSettingResponse>>
    {
        
        private readonly IAppSettingRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAppSettingHandler(
                                IAppSettingRepository repository,
                                IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllAppSettingResponse>> Handle(GetAllAppSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var appSettings = await _repository.GetAllAsync(cancellationToken);
                if (appSettings == null)
                {
                    return Enumerable.Empty<GetAllAppSettingResponse>();    
                }
                return _mapper.Map<IEnumerable<GetAllAppSettingResponse>>(appSettings).ToList();
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
