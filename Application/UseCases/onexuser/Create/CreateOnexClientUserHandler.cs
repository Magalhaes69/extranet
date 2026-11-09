
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

        // 5. Criar Lead no PROJ1
        string cardCode;

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
            var confirmationToken =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(confirmationToken));

            var activationUrl = QueryHelpers.AddQueryString(
                $"{frontendUrl!.TrimEnd('/')}/activate-account",
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

        sb.Append("<!DOCTYPE html>");
        sb.Append("<html><head><meta charset='UTF-8'></head>");

        sb.Append("<body style='margin:0;background:#f4f4f4;");
        sb.Append("font-family:Arial,sans-serif;padding:20px;'>");

        sb.Append("<table width='100%' cellpadding='0' cellspacing='0' ");
        sb.Append("style='max-width:600px;margin:auto;background:#fff;'>");

        // NAVBAR
        sb.Append("<tr><td colspan='2' style='background:#263238;");
        sb.Append("padding:20px;color:#fff;'>");

        sb.Append($"<h2 style='margin:0;'>{safeApp}</h2>");
        sb.Append("<small>Portal do Cliente</small>");
        sb.Append("</td></tr>");

        sb.Append("<tr>");

        // SIDEBAR
        sb.Append("<td width='150' valign='top' ");
        sb.Append("style='background:#f1f4f7;padding:15px;'>");

        sb.Append("<h3>Bem-vindo!</h3>");
        sb.Append("<p style='font-size:12px;color:#666;line-height:20px;'>");
        sb.Append("Confirme o seu email para activar a sua conta.");
        sb.Append("</p></td>");

        // CONTEÚDO
        sb.Append("<td valign='top' style='padding:25px;'>");

        sb.Append("<h2 style='color:#263238;'>Confirmação da conta</h2>");
        sb.Append($"<p>Olá {safeName},</p>");

        sb.Append("<p style='font-size:14px;color:#555;line-height:22px;'>");
        sb.Append("A sua conta foi criada com sucesso. ");
        sb.Append("Clique no botão abaixo para confirmar o seu endereço de email.");
        sb.Append("</p>");

        // BOTÃO
        sb.Append("<div style='text-align:center;margin:30px 0;'>");

        sb.Append($"<a href='{safeUrl}' style='background:#1976d2;color:#fff;");
        sb.Append("padding:14px 24px;text-decoration:none;border-radius:5px;");
        sb.Append("font-weight:bold;display:inline-block;'>");

        sb.Append("CONFIRMAR A MINHA CONTA");
        sb.Append("</a></div>");

        sb.Append($"<p style='font-size:13px;color:#999;'>");
        sb.Append($"Este link expirará em {expiryHours} horas.</p>");

        sb.Append("<p style='font-size:12px;color:#888;'>");
        sb.Append("Caso o botão não funcione, copie o link e cole-o no navegador:");
        sb.Append("</p>");

        sb.Append($"<p style='font-size:11px;color:#1976d2;word-break:break-all;'>{safeUrl}</p>");

        sb.Append("</td></tr>");

        // FOOTER
        sb.Append("<tr><td colspan='2' style='background:#263238;");
        sb.Append("color:#fff;padding:18px;text-align:center;'>");

        sb.Append($"<small>© {DateTime.UtcNow.Year} {safeApp}. Todos os direitos reservados.</small>");
        sb.Append("<p style='font-size:11px;color:#b0bec5;margin-bottom:0;'>");
        sb.Append("Este é um email automático. Por favor, não responda.");
        sb.Append("</p></td></tr>");

        sb.Append("</table></body></html>");

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




//using Domain.Interfaces;
//using Domain.Models;
//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace Application.UseCases.onexuser.Create;

//public sealed class CreateOnexClientUserHandler
//    : IRequestHandler<CreateOnexClientUserRequest,
//        CreateOnexClientUserResponse>
//{
//    private readonly IBaseUserRepository _userRepository;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly ILeadApiClient _leadApiClient;

//    public CreateOnexClientUserHandler(
//        IBaseUserRepository userRepository,
//        IUnitOfWork unitOfWork,
//        ILeadApiClient leadApiClient)
//    {
//        _userRepository = userRepository;
//        _unitOfWork = unitOfWork;
//        _leadApiClient = leadApiClient;
//    }

//    public async Task<CreateOnexClientUserResponse> Handle(
//        CreateOnexClientUserRequest request,
//        CancellationToken cancellationToken)
//    {
//        // 1. Normalizar os dados.
//        var email = request.AdminEmail?.Trim() ?? string.Empty;
//        var taxNumber = request.TaxNumber?.Trim() ?? string.Empty;
//        var contactName = request.ContactName?.Trim() ?? string.Empty;
//        var companyName = request.CompanyName?.Trim() ?? string.Empty;
//        var password = request.AdminInitialPassword;

//        // 2. Validar os campos obrigatórios.
//        var missingFields = new List<string>();

//        if (string.IsNullOrWhiteSpace(email))
//            missingFields.Add(nameof(request.AdminEmail));

//        if (string.IsNullOrWhiteSpace(password))
//            missingFields.Add(nameof(request.AdminInitialPassword));

//        if (string.IsNullOrWhiteSpace(taxNumber))
//            missingFields.Add(nameof(request.TaxNumber));

//        if (string.IsNullOrWhiteSpace(contactName))
//            missingFields.Add(nameof(request.ContactName));

//        if (string.IsNullOrWhiteSpace(companyName))
//            missingFields.Add(nameof(request.CompanyName));

//        if (missingFields.Count > 0)
//        {
//            return Failure(
//                email,
//                string.Empty,
//                $"Campos obrigatórios em falta: {string.Join(", ", missingFields)}.");
//        }

//        if (email.Length > 256)
//            return Failure(email, string.Empty,
//                "O email excede o limite de 256 caracteres.");

//        if (taxNumber.Length > 10)
//            return Failure(email, string.Empty,
//                "O NUIT excede o limite de 10 caracteres.");

//        // 3. Verificar se o utilizador já existe.
//        // Esta verificação deve acontecer ANTES de criar outro Lead.
//        var existing = await _userRepository.GetByEmailAsync(
//            email,
//            cancellationToken);

//        if (existing is not null)
//        {
//            if (string.Equals(
//                existing.Nuit,
//                taxNumber,
//                StringComparison.OrdinalIgnoreCase))
//            {
//                return new CreateOnexClientUserResponse
//                {
//                    Succeeded = true,
//                    Created = false,
//                    UserId = existing.Id,
//                    Email = email,
//                    CardCode = existing.CardCode,
//                    Message = "O utilizador já está registado."
//                };
//            }

//            return Failure(
//                email,
//                existing.CardCode ?? string.Empty,
//                "O email já está associado a outra empresa.");
//        }

//        // 4. Chamar a API CreateLead do PROJ1.
//        string cardCode;

//        try
//        {
//            cardCode = await _leadApiClient.CreateLeadAndGetCodeAsync(
//                companyName,
//                taxNumber,
//                contactName,
//                email,
//                request.PhoneNumber,
//                password,
//                request.Status,
//                request.Notes,
//                cancellationToken);
//        }
//        catch (OperationCanceledException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            return Failure(
//                email,
//                string.Empty,
//                $"Não foi possível criar o Lead no PROJ1: {ex.Message}");
//        }

//        // 5. Validar o código devolvido pelo PROJ1.
//        if (string.IsNullOrWhiteSpace(cardCode))
//        {
//            return Failure(
//                email,
//                string.Empty,
//                "O PROJ1 não devolveu o código do Lead.");
//        }

//        cardCode = cardCode.Trim();

//        if (cardCode.Length > 20)
//        {
//            return Failure(
//                email,
//                cardCode,
//                "O código do Lead excede o limite de 20 caracteres.");
//        }

//        // 6. Separar o nome do administrador.
//        var nameParts = contactName.Split(
//            ' ',
//            StringSplitOptions.RemoveEmptyEntries |
//            StringSplitOptions.TrimEntries);

//        var firstName = nameParts.Length > 0
//            ? nameParts[0]
//            : email.Split('@')[0];

//        var lastName = nameParts.Length > 1
//            ? string.Join(" ", nameParts.Skip(1))
//            : "Administrador";

//        // 7. Criar o utilizador com o código devolvido pelo PROJ1.
//        var user = new AppUser
//        {
//            UserName = email,
//            Email = email,
//            EmailConfirmed = true,

//            FirstName = firstName,
//            LastName = lastName,

//            Nuit = taxNumber,

//            CardCode = cardCode,

//            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
//                ? null
//                : request.PhoneNumber.Trim(),

//            DateCreated = DateTimeOffset.UtcNow,
//            DateUpdated = DateTimeOffset.UtcNow,

//            TwoFactorEnabled = false
//        };

//        // 8. Gravar na OnexClientDB.
//        IdentityResult createResult =
//            await _userRepository.CreateAsync(user, password);

//        if (!createResult.Succeeded)
//        {
//            return new CreateOnexClientUserResponse
//            {
//                Succeeded = false,
//                Created = false,
//                Email = email,
//                CardCode = cardCode,
//                Message = "Não foi possível criar o utilizador na OnexClientDB.",
//                Errors = createResult.Errors
//                    .Select(error => error.Description)
//                    .ToArray()
//            };
//        }

//        // 9. Commit.
//        await _unitOfWork.Commit(cancellationToken);

//        // 10. Resultado.
//        return new CreateOnexClientUserResponse
//        {
//            Succeeded = true,
//            Created = true,
//            UserId = user.Id,
//            Email = email,
//            CardCode = cardCode,
//            Message = "Utilizador registado na OnexClientDB com sucesso."
//        };
//    }

//    private static CreateOnexClientUserResponse Failure(
//        string email,
//        string cardCode,
//        string message)
//    {
//        return new CreateOnexClientUserResponse
//        {
//            Succeeded = false,
//            Created = false,
//            Email = email,
//            CardCode = cardCode,
//            Message = message,
//            Errors = new[] { message }
//        };
//    }

//}
