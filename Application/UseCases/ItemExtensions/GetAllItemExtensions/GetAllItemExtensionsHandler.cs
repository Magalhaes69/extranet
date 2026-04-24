using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.ItemExtensions.GetAllItemExtensions
{
    public class GetAllItemExtensionsHandler : IRequestHandler<GetAllItemExtensionsRequest, List<GetAllItemExtensionsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemExtensionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllItemExtensionsHandler(IUnitOfWork unitOfWork,
                                IItemExtensionRepository repository,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetAllItemExtensionsResponse>> Handle(GetAllItemExtensionsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _repository.GetAllAsync(cancellationToken);

                return _mapper.Map<IEnumerable<GetAllItemExtensionsResponse>>(item).ToList();
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
