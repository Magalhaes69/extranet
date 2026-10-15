namespace WebFrontEnd.Services.LicenseService.Dto
{
    public sealed class LicenseKey
    {
        public string CardCode { get; set; } = string.Empty;

        public bool IsValid { get; set; }

        public List<string> Modules { get; set; } = new();

        public DateTime? Expiration { get; set; }
    }
}
