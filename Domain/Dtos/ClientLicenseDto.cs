namespace Domain.Dtos
{

    public sealed class ClientLicenseDto
    {
        public Guid Id { get; set; }
        public string MaskedKey { get; set; } = string.Empty;

        public int LicenseType { get; set; }
        public int ApprovalStatus { get; set; }

        public bool IsInactive { get; set; }
        public string OperationalStatus { get; set; } = string.Empty;

        public DateTime? IssuedAt { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public List<ClientLicenseModuleDto> Modules { get; set; } = new();
    }
}