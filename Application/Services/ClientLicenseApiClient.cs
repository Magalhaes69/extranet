
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
namespace Application.Services;
using Domain.Dtos;
using Application.Interfaces;
public sealed class ClientLicenseApiClient : IClientLicenseApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ClientLicenseApiClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ClientLicenseOverviewDto?> GetByCardCodeAsync(
        string cardCode,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cardCode))
            throw new ArgumentException("CardCode é obrigatório.");

        var url = $"api/client/licenses/{Uri.EscapeDataString(cardCode.Trim())}";

        using var message = CreateMessage(HttpMethod.Get, url);

        using var response = await _httpClient.SendAsync(
            message, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(
                cancellationToken);

            throw new HttpRequestException(
                $"Erro ao consultar licenças no LicenseKey. " +
                $"HTTP {(int)response.StatusCode}: {error}",
                null,
                response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<ClientLicenseOverviewDto>(
            cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException(
                "O LicenseKey devolveu uma resposta vazia.");
    }

    public async Task<List<ClientLicenseRequestDto>> GetRequestsAsync(
        string cardCode,
        Guid? licenseId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cardCode))
            throw new ArgumentException("CardCode é obrigatório.");

        var url = $"api/client/licenses/{Uri.EscapeDataString(cardCode.Trim())}/requests";
        if (licenseId.HasValue)
            url += $"?licenseId={licenseId.Value}";

        using var message = CreateMessage(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(message, cancellationToken);

        // Cliente ainda sem registo no LicenseKey: não há pedidos.
        if (response.StatusCode == HttpStatusCode.NotFound)
            return new List<ClientLicenseRequestDto>();

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Erro ao consultar pedidos no LicenseKey. HTTP {(int)response.StatusCode}: {error}",
                null,
                response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<List<ClientLicenseRequestDto>>(
                   cancellationToken: cancellationToken)
               ?? new List<ClientLicenseRequestDto>();
    }

    public async Task<ClientLicenseRequestResult> CreateRequestAsync(
        string cardCode,
        CreateClientLicenseRequestDto request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cardCode))
            throw new ArgumentException("CardCode é obrigatório.");

        var url = $"api/client/licenses/{Uri.EscapeDataString(cardCode.Trim())}/requests";

        using var message = CreateMessage(HttpMethod.Post, url);
        message.Content = JsonContent.Create(request);

        using var response = await _httpClient.SendAsync(message, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var created = JsonSerializer.Deserialize<ClientLicenseRequestDto>(
                body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new ClientLicenseRequestResult(true, (int)response.StatusCode, null, created);
        }

        // O LicenseKey devolve { "message": "..." } nos erros de validação (400/404/409).
        string? error = null;
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                doc.RootElement.TryGetProperty("message", out var m))
                error = m.GetString();
        }
        catch (JsonException)
        {
            // corpo sem JSON: usa a mensagem por omissão
        }

        return new ClientLicenseRequestResult(false, (int)response.StatusCode, error, null);
    }

    private HttpRequestMessage CreateMessage(HttpMethod method, string url)
    {
        var apiKey = _configuration["LicensingApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException(
                "LicensingApi:ApiKey não está configurada.");

        var message = new HttpRequestMessage(method, url);
        message.Headers.TryAddWithoutValidation("X-Api-Key", apiKey.Trim());
        return message;
    }
}
