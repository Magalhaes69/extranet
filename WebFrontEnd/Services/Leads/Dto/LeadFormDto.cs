namespace WebFrontEnd.Services.Leads.Dto
{
    /// <summary>
    /// Data typed by the visitor in the "create account" form.
    /// The trial dates and the status are not entered by the visitor: <see cref="LeadsService"/> sets them.
    /// </summary>
    public class LeadFormDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Nuit { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
