#!/usr/bin/env dotnet-script
#r "nuget: Microsoft.AspNetCore.Identity, 2.2.0"

using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<object>();
var password = "Admin@123";
var hash = hasher.HashPassword(null, password);

Console.WriteLine($"Password: {password}");
Console.WriteLine($"Hash: {hash}");
Console.WriteLine();
Console.WriteLine("SQL Script:");
Console.WriteLine($"UPDATE AspNetUsers SET PasswordHash = '{hash}' WHERE Email = 'admin@extranet.co.mz';");
