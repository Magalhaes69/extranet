using Domain.Dtos;

namespace Application.Interfaces
{
    public interface IClientLicenseApiClient
    {
        Task<ClientLicenseOverviewDto?> GetByCardCodeAsync(
            string cardCode,
            CancellationToken cancellationToken);

        /// <summary>Pedidos (novas licenças e renovações) do cliente; opcionalmente só de uma licença.</summary>
        Task<List<ClientLicenseRequestDto>> GetRequestsAsync(
            string cardCode,
            Guid? licenseId,
            CancellationToken cancellationToken);

        /// <summary>Regista um pedido no LicenseKey, que é quem o trata.</summary>
        Task<ClientLicenseRequestResult> CreateRequestAsync(
            string cardCode,
            CreateClientLicenseRequestDto request,
            CancellationToken cancellationToken);
    }
}
