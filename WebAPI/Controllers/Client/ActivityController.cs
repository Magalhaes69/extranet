using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Client;

/// <summary>Actividade do próprio utilizador autenticado (qualquer utilizador, sem processo específico).</summary>
[ApiController]
[Route("api/client/activity")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ActivityController : ControllerBase
{
    private const int MaxPageSize = 100;
    private readonly IUserActivityLogger _activity;

    public ActivityController(IUserActivityLogger activity)
    {
        _activity = activity;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return Forbid();

        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var (items, total) = await _activity.GetAsync(userId, page, pageSize, cancellationToken);

        return Ok(new
        {
            total,
            items = items.Select(a => new
            {
                id = a.Id,
                action = a.Action,
                details = a.Details,
                // Datas em UTC com Z para o browser converter para a hora local.
                createdAt = DateTime.SpecifyKind(a.CreatedAt, DateTimeKind.Utc)
            })
        });
    }
}
