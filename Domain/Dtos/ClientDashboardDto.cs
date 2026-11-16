namespace Domain.Dtos
{
    /// <summary>
    /// DTO para dashboard do cliente com resumo de todas as informações
    /// </summary>
    public class ClientDashboardDto
    {
        // Licenças
        public int TotalLicenses { get; set; }
        public int ActiveLicenses { get; set; }
        public int ExpiredLicenses { get; set; }
        public int ExpiringLicenses { get; set; }

        // Solicitações
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int TotalRequests { get; set; }

        // Pagamentos
        public int TotalPayments { get; set; }
        public int PendingPayments { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalPending { get; set; }

        // Notificações
        public int UnreadNotifications { get; set; }

        // Informações do cliente
        public string CompanyName { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;
        public bool IsTrial { get; set; }
        public DateTime? TrialEndDate { get; set; }
    }
}
