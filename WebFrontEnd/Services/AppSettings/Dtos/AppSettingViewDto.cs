
namespace WebFrontEnd.Services.AppSettings.Dtos
{
    public class AppSettingViewDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = default!;
        public string? Value { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? DataType { get; set; }
    }
}
