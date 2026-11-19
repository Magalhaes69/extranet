using Application.UseCases.onexuser.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using WebAPI.Services;
using WebAPI.Settings;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Public "create account" endpoint used by the register page. It validates what the
    /// visitor typed and runs the same use case the LicenseKey integration uses
    /// (<see cref="CreateOnexClientUserRequest"/>): the Lead is created in the LicenseKey,
    /// whose Code becomes the CardCode of the local user, created pending email confirmation.
    /// There is no local fallback: without the LicenseKey there is no account.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LeadsController : ControllerBase
    {
        public const string RateLimitPolicy = "leads";

        private readonly IMediator _mediator;
        private readonly LeadsApiSettings _settings;
        private readonly ILogger<LeadsController> _logger;

        public LeadsController(
            IMediator mediator,
            IOptions<LeadsApiSettings> settings,
            ILogger<LeadsController> logger)
        {
            _mediator = mediator;
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

            _logger.LogInformation("Creating account via LicenseKey for {Email}", request.Email);

            var command = new CreateOnexClientUserRequest
            {
                CompanyName = request.CompanyName!.Trim(),
                TaxNumber = request.Nuit!.Trim(),
                ContactName = request.ContactName!.Trim(),
                AdminEmail = request.Email!.Trim(),
                PhoneNumber = request.PhoneNumber!.Trim(),
                AdminInitialPassword = request.Password!,
                // O estado do trial decide-se aqui (secção "LeadsApi"), nunca pelo visitante.
                Status = _settings.TrialStatus,
                Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim()
                // CardCode fica vazio de propósito: é o caso de uso que cria o Lead
                // no LicenseKey e usa o Code devolvido.
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.Succeeded && result.Created)
            {
                return Ok(new ServiceResponse
                {
                    Flag = true,
                    Message = "Conta criada. Verifique o seu email para a ativar."
                });
            }

            if (result.Succeeded && !result.Created)
            {
                // O utilizador já existia. Para quem está a registar-se é um conflito,
                // e a mensagem do caso de uso diz se falta apenas confirmar o email.
                return Conflict(new ServiceResponse
                {
                    Flag = false,
                    Message = IsSafeForVisitor(result.Message)
                        ? result.Message
                        : "Já existe uma conta com este email."
                });
            }

            _logger.LogWarning("Account creation failed for {Email}: {Message}",
                request.Email, Truncate(result.Message ?? string.Empty, 500));

            return BadRequest(new ServiceResponse
            {
                Flag = false,
                Message = IsSafeForVisitor(result.Message)
                    ? result.Message!
                    : "Não foi possível criar a conta de momento. Tente mais tarde."
            });
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

        /// <summary>
        /// Marcas de erro técnico que nunca devem chegar a um visitante anónimo: falam da
        /// infraestrutura (base de dados, rede, máquinas, excepções) e não do que ele escreveu.
        /// Uma mensagem com qualquer uma destas é descartada e o chamador usa o texto genérico;
        /// o original fica no log, que é onde serve para diagnosticar.
        /// </summary>
        private static readonly string[] TechnicalMarkers =
        {
            "sql", "database", "connection", "connectionstring", "data source",
            "trusted_connection", "initial catalog", "provider:", "instance",
            "remote connections", "timeout expired", "deadlock",
            "exception", "stack trace", "stacktrace", "   at ",
            "system.", "microsoft.", "npgsql", "entityframework",
            "localhost", "127.0.0.1", "http://", "https://",
            "c:\\", "d:\\", "/var/", "/home/",
            "inner error", "see the inner"
        };

        /// <summary>
        /// Uma mensagem só segue para o visitante se falar dos dados dele. Falha em segurança:
        /// perante dúvida (vazia, demasiado longa, com marcas técnicas) devolve false.
        /// </summary>
        private static bool IsSafeForVisitor(string? message)
        {
            if (string.IsNullOrWhiteSpace(message)) return false;

            // Mensagens de negócio e de validação são curtas. Um texto longo costuma ser
            // uma excepção formatada, por isso não arrisca.
            if (message.Length > 200) return false;

            foreach (var marker in TechnicalMarkers)
            {
                if (message.Contains(marker, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private static string Truncate(string value, int max) =>
            value.Length <= max ? value : value[..max];
    }
}
