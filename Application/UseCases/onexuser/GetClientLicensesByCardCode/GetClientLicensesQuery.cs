using Domain.Dtos;
using MediatR;

namespace Application.UseCases.onexuser.GetClientLicensesByCardCode;

public sealed record GetClientLicensesQuery(string CardCode)
    : IRequest<ClientLicenseOverviewDto?>;