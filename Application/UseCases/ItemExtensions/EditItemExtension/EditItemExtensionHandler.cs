using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.ItemExtensions.EditItemExtension
{
    public class EditItemExtensionHandler : IRequestHandler<EditItemExtensionRequest, EditItemExtensionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemExtensionRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public EditItemExtensionHandler(IUnitOfWork unitOfWork,
                                IItemExtensionRepository repository,
                                IMapper mapper,
                                IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<EditItemExtensionResponse> Handle(EditItemExtensionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var itemExtension = await _repository.GetByItemIdAsync(request.ItemId.ToString());
                if (itemExtension == null)
                {
                    return null!;
                }

                var oldImage = itemExtension.ImageUrl;
                if(request.ImageStream != null)
                {
                    var newImageUrl = await _fileService.SaveAsync(
                        request.ImageStream,
                        request.FileName!,
                        "items");

                    request.ImageUrl = newImageUrl;
                    if (!string.IsNullOrEmpty(oldImage) && oldImage != newImageUrl)
                    {
                        _fileService.Delete(oldImage);
                    }
                }

                itemExtension.ItemName = request.ItemName;
                itemExtension.ItemCode = request.ItemCode;
                itemExtension.ItemId = request.ItemId;
                itemExtension.ImageUrl = request.ImageUrl;

                var item = _mapper.Map<ItemExtension>(itemExtension);
                _repository.UpdateAsync(item);

                await _unitOfWork.Commit(cancellationToken);

                return _mapper.Map<EditItemExtensionResponse>(item);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
