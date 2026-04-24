
using Application.UseCases.ItemExtensions.CheckItemExtensions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.ItemExtensions.DeleteItemExtensions
{
    public class DeleteItemExtensionHandler : IRequestHandler<DeleteItemExtensionRequest, DeleteItemExtensionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemExtensionRepository _repository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public DeleteItemExtensionHandler(IUnitOfWork unitOfWork,
                                IItemExtensionRepository repository,
                                IMapper mapper,
                                IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<DeleteItemExtensionResponse> Handle(DeleteItemExtensionRequest request, CancellationToken cancellationToken)
        {
            try
            {
               if(request.ItemCode == null || !request.ItemCode.Any())
                {
                    return new DeleteItemExtensionResponse { Flag = false };
                }
               var getItems = await _repository.GetAllByListItemCodeAsync(request.ItemCode);

                await _repository.DeleteManyAsync(request.ItemCode);
                await _unitOfWork.Commit(cancellationToken);


                foreach (var item in getItems)
                {
                    if(!string.IsNullOrEmpty(item.ImageUrl))
                        _fileService.Delete(item.ImageUrl);
                }
                return new DeleteItemExtensionResponse { Flag = true};
            }
            catch (Exception ex)
            {
                return new DeleteItemExtensionResponse { Flag = false };
            }
        }
    }
}
