using MediatR;

namespace Application.UseCases.onexuser.Create;

public sealed class CreateOnexClientUserRequest
    : IRequest<CreateOnexClientUserResponse>
{
    public string CompanyName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AdminInitialPassword { get; set; } = string.Empty;
    public int Status { get; set; }
    public string? Notes { get; set; }

    /// <summary>
    /// Código do Lead, quando quem chama já o criou — é o caso do LicenseKey, que
    /// chama este caso de uso logo a seguir a gravar o Lead. Vindo preenchido, o
    /// passo que cria o Lead é saltado e este código é usado como CardCode.
    /// Vazio, mantém-se o comportamento antigo: o Lead é criado aqui.
    /// Sem isto, o LicenseKey chamar este endereço criaria um Lead novo, que por
    /// sua vez voltaria a chamar aqui — um ciclo entre as duas aplicações.
    /// </summary>
    public string? CardCode { get; set; }
}