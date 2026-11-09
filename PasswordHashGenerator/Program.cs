using Microsoft.AspNetCore.Identity;

// Gerar hash da password usando ASP.NET Identity PasswordHasher
var hasher = new PasswordHasher<object>();
var passwords = new Dictionary<string, string>
{
    { "#Manager@123", "" },
    { "Admin@123", "" },
    { "Admin@2024", "" }
};

Console.WriteLine("=== Password Hashes Gerados ===\n");

foreach (var pwd in passwords.Keys.ToList())
{
    var hash = hasher.HashPassword(null!, pwd);
    passwords[pwd] = hash;
    Console.WriteLine($"Password: {pwd}");
    Console.WriteLine($"Hash: {hash}\n");
}

Console.WriteLine("\n=== Script SQL para atualizar utilizador ===\n");
Console.WriteLine("-- Atualizar password do utilizador manager@app.co.mz");
Console.WriteLine($"UPDATE AspNetUsers SET PasswordHash = '{passwords["#Manager@123"]}' WHERE Email = 'manager@app.co.mz';");
Console.WriteLine();
Console.WriteLine("-- Atualizar password do utilizador admin@extranet.co.mz");
Console.WriteLine($"UPDATE AspNetUsers SET PasswordHash = '{passwords["Admin@123"]}' WHERE Email = 'admin@extranet.co.mz';");
