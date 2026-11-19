namespace Domain.Dtos
{
    /// <summary>Pedido (nova licença ou renovação) registado no LicenseKey. Espelha o DTO do LicenseKey.</summary>
    public sealed class ClientLicenseRequestDto
    {
        public Guid Id { get; set; }

        /// <summary>1 = Nova licença, 2 = Renovação.</summary>
        public int Type { get; set; }

        /// <summary>1 = Submetido, 2 = Em análise, 3 = Aprovado, 4 = Rejeitado, 5 = Concluído, 6 = Cancelado.</summary>
        public int Status { get; set; }

        public Guid? LicenseKeyId { get; set; }
        public string? MaskedKey { get; set; }

        public string? ProductName { get; set; }
        public int? Users { get; set; }
        public int? Months { get; set; }
        public string? Notes { get; set; }

        public string RequestedByName { get; set; } = string.Empty;
        public string RequestedByEmail { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ReviewNotes { get; set; }
        public string? RejectionReason { get; set; }
    }

    /// <summary>Corpo enviado ao LicenseKey para registar um pedido.</summary>
    public sealed class CreateClientLicenseRequestDto
    {
        public int Type { get; set; }
        public Guid? LicenseKeyId { get; set; }
        public string? ProductName { get; set; }
        public int? Users { get; set; }
        public int? Months { get; set; }
        public string? Notes { get; set; }
        public string RequestedByName { get; set; } = string.Empty;
        public string RequestedByEmail { get; set; } = string.Empty;
    }

    /// <summary>Resultado da criação de um pedido no LicenseKey.</summary>
    public sealed record ClientLicenseRequestResult(
        bool Succeeded,
        int StatusCode,
        string? Message,
        ClientLicenseRequestDto? Request);
}
