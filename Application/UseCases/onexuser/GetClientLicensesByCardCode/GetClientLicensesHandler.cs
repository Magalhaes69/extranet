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

        return await _licenseApiClient.GetByCardCodeAsync(
            request.CardCode,
            cancellationToken);
    }
}