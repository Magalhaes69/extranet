using AutoMapper;
using Domain.Models;

namespace Application.UseCases.ItemExtensions.GetByItemIdItemExtensions
{
    public class GetByItemIdItemExtensionsMapper : Profile
    {
        public GetByItemIdItemExtensionsMapper()
        {
            CreateMap<ItemExtension, GetByItemIdItemExtensionsResponse>();
        }
    }
}
