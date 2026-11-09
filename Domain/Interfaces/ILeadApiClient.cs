using Domain.Models;

namespace Domain.Interfaces;

public interface ILeadApiClient
{
    Task<string> CreateLeadAndGetCodeAsync(
        string companyName,
        string taxNumber,
        string contactName,
        string adminEmail,
        string? phoneNumber,
        string adminInitialPassword,
        int status,
        string? notes,
        CancellationToken cancellationToken);
}