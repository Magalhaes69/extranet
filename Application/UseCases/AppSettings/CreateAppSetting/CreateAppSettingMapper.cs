using AutoMapper;
using Domain.Models;

namespace Application.UseCases.AppSettings.CreateAppSetting
{
    public sealed class CreateAppSettingMapper : Profile
    {
        public CreateAppSettingMapper()
        {
            CreateMap<CreateAppSettingRequest, AppSetting>();
            CreateMap<AppSetting, CreateAppSettingResponse>();
        }
    }
}
