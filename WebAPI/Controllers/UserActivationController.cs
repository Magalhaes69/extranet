
using System.Text;
using System.Text.Encodings.Web;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Controllers;

[ApiController]
[Route("account")]
public sealed class UserActivationController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    private const int ActivationExpiryHours = 24;

    public UserActivationController(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
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

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return BadRequest(new
            {
                succeeded = false,
                message = "Utilizador não encontrado."
            });
        }

        if (user.EmailConfirmed)
        {
            return Ok(new
            {
                succeeded = true,
                message = "A sua conta já está activada."
            });
        }

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

        var genericResponse = new
        {
            succeeded = true,
            message = "Se existir uma conta pendente associada a este endereço, será enviado um novo email de activação."
        };

        // Procurar utilizador
        var user = await _userManager.FindByEmailAsync(email);

        // Não revelar se o email existe ou se a conta está activa
        if (user is null || user.EmailConfirmed)
            return Ok(genericResponse);

        // Obter URL do frontend
        var frontendUrl = _configuration["AppSettings:FrontendUrl"];

        if (!Uri.TryCreate(frontendUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttps &&
             uri.Scheme != Uri.UriSchemeHttp))
        {
            return StatusCode(500, new
            {
                succeeded = false,
                message = "Configuração do endereço do frontend inválida."
            });
        }

        try
        {
            // Gerar novo token Identity
            var confirmationToken =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Codificar token para URL
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

            // Construir email
            var emailBody = BuildActivationEmail(
                user.FirstName ?? "Utilizador",
                "OnexClient",
                activationUrl,
                ActivationExpiryHours);

            // Enviar email
            await _emailService.SendAsync(
                user.Email!,
                "Activação da sua conta OnexClient",
                emailBody);

            return Ok(genericResponse);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                succeeded = false,
                message = "Não foi possível enviar o email de activação. Tente novamente."
            });
        }
    }

    // =====================================================
    // TEMPLATE HTML DE ACTIVACAO
    // =====================================================

    private static string BuildActivationEmail(
        string userName,
        string appName,
        string activationUrl,
        int expiryHours)
    {
        var sb = new StringBuilder();

        var safeName = HtmlEncoder.Default.Encode(userName);
        var safeApp = HtmlEncoder.Default.Encode(appName);
        var safeUrl = HtmlEncoder.Default.Encode(activationUrl);

        const string orange = "#fd7e14";
        const string black = "#263238";

        sb.Append("<!DOCTYPE html>");
        sb.Append("<html lang='pt'>");
        sb.Append("<head><meta charset='UTF-8'>");
        sb.Append("<meta name='viewport' content='width=device-width,initial-scale=1.0'>");
        sb.Append("</head>");

        sb.Append("<body style='margin:0;background:#f4f4f4;");
        sb.Append("font-family:Arial,sans-serif;padding:20px;'>");

        sb.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
        sb.Append("<tr><td align='center'>");

        sb.Append("<table width='600' cellpadding='0' cellspacing='0' ");
        sb.Append("style='max-width:600px;width:100%;background:#fff;'>");

        // HEADER - LARANJA
        sb.Append("<tr><td bgcolor='" + orange + "' ");
        sb.Append("style='background-color:" + orange + ";padding:22px 25px;color:#fff;'>");

        sb.Append($"<h2 style='margin:0;font-size:22px;'>{safeApp}</h2>");
        sb.Append("<p style='margin:5px 0 0;font-size:12px;'>Portal do Cliente</p>");

        sb.Append("</td></tr>");

        // CONTEÚDO
        sb.Append("<tr><td style='padding:35px 30px;'>");

        sb.Append("<h2 style='color:#263238;font-size:21px;'>");
        sb.Append("Confirmação da conta");
        sb.Append("</h2>");

        sb.Append($"<p style='font-size:14px;color:#555;'>Olá {safeName},</p>");

        sb.Append("<p style='font-size:14px;color:#555;line-height:23px;'>");
        sb.Append("Recebemos um pedido para enviar o link de activação da sua conta. ");
        sb.Append("Clique no botão abaixo para confirmar o seu endereço de email.");
        sb.Append("</p>");

        // BOTÃO COMPATÍVEL COM ZIMBRA
        sb.Append("<table role='presentation' align='center' ");
        sb.Append("border='0' cellpadding='0' cellspacing='0' ");
        sb.Append("style='margin:30px auto;'>");

        sb.Append("<tr>");
        sb.Append("<td align='center' bgcolor='" + orange + "' ");
        sb.Append("style='background-color:" + orange + ";'>");

        sb.Append($"<a href='{safeUrl}' target='_blank' ");
        sb.Append("style='display:inline-block;padding:15px 30px;");
        sb.Append("font-family:Arial,sans-serif;font-size:14px;");
        sb.Append("font-weight:bold;color:#ffffff;text-decoration:none;");
        sb.Append("background-color:" + orange + ";'>");

        sb.Append("CONFIRMAR A MINHA CONTA");

        sb.Append("</a>");
        sb.Append("</td></tr></table>");

        // VALIDADE
        sb.Append("<p style='font-size:13px;color:#999;'>");
        sb.Append($"Este link expirará em {expiryHours} horas.");
        sb.Append("</p>");

        // LINK ALTERNATIVO
        sb.Append("<p style='font-size:12px;color:#888;'>");
        sb.Append("Caso o botão não funcione, copie o link e cole-o no navegador:");
        sb.Append("</p>");

        sb.Append($"<p style='font-size:11px;color:{orange};word-break:break-all;'>");
        sb.Append(safeUrl);
        sb.Append("</p>");

        sb.Append("</td></tr>");

        // FOOTER - PRETO
        sb.Append("<tr><td bgcolor='" + black + "' ");
        sb.Append("style='background-color:" + black + ";padding:20px;");
        sb.Append("text-align:center;color:#fff;'>");

        sb.Append($"<p style='font-size:12px;margin:0 0 8px;'>");
        sb.Append($"© {DateTime.UtcNow.Year} {safeApp}. Todos os direitos reservados.");
        sb.Append("</p>");

        sb.Append("<p style='font-size:11px;color:#b0bec5;margin:0;'>");
        sb.Append("Este é um email automático. Por favor, não responda.");
        sb.Append("</p>");

        sb.Append("</td></tr>");

        sb.Append("</table></td></tr></table>");
        sb.Append("</body></html>");

        return sb.ToString();
    }
}

// =========================================================
// DTOs
// =========================================================

public sealed class ActivateAccountRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public sealed class ResendActivationRequest
{
    public string Email { get; set; } = string.Empty;
}
