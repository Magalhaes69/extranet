using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Dtos;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Controller para gestão de solicitações de licenças
/// Permite criar, consultar e acompanhar solicitações
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RequestsController> _logger;

    public RequestsController(IMediator mediator, ILogger<RequestsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as solicitações do cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetClientRequestsQuery
            return Ok(new List<LicenseRequestDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar solicitações");
            return StatusCode(500, new { message = "Erro ao carregar solicitações" });
        }
    }

    /// <summary>
    /// Obtém detalhes de uma solicitação específica
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetRequestByIdQuery
            return NotFound(new { message = "Solicitação não encontrada" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter solicitação");
            return StatusCode(500, new { message = "Erro ao carregar solicitação" });
        }
    }

    /// <summary>
    /// Cria nova solicitação de licença
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLicenseRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(cardCode) || string.IsNullOrWhiteSpace(userId))
                return Forbid();

            // TODO: Implementar CreateLicenseRequestCommand
            return Ok(new
            {
                flag = true,
                message = "Solicitação criada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar solicitação");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao criar solicitação"
            });
        }
    }

    /// <summary>
    /// Obtém histórico de solicitações
    /// </summary>
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(CancellationToken cancellationToken)
    {
        try
        {
            var cardCode = User.FindFirst("CardCode")?.Value;
            if (string.IsNullOrWhiteSpace(cardCode))
                return Forbid();

            // TODO: Implementar GetRequestHistoryQuery
            return Ok(new List<LicenseRequestHistoryDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter histórico");
            return StatusCode(500, new { message = "Erro ao carregar histórico" });
        }
    }
}
