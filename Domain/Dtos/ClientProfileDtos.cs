namespace Domain.Dtos
{
    /// <summary>
    /// DTO para perfil do cliente (dados editáveis)
    /// </summary>
    public class ClientProfileDto
    {
        // Dados editáveis
        public string CompanyName { get; set; } = string.Empty;
        public string RepresentativeName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AlternativePhone { get; set; }
        public string? AlternativeEmail { get; set; }

        // Dados fiscais (readonly)
        public string Nuit { get; set; } = string.Empty;
        public string FiscalDesignation { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;

        // Informações de conta
        public DateTime CreatedAt { get; set; }
        public bool IsTrial { get; set; }
        public DateTime? TrialEndDate { get; set; }
    }

    /// <summary>
    /// DTO para atualização do perfil (apenas dados editáveis)
    /// </summary>
    public class UpdateClientProfileDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string RepresentativeName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AlternativePhone { get; set; }
        public string? AlternativeEmail { get; set; }
    }

    /// <summary>
    /// DTO para alteração de password
    /// </summary>
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
