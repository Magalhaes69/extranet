using AutoMapper;
using Domain.Models;

namespace Application.UseCases.ItemExtensions.CreateItemExtension
{
    public sealed class CreateItemExtensionMapper : Profile
    {
        public CreateItemExtensionMapper()
        {
            CreateMap<CreateItemExtensionRequest, ItemExtension>();
            CreateMap<ItemExtension, CreateItemExtensionResponse>();
        }
    }
}
