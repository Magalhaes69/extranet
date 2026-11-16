using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Dtos;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Controller para gestão de pagamentos
/// Permite consultar pagamentos, download de recibos e histórico financeiro
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os pagamentos do cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetClientPaymentsQuery
            return Ok(new List<PaymentDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pagamentos");
            return StatusCode(500, new { message = "Erro ao carregar pagamentos" });
        }
    }

    /// <summary>
    /// Obtém histórico financeiro completo
    /// </summary>
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetFinancialHistoryQuery
            return Ok(new FinancialHistoryDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter histórico financeiro");
            return StatusCode(500, new { message = "Erro ao carregar histórico" });
        }
    }

    /// <summary>
    /// Faz download do recibo de pagamento
    /// </summary>
    [HttpGet("{id}/receipt")]
    public async Task<IActionResult> DownloadReceipt(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar download de recibo
            return NotFound(new { message = "Recibo não disponível" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer download do recibo");
            return StatusCode(500, new { message = "Erro ao fazer download" });
        }
    }

    /// <summary>
    /// Faz download da factura
    /// </summary>
    [HttpGet("{id}/invoice")]
    public async Task<IActionResult> DownloadInvoice(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar download de factura
            return NotFound(new { message = "Factura não disponível" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer download da factura");
            return StatusCode(500, new { message = "Erro ao fazer download" });
        }
    }
}
