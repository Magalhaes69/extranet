namespace Domain.Dtos
{
    /// <summary>
    /// DTO para solicitação de licença
    /// </summary>
    public class LicenseRequestDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ReviewNotes { get; set; }
        public string? RejectionReason { get; set; }
        public List<string> AttachedFiles { get; set; } = new();
    }

    /// <summary>
    /// DTO para criar nova solicitação
    /// </summary>
    public class CreateLicenseRequestDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO para histórico de solicitações
    /// </summary>
    public class LicenseRequestHistoryDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
