namespace Domain.Dtos
{
public sealed class ClientLicenseOverviewDto
{
    public Guid LeadId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string NUIT { get; set; } = string.Empty;

    public bool IsTrial { get; set; }
    public DateTime? TrialStartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }

    public List<ClientLicenseDto> Licenses { get; set; } = new();
}
}