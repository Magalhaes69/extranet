
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public sealed class LeadApiClient : ILeadApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public LeadApiClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> CreateLeadAndGetCodeAsync(
        string companyName,
        string taxNumber,
        string contactName,
        string adminEmail,
        string? phoneNumber,
        string adminInitialPassword,
        DateTime? trialStartDate,
        DateTime? trialEndDate,
        int status,
        string? notes,
        CancellationToken cancellationToken)
    {
        var apiKey = _configuration["LicensingApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException(
                "LicensingApi:ApiKey não está configurada.");

        var payload = new
        {
            CompanyName = companyName.Trim(),
            NUIT = taxNumber.Trim(),
            ContactName = contactName.Trim(),
            Email = adminEmail.Trim(),
            PhoneNumber = phoneNumber?.Trim() ?? string.Empty,
            Password = adminInitialPassword,
            TrialStartDate = trialStartDate,
            TrialEndDate = trialEndDate,
            Status = status,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()
        };

        using var message = new HttpRequestMessage(
            HttpMethod.Post, "api/Leads");

        message.Headers.TryAddWithoutValidation(
            "X-Api-Key", apiKey.Trim());

        message.Content = JsonContent.Create(payload);

        using var response = await _httpClient.SendAsync(
            message, cancellationToken);

        var body = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"Erro ao criar Lead no LicenseKey. HTTP {(int)response.StatusCode}: {body}",
                null,
                response.StatusCode);

        if (string.IsNullOrWhiteSpace(body))
            throw new InvalidOperationException(
                "O LicenseKey devolveu uma resposta vazia.");

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        var succeeded = FindProperty(root, "Succeeded");

        if (succeeded.HasValue &&
            succeeded.Value.ValueKind == JsonValueKind.False)
        {
            var error = GetText(FindProperty(root, "Message"))
                        ?? "O PROJ1 não conseguiu criar o Lead.";

            throw new InvalidOperationException(error);
        }

        var code = GetText(FindProperty(root, "Code"))?.Trim();

        if (string.IsNullOrWhiteSpace(code))
            throw new InvalidOperationException(
                $"O PROJ1 não devolveu o código do Lead. Resposta: {body}");

        if (code.Length > 20)
            throw new InvalidOperationException(
                "O código do Lead excede 20 caracteres.");

        return code;
    }

    private static JsonElement? FindProperty(
        JsonElement element,
        string name)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(
                    property.Name, name,
                    StringComparison.OrdinalIgnoreCase))
                    return property.Value;
            }

            foreach (var property in element.EnumerateObject())
            {
                var result = FindProperty(property.Value, name);
                if (result.HasValue)
                    return result;
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                var result = FindProperty(item, name);
                if (result.HasValue)
                    return result;
            }
        }

        return null;
    }

    private static string? GetText(JsonElement? element)
    {
        if (!element.HasValue ||
            element.Value.ValueKind == JsonValueKind.Null)
            return null;

        return element.Value.ValueKind == JsonValueKind.String
            ? element.Value.GetString()
            : element.Value.ToString();
    }
}
