namespace WebFrontEnd.Services.Dashboard.Dtos
{
    public sealed class ClientDashboardDto
    {
        // Informações da Empresa
        public string CompanyName { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;
        public bool IsTrial { get; set; }
        public DateTime? TrialEndDate { get; set; }

        // Licenças
        public int TotalLicenses { get; set; }
        public int ActiveLicenses { get; set; }
        public int ExpiredLicenses { get; set; }
        public int ExpiringLicenses { get; set; }

        // Solicitações
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }

        // Pagamentos
        public int TotalPayments { get; set; }
        public int PendingPayments { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalPending { get; set; }

        // Notificações
        public int UnreadNotifications { get; set; }
    }
}
