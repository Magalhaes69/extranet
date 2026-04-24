using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.ItemExtensions.CheckItemExtensions
{
    public sealed class CheckItemExtensionsHandler : IRequestHandler<CheckItemExtensionsRequest, CheckItemExtensionsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemExtensionRepository _repository;
        private readonly IMapper _mapper;

        public CheckItemExtensionsHandler(IUnitOfWork unitOfWork,
                                IItemExtensionRepository repository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CheckItemExtensionsResponse> Handle(CheckItemExtensionsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _repository.GetByItemCodeAsync(request.ItemCode);
                return _mapper.Map<CheckItemExtensionsResponse>(item);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
