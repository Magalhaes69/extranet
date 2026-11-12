
using System.Text;
using System.Text.Encodings.Web;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.onexuser.Create;

public sealed class CreateOnexClientUserHandler
    : IRequestHandler<CreateOnexClientUserRequest, CreateOnexClientUserResponse>
{
    private const int ActivationExpiryHours = 24;
   private readonly IBaseUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILeadApiClient _leadApiClient;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public CreateOnexClientUserHandler(
        IBaseUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILeadApiClient leadApiClient,
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _leadApiClient = leadApiClient;
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<CreateOnexClientUserResponse> Handle(
        CreateOnexClientUserRequest request,
        CancellationToken cancellationToken)
    {
        // 1. Normalizar dados
        var email = request.AdminEmail?.Trim() ?? string.Empty;
        var taxNumber = request.TaxNumber?.Trim() ?? string.Empty;
        var contactName = request.ContactName?.Trim() ?? string.Empty;
        var companyName = request.CompanyName?.Trim() ?? string.Empty;
        var password = request.AdminInitialPassword;

        // 2. Validar campos obrigatórios
        var missingFields = new List<string>();

        if (string.IsNullOrWhiteSpace(email)) missingFields.Add(nameof(request.AdminEmail));

        if (string.IsNullOrWhiteSpace(password)) missingFields.Add(nameof(request.AdminInitialPassword));

        if (string.IsNullOrWhiteSpace(taxNumber)) missingFields.Add(nameof(request.TaxNumber));

        if (string.IsNullOrWhiteSpace(contactName)) missingFields.Add(nameof(request.ContactName));

        if (string.IsNullOrWhiteSpace(companyName))   missingFields.Add(nameof(request.CompanyName));

        if (missingFields.Count > 0)
        {
            return Failure(email, string.Empty, $"Campos obrigatórios em falta: {string.Join(", ", missingFields)}.");
        }

        if (email.Length > 256)
            return Failure(email, string.Empty,
                "O email excede o limite de 256 caracteres.");

        if (taxNumber.Length > 10)
            return Failure(email, string.Empty,
                "O NUIT excede o limite de 10 caracteres.");

        // 3. Obter URL do frontend
        var frontendUrl = _configuration["AppSettings:FrontendUrl"];

        if (!Uri.TryCreate(frontendUrl, UriKind.Absolute, out var frontendUri) ||
            (frontendUri.Scheme != Uri.UriSchemeHttps &&
             frontendUri.Scheme != Uri.UriSchemeHttp))
        {
            return Failure(email, string.Empty,
                "FrontendUrl não está configurado correctamente.");
        }

        // 4. Verificar se o utilizador já existe
        var existing = await _userRepository.GetByEmailAsync(
            email, cancellationToken);

        if (existing is not null)
        {
            return new CreateOnexClientUserResponse
            {
                Succeeded = true,
                Created = false,
                UserId = existing.Id,
                Email = email,
                CardCode = existing.CardCode,
                Message = existing.EmailConfirmed
                    ? "O utilizador já está registado e activo."
                    : "O utilizador já está registado, mas ainda não confirmou o email."
            };
        }

        // 5. Obter o código do Lead.
        // Quem já criou o Lead manda o código no pedido e este passo é saltado — é o
        // caso do LicenseKey, que chama aqui a seguir a gravá-lo. Criar outro Lead
        // nesse caso poria as duas aplicações a chamarem-se uma à outra sem fim.
        string cardCode;

        if (!string.IsNullOrWhiteSpace(request.CardCode))
        {
            cardCode = request.CardCode;
        }
        else
        {
            try
            {
                cardCode = await _leadApiClient.CreateLeadAndGetCodeAsync(
                    companyName,
                    taxNumber,
                    contactName,
                    email,
                    request.PhoneNumber,
                    password,
                    request.Status,
                    request.Notes,
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Failure(email, string.Empty,
                    $"Não foi possível criar o Lead no PROJ1: {ex.Message}");
            }
        }

        if (string.IsNullOrWhiteSpace(cardCode))
            return Failure(email, string.Empty,
                "O PROJ1 não devolveu o código do Lead.");

        cardCode = cardCode.Trim();

        if (cardCode.Length > 20)
            return Failure(email, cardCode,
                "O código do Lead excede o limite de 20 caracteres.");

        // 6. Separar nome e apelido
        var nameParts = contactName.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        var firstName = nameParts.Length > 0
            ? nameParts[0]
            : email.Split('@')[0];

        var lastName = nameParts.Length > 1
            ? string.Join(" ", nameParts.Skip(1))
            : "Administrador";

        // 7. Criar utilizador pendente de confirmação
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,

            FirstName = firstName,
            LastName = lastName,
            Nuit = taxNumber,
            CardCode = cardCode,

            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
                ? null
                : request.PhoneNumber.Trim(),

            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow,
            TwoFactorEnabled = false
        };

        // 8. Gravar utilizador na OnexClientDB
        var createResult = await _userRepository.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            return new CreateOnexClientUserResponse
            {
                Succeeded = false,
                Created = false,
                Email = email,
                CardCode = cardCode,
                Message = "Não foi possível criar o utilizador na OnexClientDB.",
                Errors = createResult.Errors
                    .Select(error => error.Description)
                    .ToArray()
            };
        }

        // 9. Commit
        await _unitOfWork.Commit(cancellationToken);

        // 10. Gerar token e enviar email de confirmação
        try
        {
            // Gerar token original do Identity
            var confirmationToken =await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Codificar para utilização no URL
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));
            var activationUrl = QueryHelpers.AddQueryString(
                $"{frontendUrl.TrimEnd('/')}/activate-account",
                new Dictionary<string, string?>
                {
                    ["userId"] = user.Id,
                    ["token"] = encodedToken
                });

            var emailBody = BuildActivationEmail(user.FirstName ?? "Utilizador", "OnexClient",activationUrl, ActivationExpiryHours);
            await _emailService.SendAsync(email,"Activação da sua conta OnexClient",emailBody);

            return new CreateOnexClientUserResponse
            {
                Succeeded = true,
                Created = true,
                UserId = user.Id,
                Email = email,
                CardCode = cardCode,
                Message = "Utilizador criado com sucesso. Foi enviado um email para confirmar a conta.",
                Errors = Array.Empty<string>()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            return new CreateOnexClientUserResponse
            {
                Succeeded = true,
                Created = true,
                UserId = user.Id,
                Email = email,
                CardCode = cardCode,
                Message = "Utilizador criado, mas ocorreu um erro no envio do email de confirmação.",
                Errors = new[]
                {
                    "Não foi possível enviar o email de confirmação."
                }
            };
        }
    }

    // 11. Construir HTML do email
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

        sb.Append("<head>");
        sb.Append("<meta charset='UTF-8'>");
        sb.Append("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        sb.Append("<title>Confirmação da conta</title>");
        sb.Append("</head>");

        sb.Append("<body style='margin:0;background:#f4f4f4;");
        sb.Append("font-family:Arial,sans-serif;padding:20px;'>");

        sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
        sb.Append("<tr><td align='center'>");

        sb.Append("<table width='600' cellpadding='0' cellspacing='0' border='0' ");
        sb.Append("style='max-width:600px;width:100%;background:#ffffff;");
        sb.Append("border-radius:8px;overflow:hidden;'>");

        // CABEÇALHO - LARANJA
        sb.Append("<tr>");
        sb.Append("<td style='background:" + orange + ";padding:22px 25px;color:#ffffff;'>");

        sb.Append($"<h2 style='margin:0;font-size:22px;'>{safeApp}</h2>");

        sb.Append("<p style='margin:5px 0 0;font-size:12px;color:#ffffff;'>");
        sb.Append("Portal do Cliente");
        sb.Append("</p>");

        sb.Append("</td>");
        sb.Append("</tr>");

        // CONTEÚDO PRINCIPAL - BRANCO
        sb.Append("<tr>");
        sb.Append("<td style='padding:35px 30px;'>");

        sb.Append("<h2 style='color:#263238;font-size:21px;margin-top:0;'>");
        sb.Append("Confirmação da conta");
        sb.Append("</h2>");

        sb.Append($"<p style='font-size:14px;color:#555;'>Olá {safeName},</p>");

        sb.Append("<p style='font-size:14px;color:#555;line-height:23px;'>");
        sb.Append("A sua conta foi criada com sucesso. ");
        sb.Append("Para começar a utilizar a plataforma, confirme o seu endereço ");
        sb.Append("de email através do botão abaixo.");
        sb.Append("</p>");

        // BOTÃO DE CONFIRMAÇÃO
        // BOTÃO DE CONFIRMAÇÃO - COMPATÍVEL COM ZIMBRA
        sb.Append("<table role='presentation' align='center' ");
        sb.Append("border='0' cellpadding='0' cellspacing='0' ");
        sb.Append("style='margin:30px auto;'>");

        sb.Append("<tr>");

        sb.Append("<td align='center' bgcolor='#fd7e14' ");
        sb.Append("style='background-color:#fd7e14;padding:0;'>");

        sb.Append($"<a href='{safeUrl}' target='_blank' ");
        sb.Append("style='display:inline-block;padding:15px 30px;");
        sb.Append("font-family:Arial,sans-serif;font-size:14px;");
        sb.Append("font-weight:bold;color:#ffffff;text-decoration:none;");
        sb.Append("background-color:#fd7e14;border:1px solid #fd7e14;'>");

        sb.Append("CONFIRMAR A MINHA CONTA");

        sb.Append("</a>");

        sb.Append("</td>");
        sb.Append("</tr>");
        sb.Append("</table>");

        // VALIDADE DO LINK
        sb.Append("<p style='font-size:13px;color:#999;'>");
        sb.Append($"Este link expirará em {expiryHours} horas.");
        sb.Append("</p>");

        // LINK ALTERNATIVO
        sb.Append("<p style='font-size:12px;color:#888;line-height:20px;'>");
        sb.Append("Caso o botão não funcione, copie o link seguinte e cole-o ");
        sb.Append("no seu navegador:");
        sb.Append("</p>");

        sb.Append($"<p style='font-size:11px;color:{orange};word-break:break-all;'>");
        sb.Append(safeUrl);
        sb.Append("</p>");

        sb.Append("</td>");
        sb.Append("</tr>");

        // RODAPÉ - PRETO
        sb.Append("<tr>");
        sb.Append("<td style='background:" + black + ";padding:20px;");
        sb.Append("text-align:center;color:#ffffff;'>");

        sb.Append("<p style='font-size:12px;margin:0 0 8px;'>");
        sb.Append($"© {DateTime.UtcNow.Year} {safeApp}. Todos os direitos reservados.");
        sb.Append("</p>");

        sb.Append("<p style='font-size:11px;color:#b0bec5;margin:0;'>");
        sb.Append("Este é um email automático. Por favor, não responda.");
        sb.Append("</p>");

        sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append("</table>");

        sb.Append("</td></tr>");
        sb.Append("</table>");

        sb.Append("</body>");
        sb.Append("</html>");

        return sb.ToString();
    }

    // 12. Resposta de erro
    private static CreateOnexClientUserResponse Failure(
        string email,
        string cardCode,
        string message)
    {
        return new CreateOnexClientUserResponse
        {
            Succeeded = false,
            Created = false,
            Email = email,
            CardCode = cardCode,
            Message = message,
            Errors = new[] { message }
        };
    }
}


 