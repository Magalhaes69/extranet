using Microsoft.AspNetCore.Identity;

namespace Domain.Security
{
    /// <summary>Mensagens do ASP.NET Identity em português (por omissão vêm em inglês).</summary>
    public static class IdentityErrorMessages
    {
        public static string Describe(IdentityResult result) =>
            string.Join(" ", result.Errors.Select(Translate).Distinct());

        public static string Translate(IdentityError error) => error.Code switch
        {
            "PasswordMismatch" => "A palavra-passe actual está incorrecta.",
            "PasswordTooShort" => "A palavra-passe é demasiado curta.",
            "PasswordRequiresDigit" => "A palavra-passe tem de ter pelo menos um número.",
            "PasswordRequiresLower" => "A palavra-passe tem de ter pelo menos uma letra minúscula.",
            "PasswordRequiresUpper" => "A palavra-passe tem de ter pelo menos uma letra maiúscula.",
            "PasswordRequiresNonAlphanumeric" => "A palavra-passe tem de ter pelo menos um símbolo (por exemplo ! @ # $).",
            "PasswordRequiresUniqueChars" => "A palavra-passe tem de ter mais caracteres diferentes.",
            "DuplicateEmail" or "DuplicateUserName" => "Já existe um utilizador com este email.",
            "InvalidEmail" => "O endereço de email é inválido.",
            _ => error.Description
        };
    }
}
