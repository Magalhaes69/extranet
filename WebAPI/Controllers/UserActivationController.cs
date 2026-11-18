
using Application.Shared.Emails;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace WebAPI.Controllers;

[ApiController]
[Route("account")]
public sealed class UserActivationController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserActivationController> _logger;

    private const int ActivationExpiryHours = 24;

    public UserActivationController(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IConfiguration configuration,
        ILogger<UserActivationController> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    // =====================================================
    // ACTIVAR CONTA
    // POST /account/activate
    // =====================================================

    [AllowAnonymous]
    [HttpPost("activate")]
    public async Task<IActionResult> Activate(
        [FromBody] ActivateAccountRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId) ||
            string.IsNullOrWhiteSpace(request.Token))
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "Dados de activação inválidos."
            });
        }

        // Procurar utilizador
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "Utilizador não encontrado."
            });
        }

        // Verificar se já está activado
        if (user.EmailConfirmed)
        {
            return Ok(new
            {
                succeeded = true,
                message = "A sua conta já está activada."
            });
        }

        // Descodificar token
        string decodedToken;

        try
        {
            decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(request.Token.Trim()));
        }
        catch (FormatException)
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "Token de activação inválido."
            });
        }

        // Confirmar email
        var result = await _userManager.ConfirmEmailAsync(
            user,
            decodedToken);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "O link de activação é inválido ou expirou.",
                errors = result.Errors.Select(x => x.Description)
            });
        }

        _logger.LogInformation(
            "Conta do utilizador {UserId} activada com sucesso.",
            user.Id);

        return Ok(new
        {
            succeeded = true,
            message = "A sua conta foi activada com sucesso."
        });
    }

    // =====================================================
    // REENVIAR EMAIL DE ACTIVACAO
    // POST /account/resend-activation
    // =====================================================

    [AllowAnonymous]
    [HttpPost("resend-activation")]
    public async Task<IActionResult> ResendActivation(
        [FromBody] ResendActivationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "O endereço de email é obrigatório."
            });
        }

        var email = request.Email.Trim();

        // Resposta genérica para proteger a existência das contas
        var genericResponse = new
        {
            succeeded = true,
            message = "Se existir uma conta pendente associada a este endereço, será enviado um novo email de activação."
        };

        try
        {
            // Procurar utilizador
            var user = await _userManager.FindByEmailAsync(email);

            // Não enviar se não existir ou já estiver activado
            if (user is null || user.EmailConfirmed)
                return Ok(genericResponse);

            // Obter URL do frontend
            var frontendUrl = _configuration["AppSettings:FrontendUrl"];

            if (string.IsNullOrWhiteSpace(frontendUrl) ||
                !Uri.TryCreate(frontendUrl, UriKind.Absolute, out var frontendUri) ||
                (frontendUri.Scheme != Uri.UriSchemeHttps &&
                 frontendUri.Scheme != Uri.UriSchemeHttp))
            {
                _logger.LogError(
                    "AppSettings:FrontendUrl não está configurado correctamente.");

                return StatusCode(500, new
                {
                    succeeded = false,
                    message = "Configuração do endereço do frontend inválida."
                });
            }

            // Gerar novo token Identity
            var confirmationToken =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Codificar token
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(confirmationToken));

            // Construir link de activação
            var activationUrl = QueryHelpers.AddQueryString(
                $"{frontendUrl.TrimEnd('/')}/activate-account",
                new Dictionary<string, string?>
                {
                    ["userId"] = user.Id,
                    ["token"] = encodedToken
                });

            // Utilizar template partilhado
            var emailBody = ActivationEmailBuilder.Build(
                user.FirstName ?? "Utilizador",
                "OnexClient",
                activationUrl,
                ActivationExpiryHours,
                "Recebemos um pedido para reenviar o link de activação da sua conta. " +
                "Clique no botão abaixo para confirmar o seu endereço de email.");

            // Enviar email
            await _emailService.SendAsync(
                user.Email!,
                "Activação da sua conta OnexClient",
                emailBody);

            _logger.LogInformation(
                "Email de activação reenviado para o utilizador {UserId}.",
                user.Id);

            return Ok(genericResponse);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao reenviar email de activação para {Email}.",
                email);

            return StatusCode(500, new
            {
                succeeded = false,
                message = "Não foi possível enviar o email de activação. Tente novamente."
            });
        }
    }
}

// =====================================================
// DTOs
// =====================================================

public sealed class ActivateAccountRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public sealed class ResendActivationRequest
{
    public string Email { get; set; } = string.Empty;
}
