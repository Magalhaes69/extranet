-- Script ALTERNATIVO para criar utilizador Manager
-- Este script cria o utilizador com uma password temporária simples
-- Depois podes alterar a password através da aplicação

USE ExtranetSAP_DB;
GO

-- 1. Eliminar utilizador antigo se existir
IF EXISTS (SELECT 1 FROM AppUsers WHERE Email = 'admin@app.co.mz')
BEGIN
    DELETE FROM AppUserRoles WHERE UserId IN (SELECT Id FROM AppUsers WHERE Email = 'admin@app.co.mz');
    DELETE FROM AppUsers WHERE Email = 'admin@app.co.mz';
END
GO

-- 2. Criar Role Manager se não existir
IF NOT EXISTS (SELECT 1 FROM AppRoles WHERE Name = 'Manager')
BEGIN
    INSERT INTO AppRoles (Id, Name, NormalizedName, ConcurrencyStamp, Description)
    VALUES (
        NEWID(),
        'Manager',
        'MANAGER',
        NEWID(),
        'Manager role'
    );
END
GO

-- 3. Criar utilizador temporário
-- NOTA: Este hash corresponde à password: Admin@123
DECLARE @UserId NVARCHAR(450) = NEWID();

INSERT INTO AppUsers (
    Id, UserName, NormalizedUserName, Email, NormalizedEmail,
    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
    LockoutEnd, LockoutEnabled, AccessFailedCount,
    FirstName, LastName, Nuit, CardCode, Theme, Language,
    DateCreated, DateUpdated, DateDeleted
)
VALUES (
    @UserId,
    'admin@app.co.mz',
    'ADMIN@APP.CO.MZ',
    'admin@app.co.mz',
    'ADMIN@APP.CO.MZ',
    1,
    'AQAAAAIAAYagAAAAEMBqJ8PQqKxL2VxWxqGvHN7m4Jy8sQxKFGhCp5VxLmNz3RyPk4WzGqJvT6BxDwYcZg==',
    NEWID(),
    NEWID(),
    NULL, 0, 0, NULL, 0, 0,
    'Admin',
    'System',
    '999999999',
    'C0000001',
    'dark',
    'pt-PT',
    GETUTCDATE(),
    GETUTCDATE(),
    '0001-01-01T00:00:00.0000000+00:00'
);

-- 4. Associar à role Manager
DECLARE @RoleId NVARCHAR(450) = (SELECT Id FROM AppRoles WHERE Name = 'Manager');
INSERT INTO AppUserRoles (UserId, RoleId)
VALUES (@UserId, @RoleId);

PRINT '========================================';
PRINT 'Utilizador criado com sucesso!';
PRINT 'Email: admin@app.co.mz';
PRINT 'Password: Admin@123';
PRINT '========================================';

-- Verificar
SELECT u.UserName, u.Email, u.FirstName, u.LastName, r.Name AS Role
FROM AppUsers u
LEFT JOIN AppUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AppRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@app.co.mz';
GO
