namespace Domain.Models
{
    /// <summary>
    /// Representa uma solicitação de licença feita por um cliente
    /// </summary>
    public class LicenseRequest
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Notes { get; set; }

        /// <summary>
        /// Estados: Submitted, InReview, AwaitingPayment, Approved, Rejected, Completed
        /// </summary>
        public string Status { get; set; } = "Submitted";

        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? ReviewNotes { get; set; }
        public string? RejectionReason { get; set; }

        // Arquivos anexados (JSON array de paths)
        public string? AttachedFiles { get; set; }

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
