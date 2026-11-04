namespace WebFrontEnd.Services.Users.Dto
{
    /// <summary>Data entered in the add/edit user form (validated in the page so the messages are localized).</summary>
    public class UserFormDto
    {
        public string? Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nuit { get; set; } = string.Empty;
        public string CardCode { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "User";
        public bool TwoFactorEnabled { get; set; } = true;

        // Only used when creating a user
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
