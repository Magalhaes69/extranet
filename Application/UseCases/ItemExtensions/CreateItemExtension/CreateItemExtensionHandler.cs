using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.ItemExtensions.CreateItemExtension
{
    public sealed class CreateItemExtensionHandler : IRequestHandler<CreateItemExtensionRequest, CreateItemExtensionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemExtensionRepository _repository;
        private readonly IMapper _mapper;

        public CreateItemExtensionHandler(IUnitOfWork unitOfWork,
                                IItemExtensionRepository repository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateItemExtensionResponse> Handle(CreateItemExtensionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var item = _mapper.Map<ItemExtension>(request);
                await _repository.CreateAsync(item);
                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<CreateItemExtensionResponse>(item);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
