namespace Application.Dtos
{
    public abstract record AppSettingBase
    {
        public string Key { get; set; }
        public string? Value { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
    }
}
