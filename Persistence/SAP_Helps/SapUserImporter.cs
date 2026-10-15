using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.Seeders;
using System.Text;


namespace Persistence.SAP_Helps
{
    public sealed class SapUserImporter
    {
        private const string AppName = "Extranet";
        private const int InvitationValidDays = 3;

        private readonly ISapClient _sap;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SapUserImporter> _logger;

        public SapUserImporter(
            ISapClient sap,
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<SapUserImporter> logger)
        {
            _sap = sap;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<int> ImportAsync(CancellationToken ct)
        {
            var contacts = await _sap.ExecuteQueryListAsync<UserContactResponse>(
                SapSqlQueries.InactiveUserContacts, null, ct);

            var invited = 0;

            foreach (var contact in contacts)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    if (await ImportOneAsync(contact, ct))
                        invited++;
                }
                catch (Exception ex)
                {
                    // One bad row must not stop the others; it is retried on the next run
                    _logger.LogWarning(ex, "Could not import USCT user {Code}.", contact.Code);
                }
            }

            return invited;
        }

        private async Task<bool> ImportOneAsync(UserContactResponse contact, CancellationToken ct)
        {
            var email = contact.U_Email?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contact.U_CardCode))
            {
                _logger.LogWarning("USCT {Code} skipped: missing e-mail or CardCode.", contact.Code);
                return false;
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                var (firstName, lastName) = SplitName(contact.Name);

                user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    CardCode = contact.U_CardCode.Trim(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    TwoFactorEnabled = true,
                    EmailConfirmed = false,
                    Nuit = "258963254"
                };

                // No password: the user sets it through the invitation link
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("USCT {Code}: user not created: {Errors}",
                        contact.Code, string.Join("; ", result.Errors.Select(e => e.Description)));
                    return false;
                }
            }
            else if (user.EmailConfirmed)
            {
                // Already set up earlier: just sync the flag in SAP
                await MarkActiveAsync(contact.Code, ct);
                return false;
            }

            await SendInvitationAsync(user);
            await MarkActiveAsync(contact.Code, ct);

            _logger.LogInformation("USCT {Code}: invitation sent to {Email}.", contact.Code, email);
            return true;
        }

        private async Task SendInvitationAsync(AppUser user)
        {
            var frontendUrl = _configuration["AppSettings:FrontendUrl"]
                ?? throw new InvalidOperationException("AppSettings:FrontendUrl is not configured.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = $"{frontendUrl.TrimEnd('/')}/set-password" +
                       $"?email={Uri.EscapeDataString(user.Email!)}&token={encodedToken}";

            var body = EmailTemplates.Invitation(user.FirstName, link, AppName, InvitationValidDays);
            await _emailService.SendAsync(user.Email!, $"Bem-vindo à {AppName} - defina a sua palavra-passe", body);
        }

        private Task MarkActiveAsync(string code, CancellationToken ct) =>
            _sap.PatchAsync($"U_USCT('{code.Replace("'", "''")}')", new { U_IsActive = "Y" }, ct);

        private static (string First, string Last) SplitName(string? name)
        {
            var parts = (name ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => (string.Empty, string.Empty),
                1 => (parts[0], string.Empty),
                _ => (parts[0], parts[^1])
            };
        }
    }
}
