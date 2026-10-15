
using MediatR;

namespace Application.UseCases.AppSettings.GetAllAppSetting
{
    public sealed record GetAllAppSettingRequest : IRequest<IEnumerable<GetAllAppSettingResponse>>
    {
    }
}
