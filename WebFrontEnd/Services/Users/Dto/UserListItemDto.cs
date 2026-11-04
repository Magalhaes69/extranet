namespace WebFrontEnd.Services.Users.Dto
{
    /// <summary>One row of the users list (mirrors Application.Dtos.UserListItemDto of the API).</summary>
    public class UserListItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nuit { get; set; }
        public string? CardCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool TwoFactorEnabled { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
