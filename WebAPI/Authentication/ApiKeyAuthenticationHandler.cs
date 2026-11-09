
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace WebAPI.Authentication;

public sealed class ApiKeyAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeader = "X-Internal-Api-Key";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }


    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Logger.LogInformation(
            "API Key Handler executado. Path: {Path}",
            Request.Path);

        if (!Request.Headers.TryGetValue(ApiKeyHeader, out var suppliedKey))
        {
            Logger.LogWarning(
                "Cabeçalho {Header} não recebido.",
                ApiKeyHeader);

            return Task.FromResult(
                AuthenticateResult.Fail("API Key não fornecida."));
        }

        var expectedKey = _configuration["InternalApi:Key"];
        var receivedKey = suppliedKey.ToString();

        Logger.LogInformation(
            "InternalApi:Key configurada: {Configured}",
            !string.IsNullOrWhiteSpace(expectedKey));

        Logger.LogInformation(
            "X-Api-Key recebida: {Received}",
            !string.IsNullOrWhiteSpace(receivedKey));

        if (string.IsNullOrWhiteSpace(expectedKey) ||
            string.IsNullOrWhiteSpace(receivedKey))
        {
            return Task.FromResult(
                AuthenticateResult.Fail("API Key inválida."));
        }

        var expectedBytes = Encoding.UTF8.GetBytes(expectedKey);
        var receivedBytes = Encoding.UTF8.GetBytes(receivedKey);

        var isValid =
            expectedBytes.Length == receivedBytes.Length &&
            CryptographicOperations.FixedTimeEquals(
                expectedBytes,
                receivedBytes);

        Logger.LogInformation(
            "Resultado da validação da API Key: {IsValid}",
            isValid);

        if (!isValid)
        {
            return Task.FromResult(
                AuthenticateResult.Fail("API Key inválida."));
        }

        var identity = new ClaimsIdentity(
            new[]
            {
            new Claim(ClaimTypes.Name, "InternalApiClient")
            },
            Scheme.Name);

        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(
            principal,
            Scheme.Name);

        return Task.FromResult(
            AuthenticateResult.Success(ticket));
    }
}

