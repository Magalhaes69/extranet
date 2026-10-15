using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.UseCases.AppSettings.CreateAppSetting
{
    public sealed record CreateAppSettingRequest : IRequest<CreateAppSettingResponse>
    {
        [MaxLength(25)]
        [Required]
        public string Key { get; set; } = default!;
        [MaxLength(100)]
        public string? Value { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
