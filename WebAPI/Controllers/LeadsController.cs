using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using WebAPI.Services;
using WebAPI.Settings;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Public "create account" endpoint. It validates what the visitor typed, decides the trial dates and status
    /// itself and forwards the request to the leads API. The browser never talks to the leads API directly.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LeadsController : ControllerBase
    {
        public const string HttpClientName = "LeadsApi";
        public const string RateLimitPolicy = "leads";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly LeadsApiSettings _settings;
        private readonly ILogger<LeadsController> _logger;

        public LeadsController(
            IHttpClientFactory httpClientFactory,
            IOptions<LeadsApiSettings> settings,
            ILogger<LeadsController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _logger = logger;
        }

        /// <summary>What the visitor types. Anything else sent in the body (dates, status) is ignored.</summary>
        public sealed class CreateLeadRequest
        {
            public string? CompanyName { get; set; }
            public string? Nuit { get; set; }
            public string? ContactName { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Notes { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost]
        [EnableRateLimiting(RateLimitPolicy)]
        public async Task<IActionResult> Create([FromBody] CreateLeadRequest request, CancellationToken cancellationToken)
        {
            var validationError = Validate(request);
            if (validationError is not null)
                return BadRequest(new ServiceResponse { Flag = false, Message = validationError });

            if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
            {
                _logger.LogError("LeadsApi:BaseUrl is not configured.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new ServiceResponse { Flag = false, Message = "O registo de contas não está disponível de momento." });
            }

            var today = DateTime.UtcNow.Date;
            var payload = new
            {
                companyName = request.CompanyName!.Trim(),
                nuit = request.Nuit!.Trim(),
                contactName = request.ContactName!.Trim(),
                email = request.Email!.Trim(),
                phoneNumber = request.PhoneNumber!.Trim(),
                trialStartDate = today,
                trialEndDate = today.AddDays(_settings.TrialDays),
                status = _settings.TrialStatus,
                notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
                password = request.Password
            };

            try
            {
                var client = _httpClientFactory.CreateClient(HttpClientName);
                using var message = new HttpRequestMessage(HttpMethod.Post, _settings.CreatePath)
                {
                    Content = JsonContent.Create(payload)
                };
                if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
                    message.Headers.TryAddWithoutValidation(_settings.ApiKeyHeader, _settings.ApiKey);

                using var response = await client.SendAsync(message, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new ServiceResponse
                    {
                        Flag = true,
                        Message = "Conta criada com sucesso. Já pode iniciar sessão."
                    });
                }

                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                // Never log the request (it has the password). The leads API answer is enough to diagnose.
                _logger.LogWarning("Leads API answered {StatusCode}: {Body}", (int)response.StatusCode, Truncate(body, 500));

                return response.StatusCode switch
                {
                    HttpStatusCode.Conflict => Conflict(new ServiceResponse
                    {
                        Flag = false,
                        Message = ExtractMessage(body) ?? "Já existe uma conta com estes dados."
                    }),
                    HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity => BadRequest(new ServiceResponse
                    {
                        Flag = false,
                        Message = ExtractMessage(body) ?? "Os dados enviados não são válidos."
                    }),
                    _ => StatusCode(StatusCodes.Status502BadGateway, new ServiceResponse
                    {
                        Flag = false,
                        Message = "Não foi possível criar a conta de momento. Tente mais tarde."
                    })
                };
            }
            catch (Exception ex) when ((ex is HttpRequestException || ex is TaskCanceledException) && !cancellationToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "Could not reach the leads API.");
                return StatusCode(StatusCodes.Status502BadGateway, new ServiceResponse
                {
                    Flag = false,
                    Message = "Não foi possível criar a conta de momento. Tente mais tarde."
                });
            }
        }

        private static string? Validate(CreateLeadRequest? request)
        {
            if (request is null) return "Pedido inválido.";

            if (string.IsNullOrWhiteSpace(request.CompanyName)) return "Indique o nome da empresa.";
            if (string.IsNullOrWhiteSpace(request.Nuit)) return "Indique o NUIT.";
            if (string.IsNullOrWhiteSpace(request.ContactName)) return "Indique o nome do contacto.";
            if (string.IsNullOrWhiteSpace(request.PhoneNumber)) return "Indique o telefone.";
            if (!IsValidEmail(request.Email)) return "Indique um email válido.";
            if (string.IsNullOrEmpty(request.Password) || request.Password.Length < 6)
                return "A palavra-passe deve ter pelo menos 6 caracteres.";

            if (request.CompanyName.Length > 200 || request.ContactName.Length > 150 ||
                request.Nuit.Length > 10 || request.PhoneNumber.Length > 20 ||
                request.Email!.Length > 150 || request.Password.Length > 100 ||
                (request.Notes?.Length ?? 0) > 1000)
                return "Algum dos campos excede o tamanho permitido.";

            return null;
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var value = email.Trim();
            var at = value.IndexOf('@');
            return at > 0 && at < value.Length - 3 && value.IndexOf('.', at) > at + 1 && !value.Contains(' ');
        }

        /// <summary>Reads a human message from the usual shapes: { message }, ProblemDetails or validation errors.</summary>
        private static string? ExtractMessage(string body)
        {
            if (string.IsNullOrWhiteSpace(body)) return null;

            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.ValueKind != JsonValueKind.Object) return null;
                var root = doc.RootElement;

                foreach (var name in new[] { "message", "detail", "title" })
                {
                    foreach (var property in root.EnumerateObject())
                    {
                        if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase) &&
                            property.Value.ValueKind == JsonValueKind.String &&
                            !string.IsNullOrWhiteSpace(property.Value.GetString()))
                        {
                            return Truncate(property.Value.GetString()!, 300);
                        }
                    }
                }

                if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
                {
                    var messages = errors.EnumerateObject()
                        .SelectMany(e => e.Value.ValueKind == JsonValueKind.Array ? e.Value.EnumerateArray() : Enumerable.Empty<JsonElement>())
                        .Where(v => v.ValueKind == JsonValueKind.String)
                        .Select(v => v.GetString())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Take(3);

                    var joined = string.Join(" ", messages);
                    if (!string.IsNullOrWhiteSpace(joined)) return Truncate(joined, 300);
                }
            }
            catch (JsonException)
            {
                // Not JSON: do not show raw server text to a visitor
            }

            return null;
        }

        private static string Truncate(string value, int max) =>
            value.Length <= max ? value : value[..max];
    }
}
