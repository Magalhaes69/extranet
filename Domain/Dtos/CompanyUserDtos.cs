namespace Domain.Dtos
{
    public class CompanyUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        /// <summary>Gestor/administrador: tem acesso a tudo e não é editável por outros gestores.</summary>
        public bool IsManager { get; set; }
        public List<string> Processes { get; set; } = new();
        public DateTimeOffset DateCreated { get; set; }
    }

    public class CreateCompanyUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public List<string> Processes { get; set; } = new();
    }

    public class UpdateCompanyUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> Processes { get; set; } = new();
        /// <summary>Opcional: se preenchida, redefine a palavra-passe do utilizador.</summary>
        public string? NewPassword { get; set; }
    }
}
