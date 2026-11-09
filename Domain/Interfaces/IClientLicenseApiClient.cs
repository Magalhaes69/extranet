using Domain.Dtos;

namespace Application.Interfaces
{
    public interface IClientLicenseApiClient
    {
        Task<ClientLicenseOverviewDto?> GetByCardCodeAsync(
            string cardCode,
            CancellationToken cancellationToken);
    }
}