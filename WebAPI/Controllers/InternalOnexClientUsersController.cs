
using Application.UseCases.onexuser.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("internal/onex-client/users")]
[Authorize(AuthenticationSchemes = "InternalApiKey")]
public sealed class InternalOnexClientUsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<InternalOnexClientUsersController> _logger;

    public InternalOnexClientUsersController(
        IMediator mediator,
        ILogger<InternalOnexClientUsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFromTenantResponse(
        [FromBody] CreateOnexClientUserRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "O pedido não pode ser nulo."
            });
        }

        _logger.LogInformation(
            "Pedido de registo recebido para {Email}",
            request.AdminEmail);

        var result = await _mediator.Send(
            request,
            cancellationToken);

        if (!result.Succeeded)
            return Conflict(result);

        return Ok(result);
    }
}
