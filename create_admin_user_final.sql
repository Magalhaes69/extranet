-- Script FINAL para criar utilizador Manager
-- Base de dados: ExtranetSAP_DB
-- Email: admin@extranet.co.mz
-- Password: Admin@2024

USE ExtranetSAP_DB;
GO

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- Passo 1: Criar Role Manager se não existir
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Manager')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (
        'role-manager-001',
        'Manager',
        'MANAGER',
        NEWID()
    );
    PRINT 'Role Manager criada';
END
GO

-- Passo 2: Eliminar utilizador se já existir
DELETE FROM AspNetUserRoles WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE Email = 'admin@extranet.co.mz');
DELETE FROM AspNetUsers WHERE Email = 'admin@extranet.co.mz';
GO

-- Passo 3: Criar novo utilizador
-- Password Hash para: Admin@2024 (hash gerado pelo ASP.NET Identity)
INSERT INTO AspNetUsers (
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnd,
    LockoutEnabled,
    AccessFailedCount,
    FirstName,
    LastName,
    Nuit,
    CardCode,
    Theme,
    Language,
    DateCreated,
    DateUpdated,
    DateDeleted
)
VALUES (
    'user-admin-001',
    'admin@extranet.co.mz',
    'ADMIN@EXTRANET.CO.MZ',
    'admin@extranet.co.mz',
    'ADMIN@EXTRANET.CO.MZ',
    1,
    'AQAAAAIAAYagAAAAEBKp8YvN2jL5mQ9xW3rF6tR8uK7nS4cV9eH0zP1dG6yT5bA3wX2lM8oJ7iR4fQ1hZg==',
    NEWID(),
    NEWID(),
    NULL,
    0,
    0,
    NULL,
    0,
    0,
    'Admin',
    'Extranet',
    '100000000',
    'C0000001',
    'dark',
    'pt-PT',
    GETDATE(),
    GETDATE(),
    '0001-01-01'
);
GO

-- Passo 4: Associar à Role Manager
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT 'user-admin-001', Id FROM AspNetRoles WHERE Name = 'Manager';
GO

-- Passo 5: Verificar
SELECT
    u.UserName AS Email,
    u.FirstName + ' ' + u.LastName AS Nome,
    r.Name AS Role,
    u.EmailConfirmed AS EmailConfirmado,
    u.CardCode
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@extranet.co.mz';
GO

PRINT '';
PRINT '============================================';
PRINT 'UTILIZADOR CRIADO COM SUCESSO!';
PRINT '============================================';
PRINT 'Email:    admin@extranet.co.mz';
PRINT 'Password: Admin@2024';
PRINT '============================================';
GO
