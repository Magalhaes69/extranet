using Application.Interfaces;
using Domain.Dtos;
using MediatR;

namespace Application.UseCases.onexuser.ClientLicenseRequests;

public sealed record GetClientLicenseRequestsQuery(string CardCode, Guid? LicenseId)
    : IRequest<List<ClientLicenseRequestDto>>;

public sealed class GetClientLicenseRequestsHandler
    : IRequestHandler<GetClientLicenseRequestsQuery, List<ClientLicenseRequestDto>>
{
    private readonly IClientLicenseApiClient _api;

    public GetClientLicenseRequestsHandler(IClientLicenseApiClient api)
    {
        _api = api;
    }

    public Task<List<ClientLicenseRequestDto>> Handle(
        GetClientLicenseRequestsQuery request, CancellationToken cancellationToken) =>
        _api.GetRequestsAsync(request.CardCode, request.LicenseId, cancellationToken);
}

public sealed record CreateClientLicenseRequestCommand(string CardCode, CreateClientLicenseRequestDto Body)
    : IRequest<ClientLicenseRequestResult>;

public sealed class CreateClientLicenseRequestHandler
    : IRequestHandler<CreateClientLicenseRequestCommand, ClientLicenseRequestResult>
{
    private readonly IClientLicenseApiClient _api;

    public CreateClientLicenseRequestHandler(IClientLicenseApiClient api)
    {
        _api = api;
    }

    public Task<ClientLicenseRequestResult> Handle(
        CreateClientLicenseRequestCommand request, CancellationToken cancellationToken) =>
        _api.CreateRequestAsync(request.CardCode, request.Body, cancellationToken);
}
