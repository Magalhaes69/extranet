namespace Domain.Models
{
    /// <summary>
    /// Representa um pagamento associado a uma licença ou solicitação
    /// </summary>
    public class Payment
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;

        public Guid? LicenseRequestId { get; set; }
        public string? LicenseId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "MZN";

        /// <summary>
        /// Estados: Pending, Processing, Confirmed, Failed, Refunded
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Método: BankTransfer, MBankWallet, MPesa, Card, Other
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;

        public string? TransactionReference { get; set; }
        public string? PaymentReference { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }

        public string? Notes { get; set; }

        // URLs de documentos
        public string? ReceiptUrl { get; set; }
        public string? InvoiceUrl { get; set; }

        // Auditoria
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
