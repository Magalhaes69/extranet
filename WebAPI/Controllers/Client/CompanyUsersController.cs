using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;
using Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;

namespace WebAPI.Controllers.Client;

/// <summary>
/// Gestão dos utilizadores da empresa. Só gestores (Manager/Administrator) têm acesso,
/// e apenas aos utilizadores do seu próprio CardCode.
/// </summary>
[ApiController]
[Route("api/client/company-users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AppRoleNames.ManagerRoles)]
public class CompanyUsersController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<CompanyUsersController> _logger;
    private readonly IUserActivityLogger _activity;
    private readonly INotificationService _notifications;

    public CompanyUsersController(UserManager<AppUser> userManager, ILogger<CompanyUsersController> logger, IUserActivityLogger activity, INotificationService notifications)
    {
        _userManager = userManager;
        _logger = logger;
        _activity = activity;
        _notifications = notifications;
    }

    private string? CardCode => User.FindFirst("CardCode")?.Value;
    private string? CurrentUserId => User.FindFirst("UserId")?.Value;

    // ------------------------------------------------------------------ listagem

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (string.IsNullOrWhiteSpace(CardCode))
            return Forbid();

        var users = _userManager.Users
            .Where(u => u.CardCode == CardCode)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToList();

        var result = new List<CompanyUserDto>();
        foreach (var user in users)
            result.Add(await ToDtoAsync(user));

        return Ok(result);
    }

    // ------------------------------------------------------------------ detalhe

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await FindCompanyUserAsync(id);
        if (user is null)
            return NotFound(new { message = "Utilizador não encontrado." });

        return Ok(await ToDtoAsync(user));
    }

    // ------------------------------------------------------------------ criar

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyUserRequest request)
    {
        var manager = await _userManager.FindByIdAsync(CurrentUserId ?? string.Empty);
        if (manager is null || string.IsNullOrWhiteSpace(manager.CardCode))
            return Forbid();

        var error = Validate(request.FirstName, request.LastName, request.Processes);
        if (error is not null)
            return BadRequest(new { message = error });

        var email = request.Email?.Trim() ?? string.Empty;
        if (!IsValidEmail(email))
            return BadRequest(new { message = "O endereço de email é inválido." });

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "A palavra-passe é obrigatória." });

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            // A palavra-passe é definida pelo gestor, por isso não há email de confirmação.
            EmailConfirmed = true,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
            Nuit = manager.Nuit,
            CardCode = manager.CardCode,
            Theme = manager.Theme,
            Language = manager.Language,
            TwoFactorEnabled = false,
            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow
        };

        var created = await _userManager.CreateAsync(user, request.Password);
        if (!created.Succeeded)
            return BadRequest(new { message = Describe(created) });

        await _userManager.AddToRoleAsync(user, AppRoleNames.User);
        await SyncProcessesAsync(user, request.Processes);

        _logger.LogInformation("Utilizador {Email} criado pelo gestor {Manager}", email, manager.Email);
        await _activity.LogAsync(manager.Id, manager.CardCode, UserActivityActions.UserCreated, email);
        await _notifications.AddAsync(user.Id, user.CardCode, NotificationTypes.AccountAccess, NotificationLevels.Success,
            "Bem-vindo",
            $"A sua conta foi criada por {manager.FirstName} {manager.LastName}. Tem acesso a: {DescribeProcesses(request.Processes)}.".Replace("  ", " "),
            "/", null);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, await ToDtoAsync(user));
    }

    // ------------------------------------------------------------------ editar

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCompanyUserRequest request)
    {
        var user = await FindCompanyUserAsync(id);
        if (user is null)
            return NotFound(new { message = "Utilizador não encontrado." });

        if (await IsManagerAsync(user))
            return StatusCode(403, new { message = "Não é possível editar um gestor." });

        var error = Validate(request.FirstName, request.LastName, request.Processes);
        if (error is not null)
            return BadRequest(new { message = error });

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim();
        user.DateUpdated = DateTimeOffset.UtcNow;

        var updated = await _userManager.UpdateAsync(user);
        if (!updated.Succeeded)
            return BadRequest(new { message = Describe(updated) });

        var wasActive = !(user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow);

        // Activar / desactivar (bloqueio permanente)
        await _userManager.SetLockoutEnabledAsync(user, true);
        await _userManager.SetLockoutEndDateAsync(user, request.IsActive ? null : DateTimeOffset.MaxValue);

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var reset = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!reset.Succeeded)
                return BadRequest(new { message = Describe(reset) });
        }

        var processesBefore = (await _userManager.GetClaimsAsync(user))
            .Where(c => c.Type == AppProcesses.ClaimType).Select(c => c.Value).ToHashSet();

        await SyncProcessesAsync(user, request.Processes);

        var processesAfter = request.Processes.Where(AppProcesses.IsValid).ToHashSet();
        if (!processesBefore.SetEquals(processesAfter))
        {
            await _notifications.AddAsync(user.Id, user.CardCode, NotificationTypes.AccountAccess, NotificationLevels.Info,
                "Permissões actualizadas",
                $"O gestor actualizou o seu acesso. Passa a ter acesso a: {DescribeProcesses(processesAfter)}.",
                "/", null);
        }

        var action = wasActive == request.IsActive
            ? UserActivityActions.UserUpdated
            : request.IsActive ? UserActivityActions.UserActivated : UserActivityActions.UserDeactivated;
        await _activity.LogAsync(CurrentUserId ?? string.Empty, CardCode, action, user.Email);

        return Ok(await ToDtoAsync(user));
    }

    // ------------------------------------------------------------------ auxiliares

    private async Task<AppUser?> FindCompanyUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user is not null && user.CardCode == CardCode ? user : null;
    }

    private async Task<bool> IsManagerAsync(AppUser user) =>
        await _userManager.IsInRoleAsync(user, AppRoleNames.Manager) ||
        await _userManager.IsInRoleAsync(user, AppRoleNames.Administrator);

    private async Task<CompanyUserDto> ToDtoAsync(AppUser user)
    {
        var isManager = await IsManagerAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        return new CompanyUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            IsActive = !(user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow),
            IsManager = isManager,
            Processes = isManager
                ? AppProcesses.All.ToList()
                : claims.Where(c => c.Type == AppProcesses.ClaimType && AppProcesses.IsValid(c.Value))
                        .Select(c => c.Value).Distinct().ToList(),
            DateCreated = user.DateCreated
        };
    }

    /// <summary>Substitui os processos do utilizador pelos indicados (claims "process").</summary>
    private async Task SyncProcessesAsync(AppUser user, IEnumerable<string> processes)
    {
        var wanted = processes.Where(AppProcesses.IsValid).Distinct().ToHashSet();
        var current = (await _userManager.GetClaimsAsync(user))
            .Where(c => c.Type == AppProcesses.ClaimType)
            .ToList();

        var toRemove = current.Where(c => !wanted.Contains(c.Value)).ToList();
        if (toRemove.Count > 0)
            await _userManager.RemoveClaimsAsync(user, toRemove);

        var existing = current.Select(c => c.Value).ToHashSet();
        var toAdd = wanted.Where(p => !existing.Contains(p))
                          .Select(p => new Claim(AppProcesses.ClaimType, p))
                          .ToList();
        if (toAdd.Count > 0)
            await _userManager.AddClaimsAsync(user, toAdd);
    }

    private static string DescribeProcesses(IEnumerable<string> processes)
    {
        var names = processes
            .Where(AppProcesses.IsValid)
            .Distinct()
            .Select(p => p switch
            {
                AppProcesses.Dashboard => "Painel",
                AppProcesses.Requests => "Pedidos",
                AppProcesses.Payments => "Pagamentos",
                AppProcesses.Licenses => "Licenças e notificações",
                _ => p
            })
            .ToList();

        return names.Count == 0 ? "nenhuma área" : string.Join(", ", names);
    }

    private static string? Validate(string firstName, string lastName, IEnumerable<string> processes)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return "O nome é obrigatório.";
        if (string.IsNullOrWhiteSpace(lastName))
            return "O apelido é obrigatório.";
        if (processes.Any(p => !AppProcesses.IsValid(p)))
            return "Foi indicado um processo inválido.";
        return null;
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        try { return new MailAddress(email).Address == email; }
        catch { return false; }
    }

    private static string Describe(IdentityResult result) => IdentityErrorMessages.Describe(result);
}
