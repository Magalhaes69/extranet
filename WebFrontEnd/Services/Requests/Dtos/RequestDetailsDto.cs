namespace WebFrontEnd.Services.Requests.Dtos
{
    public sealed class RequestDetailsDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? RejectionReason { get; set; }
        public string? InternalNotes { get; set; }

        // Informações adicionais
        public decimal? EstimatedValue { get; set; }
        public string? Currency { get; set; }
        public string? ApprovedBy { get; set; }
        public string? ProcessedBy { get; set; }
    }
}
