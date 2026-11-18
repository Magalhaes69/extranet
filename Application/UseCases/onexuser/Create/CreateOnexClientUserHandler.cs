using System.Text;
using Application.Shared.Emails;
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

        if (string.IsNullOrWhiteSpace(companyName)) missingFields.Add(nameof(request.CompanyName));

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
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Codificar para utilização no URL
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));
            var activationUrl = QueryHelpers.AddQueryString(
                $"{frontendUrl.TrimEnd('/')}/activate-account",
                new Dictionary<string, string?>
                {
                    ["userId"] = user.Id,
                    ["token"] = encodedToken
                });

            var emailBody = ActivationEmailBuilder.Build(
                user.FirstName ?? "Utilizador",
                "OnexClient",
                activationUrl,
                ActivationExpiryHours,
                "A sua conta foi criada com sucesso. Clique no botão abaixo para confirmar o seu endereço de email.");
            await _emailService.SendAsync(
                email,
                "Activação da sua conta OnexClient",
                emailBody);

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

    // 11. Resposta de erro
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


