using MediatR;

namespace Application.UseCases.onexuser.Create;

public sealed class CreateOnexClientUserRequest
    : IRequest<CreateOnexClientUserResponse>
{
    public string CompanyName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AdminInitialPassword { get; set; } = string.Empty;

    public DateTime? TrialStartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }

    public int Status { get; set; }
    public string? Notes { get; set; }
}