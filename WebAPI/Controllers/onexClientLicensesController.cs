using Application.UseCases.onexuser.GetClientLicensesByCardCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("client/licenses")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class OnexClientLicensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public OnexClientLicensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("MyLicenses")]
    public async Task<IActionResult> GetMyLicenses(
        CancellationToken cancellationToken)
    {
        var cardCode = User.FindFirst("CardCode")?.Value;

        if (string.IsNullOrWhiteSpace(cardCode))
            return Forbid();

        var result = await _mediator.Send(
            new GetClientLicensesQuery(cardCode),
            cancellationToken);

        if (result == null)
            return NotFound("Licenças não encontradas.");

        return Ok(result);
    }
}