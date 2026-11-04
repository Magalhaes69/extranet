
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.onexuser.Create;

public sealed class CreateOnexClientUserHandler
    : IRequestHandler<CreateOnexClientUserRequest,
        CreateOnexClientUserResponse>
{
    private readonly IBaseUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILeadApiClient _leadApiClient;

    public CreateOnexClientUserHandler(
        IBaseUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILeadApiClient leadApiClient)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _leadApiClient = leadApiClient;
    }

    public async Task<CreateOnexClientUserResponse> Handle(
        CreateOnexClientUserRequest request,
        CancellationToken cancellationToken)
    {
        // 1. Normalizar os dados.
        var email = request.AdminEmail?.Trim() ?? string.Empty;
        var taxNumber = request.TaxNumber?.Trim() ?? string.Empty;
        var contactName = request.ContactName?.Trim() ?? string.Empty;
        var companyName = request.CompanyName?.Trim() ?? string.Empty;
        var password = request.AdminInitialPassword;

        // 2. Validar os campos obrigatórios.
        var missingFields = new List<string>();

        if (string.IsNullOrWhiteSpace(email))
            missingFields.Add(nameof(request.AdminEmail));

        if (string.IsNullOrWhiteSpace(password))
            missingFields.Add(nameof(request.AdminInitialPassword));

        if (string.IsNullOrWhiteSpace(taxNumber))
            missingFields.Add(nameof(request.TaxNumber));

        if (string.IsNullOrWhiteSpace(contactName))
            missingFields.Add(nameof(request.ContactName));

        if (string.IsNullOrWhiteSpace(companyName))
            missingFields.Add(nameof(request.CompanyName));

        if (missingFields.Count > 0)
        {
            return Failure(
                email,
                string.Empty,
                $"Campos obrigatórios em falta: {string.Join(", ", missingFields)}.");
        }

        if (email.Length > 256)
            return Failure(email, string.Empty,
                "O email excede o limite de 256 caracteres.");

        if (taxNumber.Length > 10)
            return Failure(email, string.Empty,
                "O NUIT excede o limite de 10 caracteres.");

        // 3. Verificar se o utilizador já existe.
        // Esta verificação deve acontecer ANTES de criar outro Lead.
        var existing = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (existing is not null)
        {
            if (string.Equals(
                existing.Nuit,
                taxNumber,
                StringComparison.OrdinalIgnoreCase))
            {
                return new CreateOnexClientUserResponse
                {
                    Succeeded = true,
                    Created = false,
                    UserId = existing.Id,
                    Email = email,
                    CardCode = existing.CardCode,
                    Message = "O utilizador já está registado."
                };
            }

            return Failure(
                email,
                existing.CardCode ?? string.Empty,
                "O email já está associado a outra empresa.");
        }

        // 4. Chamar a API CreateLead do PROJ1.
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
                request.TrialStartDate,
                request.TrialEndDate,
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
            return Failure(
                email,
                string.Empty,
                $"Não foi possível criar o Lead no PROJ1: {ex.Message}");
        }

        // 5. Validar o código devolvido pelo PROJ1.
        if (string.IsNullOrWhiteSpace(cardCode))
        {
            return Failure(
                email,
                string.Empty,
                "O PROJ1 não devolveu o código do Lead.");
        }

        cardCode = cardCode.Trim();

        if (cardCode.Length > 20)
        {
            return Failure(
                email,
                cardCode,
                "O código do Lead excede o limite de 20 caracteres.");
        }

        // 6. Separar o nome do administrador.
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

        // 7. Criar o utilizador com o código devolvido pelo PROJ1.
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,

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

        // 8. Gravar na OnexClientDB.
        IdentityResult createResult =
            await _userRepository.CreateAsync(user, password);

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

        // 9. Commit.
        await _unitOfWork.Commit(cancellationToken);

        // 10. Resultado.
        return new CreateOnexClientUserResponse
        {
            Succeeded = true,
            Created = true,
            UserId = user.Id,
            Email = email,
            CardCode = cardCode,
            Message = "Utilizador registado na OnexClientDB com sucesso."
        };
    }

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
