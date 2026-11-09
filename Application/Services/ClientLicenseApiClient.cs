
using System.Net;
using System.Net.Http.Json;
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

        var apiKey = _configuration["LicensingApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException(
                "LicensingApi:ApiKey não está configurada.");

        var url = $"api/client/licenses/{Uri.EscapeDataString(cardCode.Trim())}";

        using var message = new HttpRequestMessage(
            HttpMethod.Get, url);

        message.Headers.TryAddWithoutValidation(
            "X-Api-Key", apiKey.Trim());

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
}
