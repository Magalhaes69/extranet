namespace Domain.Dtos
{
    /// <summary>
    /// DTO para pagamento
    /// </summary>
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "MZN";
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionReference { get; set; }
        public string? PaymentReference { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? Notes { get; set; }
        public string? ReceiptUrl { get; set; }
        public string? InvoiceUrl { get; set; }
        public string? RelatedLicenseId { get; set; }
    }

    /// <summary>
    /// DTO para histórico financeiro
    /// </summary>
    public class FinancialHistoryDto
    {
        public List<PaymentDto> Payments { get; set; } = new();
        public decimal TotalPaid { get; set; }
        public decimal TotalPending { get; set; }
        public int PendingCount { get; set; }
        public int CompletedCount { get; set; }
    }
}
