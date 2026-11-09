namespace Domain.Dtos
{
    public sealed class ClientLicenseModuleDto
    {
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }

        public bool IsEnabled { get; set; }
        public bool ModuleAvailable { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public int MaxUsers { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}