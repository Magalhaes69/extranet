# Script PowerShell para gerar hash de password usando ASP.NET Core Identity
Add-Type -Path "C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App\8.0.11\Microsoft.AspNetCore.Identity.dll"

$hasher = New-Object Microsoft.AspNetCore.Identity.PasswordHasher[Object]
$password = "Admin@123"

$hash = $hasher.HashPassword($null, $password)

Write-Host "Password: $password"
Write-Host "Hash: $hash"
Write-Host ""
Write-Host "SQL Script:"
Write-Host "UPDATE AspNetUsers SET PasswordHash = '$hash' WHERE Email = 'admin@extranet.co.mz';"
