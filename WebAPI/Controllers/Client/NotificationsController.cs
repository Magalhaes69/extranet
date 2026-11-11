using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Dtos;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Controller para gestão de notificações
/// Permite listar, marcar como lida e obter notificações não lidas
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(IMediator mediator, ILogger<NotificationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as notificações do cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? onlyUnread,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            // TODO: Implementar GetClientNotificationsQuery
            return Ok(new List<NotificationDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar notificações");
            return StatusCode(500, new { message = "Erro ao carregar notificações" });
        }
    }

    /// <summary>
    /// Obtém contador de notificações não lidas
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            // TODO: Implementar GetUnreadNotificationsCountQuery
            return Ok(new { count = 0 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contador de notificações");
            return StatusCode(500, new { message = "Erro ao carregar contador" });
        }
    }

    /// <summary>
    /// Marca notificação como lida
    /// </summary>
    [HttpPost("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            // TODO: Implementar MarkNotificationReadCommand
            return Ok(new { flag = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar notificação como lida");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao marcar notificação"
            });
        }
    }

    /// <summary>
    /// Marca todas as notificações como lidas
    /// </summary>
    [HttpPost("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            // TODO: Implementar MarkAllNotificationsReadCommand
            return Ok(new { flag = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar todas notificações como lidas");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao marcar notificações"
            });
        }
    }
}
