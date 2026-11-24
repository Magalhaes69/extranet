using Domain.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.UseCases.onexuser.GetClientLicensesByCardCode;

public sealed class GetClientLicensesHandler :
    IRequestHandler<GetClientLicensesQuery, ClientLicenseOverviewDto?>
{
    private readonly IClientLicenseApiClient _licenseApiClient;

    public GetClientLicensesHandler(
        IClientLicenseApiClient licenseApiClient)
    {
        _licenseApiClient = licenseApiClient;
    }

    public async Task<ClientLicenseOverviewDto?> Handle(
        GetClientLicensesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CardCode))
            return null;

        // Consultar a API LicenseKey.
        // A resposta já deverá incluir os preços dos módulos.
        return await _licenseApiClient.GetByCardCodeAsync(
            request.CardCode.Trim(),
            cancellationToken);
    }
}