
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.ItemExtensions.DeleteItemExtensions
{
    public class DeleteItemExtensionMapper : Profile
    {
        public DeleteItemExtensionMapper()
        {
            CreateMap<DeleteItemExtensionRequest, ItemExtension>();
            CreateMap<ItemExtension, DeleteItemExtensionResponse>();
        }
    }
}
