
using Application.UseCases.ItemExtensions.CheckItemExtensions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.ItemExtensions.GetByItemIdItemExtensions
{
    public sealed class GetByItemIdItemExtensionsHandler : IRequestHandler<GetByItemIdItemExtensionsRequest, GetByItemIdItemExtensionsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemExtensionRepository _repository;
        private readonly IMapper _mapper;

        public GetByItemIdItemExtensionsHandler(IUnitOfWork unitOfWork,
                                IItemExtensionRepository repository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GetByItemIdItemExtensionsResponse> Handle(GetByItemIdItemExtensionsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _repository.GetByItemIdAsync(request.ItemId);
                return _mapper.Map<GetByItemIdItemExtensionsResponse>(item);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
