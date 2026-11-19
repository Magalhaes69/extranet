using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPI.Controllers;

/// <summary>
/// Controller para o Dashboard do Cliente
/// Fornece resumo geral das licenças, solicitações e pagamentos
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "process:dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IMediator mediator, ILogger<DashboardController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtém o resumo do dashboard do cliente autenticado
    /// </summary>
    /// <returns>Dados do dashboard com métricas de licenças, solicitações e pagamentos</returns>
    [HttpGet]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetClientDashboardQuery e Handler
            // var result = await _mediator.Send(
            //     new GetClientDashboardQuery(cardCode),
            //     cancellationToken);

            // Por enquanto, retorna dados de exemplo
            return Ok(new
            {
                totalLicenses = 0,
                activeLicenses = 0,
                expiredLicenses = 0,
                expiringLicenses = 0,
                pendingRequests = 0,
                approvedRequests = 0,
                totalRequests = 0,
                totalPayments = 0,
                pendingPayments = 0,
                totalPaid = 0m,
                totalPending = 0m,
                unreadNotifications = 0,
                companyName = "",
                cardCode = cardCode,
                isTrial = false,
                trialEndDate = (DateTime?)null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dashboard do cliente");
            return StatusCode(500, new { message = "Erro ao carregar dashboard" });
        }
    }
}
