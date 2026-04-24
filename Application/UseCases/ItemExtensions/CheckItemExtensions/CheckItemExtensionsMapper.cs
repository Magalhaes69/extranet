
using AutoMapper;
using Domain.Models;

namespace Application.UseCases.ItemExtensions.CheckItemExtensions
{
    public class CheckItemExtensionsMapper : Profile
    {
        public CheckItemExtensionsMapper()
        {
            CreateMap<ItemExtension, CheckItemExtensionsResponse>();
        }
    }
}
