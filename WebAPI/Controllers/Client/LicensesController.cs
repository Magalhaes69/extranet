using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Dtos;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Controller para gestão de licenças do cliente
/// Permite visualizar, requisitar e renovar licenças
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "process:licenses")]
public class LicensesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LicensesController> _logger;

    public LicensesController(IMediator mediator, ILogger<LicensesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as licenças do cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetClientLicensesQuery
            // var result = await _mediator.Send(
            //     new GetClientLicensesQuery(cardCode),
            //     cancellationToken);

            return Ok(new List<object>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar licenças");
            return StatusCode(500, new { message = "Erro ao carregar licenças" });
        }
    }

    /// <summary>
    /// Obtém detalhes de uma licença específica
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetLicenseByIdQuery
            return NotFound(new { message = "Licença não encontrada" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter detalhes da licença");
            return StatusCode(500, new { message = "Erro ao carregar licença" });
        }
    }

    /// <summary>
    /// Requisita uma nova licença
    /// </summary>
    [HttpPost("request")]
    public async Task<IActionResult> RequestLicense(
        [FromBody] CreateLicenseRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(cardCode) || string.IsNullOrWhiteSpace(userId))
                return Forbid();

            // TODO: Implementar RequestLicenseCommand
            // var result = await _mediator.Send(
            //     new RequestLicenseCommand(userId, cardCode, request),
            //     cancellationToken);

            return Ok(new
            {
                flag = true,
                message = "Solicitação enviada com sucesso. Aguarde análise da equipa."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao requisitar licença");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao processar solicitação"
            });
        }
    }

    /// <summary>
    /// Renova uma licença existente
    /// </summary>
    [HttpPost("{id}/renew")]
    public async Task<IActionResult> RenewLicense(
        string id,
        [FromBody] RenewLicenseDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar RenewLicenseCommand
            // var result = await _mediator.Send(
            //     new RenewLicenseCommand(id, cardCode, request),
            //     cancellationToken);

            return Ok(new
            {
                flag = true,
                message = "Pedido de renovação enviado com sucesso."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar licença");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao processar renovação"
            });
        }
    }

    /// <summary>
    /// Obtém histórico de uma licença
    /// </summary>
    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory(string id, CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetLicenseHistoryQuery
            return Ok(new List<object>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter histórico da licença");
            return StatusCode(500, new { message = "Erro ao carregar histórico" });
        }
    }

    /// <summary>
    /// Faz download do certificado de licença (quando disponível)
    /// </summary>
    [HttpGet("{id}/certificate")]
    public async Task<IActionResult> DownloadCertificate(string id, CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar download de certificado
            return NotFound(new { message = "Certificado não disponível" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer download do certificado");
            return StatusCode(500, new { message = "Erro ao fazer download" });
        }
    }
}

public class RenewLicenseDto
{
    public int Months { get; set; }
    public string? Notes { get; set; }
}
