using Application.UseCases.onexuser.ClientLicenseRequests;
using Application.UseCases.onexuser.GetClientLicensesByCardCode;
using Domain.Dtos;
using Domain.Models;
using Domain.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Licenças do cliente. A consulta e os pedidos (nova licença / renovação) são feitos no
/// LicenseKey, que é quem trata as licenças. Consultar exige o processo "licenses";
/// requisitar e renovar exige também ser gestor da empresa.
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "process:licenses")]
public class LicensesController : ControllerBase
{
    private const int NewType = 1;
    private const int RenewalType = 2;

    private readonly IMediator _mediator;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<LicensesController> _logger;

    public LicensesController(
        IMediator mediator,
        UserManager<AppUser> userManager,
        ILogger<LicensesController> logger)
    {
        _mediator = mediator;
        _userManager = userManager;
        _logger = logger;
    }

    private string? CardCode => User.FindFirst("CardCode")?.Value;

    /// <summary>Licenças da empresa (com módulos) e estado do período de teste.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        try
        {
            var overview = await _mediator.Send(new GetClientLicensesQuery(CardCode), cancellationToken);

            // Cliente ainda sem registo no LicenseKey: lista vazia em vez de erro.
            return Ok(overview ?? new ClientLicenseOverviewDto { Code = CardCode });
        }
        catch (Exception ex)
        {
            return LicenseKeyUnavailable(ex, "Erro ao carregar licenças");
        }
    }

    /// <summary>Detalhe de uma licença.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        try
        {
            var overview = await _mediator.Send(new GetClientLicensesQuery(CardCode), cancellationToken);
            var license = overview?.Licenses.FirstOrDefault(l => l.Id == id);

            return license is null
                ? NotFound(new { message = "Licença não encontrada" })
                : Ok(license);
        }
        catch (Exception ex)
        {
            return LicenseKeyUnavailable(ex, "Erro ao carregar licença");
        }
    }

    /// <summary>Todos os pedidos (novas licenças e renovações) da empresa.</summary>
    [HttpGet("requests")]
    public async Task<IActionResult> GetRequests(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        try
        {
            return Ok(await _mediator.Send(new GetClientLicenseRequestsQuery(CardCode, null), cancellationToken));
        }
        catch (Exception ex)
        {
            return LicenseKeyUnavailable(ex, "Erro ao carregar pedidos");
        }
    }

    /// <summary>Histórico de pedidos (renovações) de uma licença.</summary>
    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        try
        {
            return Ok(await _mediator.Send(new GetClientLicenseRequestsQuery(CardCode, id), cancellationToken));
        }
        catch (Exception ex)
        {
            return LicenseKeyUnavailable(ex, "Erro ao carregar histórico");
        }
    }

    /// <summary>Requisita uma nova licença (só gestores). O pedido fica registado no LicenseKey.</summary>
    [HttpPost("request")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AppRoleNames.ManagerRoles)]
    public async Task<IActionResult> RequestLicense(
        [FromBody] NewLicenseRequestBody body,
        CancellationToken cancellationToken)
    {
        var (name, email) = await RequesterAsync();
        return await SubmitAsync(new CreateClientLicenseRequestDto
        {
            Type = NewType,
            ProductName = body.ProductName,
            Users = body.Users,
            Notes = body.Notes,
            RequestedByName = name,
            RequestedByEmail = email
        }, "Pedido de licença enviado com sucesso. A nossa equipa irá analisá-lo.", cancellationToken);
    }

    /// <summary>Pede a renovação de uma licença (só gestores). O pedido fica registado no LicenseKey.</summary>
    [HttpPost("{id:guid}/renew")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AppRoleNames.ManagerRoles)]
    public async Task<IActionResult> RenewLicense(
        Guid id,
        [FromBody] RenewLicenseDto body,
        CancellationToken cancellationToken)
    {
        var (name, email) = await RequesterAsync();
        return await SubmitAsync(new CreateClientLicenseRequestDto
        {
            Type = RenewalType,
            LicenseKeyId = id,
            Months = body.Months,
            Notes = body.Notes,
            RequestedByName = name,
            RequestedByEmail = email
        }, "Pedido de renovação enviado com sucesso. A nossa equipa irá analisá-lo.", cancellationToken);
    }

    /// <summary>Faz download do certificado de licença (quando disponível)</summary>
    [HttpGet("{id}/certificate")]
    public IActionResult DownloadCertificate(string id)
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        // TODO: Implementar download de certificado
        return NotFound(new { message = "Certificado não disponível" });
    }

    // ------------------------------------------------------------------ auxiliares

    private async Task<IActionResult> SubmitAsync(
        CreateClientLicenseRequestDto dto, string successMessage, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        try
        {
            var result = await _mediator.Send(new CreateClientLicenseRequestCommand(CardCode, dto), cancellationToken);

            if (result.Succeeded)
                return Ok(new { flag = true, message = successMessage, request = result.Request });

            // Erros de validação do LicenseKey (400/404/409) seguem para o utilizador tal como estão.
            var status = result.StatusCode is 400 or 404 or 409 ? result.StatusCode : 502;
            return StatusCode(status, new
            {
                flag = false,
                message = result.Message ?? "Não foi possível registar o pedido. Tente novamente."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registar pedido de licença no LicenseKey");
            return StatusCode(502, new
            {
                flag = false,
                message = "Não foi possível contactar o serviço de licenciamento. Tente novamente mais tarde."
            });
        }
    }

    private async Task<(string Name, string Email)> RequesterAsync()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirst("UserId")?.Value ?? string.Empty);
        var name = user is null ? "" : $"{user.FirstName} {user.LastName}".Trim();
        var email = user?.Email ?? "";
        return (name, email);
    }

    private IActionResult LicenseKeyUnavailable(Exception ex, string message)
    {
        _logger.LogError(ex, "{Message}", message);
        return StatusCode(502, new { message = "Não foi possível contactar o serviço de licenciamento." });
    }
}

public class NewLicenseRequestBody
{
    public string ProductName { get; set; } = string.Empty;
    public int Users { get; set; }
    public string? Notes { get; set; }
}

public class RenewLicenseDto
{
    public int Months { get; set; }
    public string? Notes { get; set; }
}
