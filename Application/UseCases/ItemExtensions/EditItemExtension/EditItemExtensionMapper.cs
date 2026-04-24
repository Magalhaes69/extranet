
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.ItemExtensions.EditItemExtension
{
    public sealed class EditItemExtensionMapper : Profile
    {
        public EditItemExtensionMapper()
        {
            CreateMap<EditItemExtensionRequest, ItemExtension>();
            CreateMap<ItemExtension, EditItemExtensionResponse>();
        }
    }
}
