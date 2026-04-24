
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.ItemExtensions.GetAllItemExtensions
{
    public class GetAllItemExtensionsMapper : Profile
    {
        public GetAllItemExtensionsMapper()
        {
            CreateMap<ItemExtension, GetAllItemExtensionsResponse>();
        }
    }
}
