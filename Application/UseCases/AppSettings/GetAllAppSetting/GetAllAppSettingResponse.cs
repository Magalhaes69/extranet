
using Application.Dtos;

namespace Application.UseCases.AppSettings.GetAllAppSetting
{
    public sealed record GetAllAppSettingResponse : AppSettingBase
    {
        public Guid Id { get; set; }
    }
}
