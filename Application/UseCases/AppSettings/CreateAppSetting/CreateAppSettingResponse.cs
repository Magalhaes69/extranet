
namespace Application.UseCases.AppSettings.CreateAppSetting
{
    public sealed record CreateAppSettingResponse
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
    }
}
