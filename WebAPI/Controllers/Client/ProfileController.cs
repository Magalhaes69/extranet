using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Controller para gestão do perfil do cliente
/// Permite consultar e atualizar dados cadastrais (exceto dados fiscais)
/// </summary>
[ApiController]
[Route("api/client/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IMediator mediator,
        UserManager<AppUser> userManager,
        ILogger<ProfileController> logger)
    {
        _mediator = mediator;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Obtém o perfil completo do cliente
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Utilizador não encontrado" });

            // TODO: Buscar dados adicionais (trial, etc)
            var profile = new ClientProfileDto
            {
                CompanyName = user.FirstName ?? "",
                RepresentativeName = $"{user.FirstName} {user.LastName}".Trim(),
                Address = "",
                PhoneNumber = user.PhoneNumber ?? "",
                Email = user.Email ?? "",
                Nuit = user.Nuit ?? "",
                FiscalDesignation = user.FirstName ?? "",
                CardCode = user.CardCode ?? "",
                CreatedAt = user.DateCreated.DateTime,
                IsTrial = false
            };

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter perfil");
            return StatusCode(500, new { message = "Erro ao carregar perfil" });
        }
    }

    /// <summary>
    /// Atualiza dados editáveis do perfil
    /// NÃO permite alteração de dados fiscais (NUIT, designação fiscal)
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateClientProfileDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Utilizador não encontrado" });

            // Atualizar apenas dados permitidos
            user.FirstName = request.CompanyName;
            user.PhoneNumber = request.PhoneNumber;
            // TODO: Adicionar campos adicionais quando expandir AppUser

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    flag = false,
                    message = "Erro ao atualizar perfil"
                });
            }

            return Ok(new
            {
                flag = true,
                message = "Perfil atualizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar perfil");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao atualizar perfil"
            });
        }
    }

    /// <summary>
    /// Altera a password do utilizador
    /// </summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new
                {
                    flag = false,
                    message = "As passwords não coincidem"
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Utilizador não encontrado" });

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    flag = false,
                    message = "Password atual incorreta ou nova password inválida"
                });
            }

            return Ok(new
            {
                flag = true,
                message = "Password alterada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao alterar password");
            return StatusCode(500, new
            {
                flag = false,
                message = "Erro ao alterar password"
            });
        }
    }
}
