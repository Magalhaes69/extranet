

using AutoMapper;
using Domain.Models;

namespace Application.UseCases.AppSettings.GetAllAppSetting
{
    public sealed class GetAllAppSettingMapper : Profile
    {
        public GetAllAppSettingMapper()
        {
            CreateMap<AppSetting, GetAllAppSettingResponse>();    
        }
    }
}
