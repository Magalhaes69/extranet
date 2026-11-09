-- Script para criar utilizador Manager na base de dados ExtranetSAP_DB
-- Password: #Manager@123
-- Email: manager@app.co.mz

USE ExtranetSAP_DB;
GO

-- 1. Criar a Role Manager se não existir
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
    PRINT 'Role Manager criada com sucesso';
END
ELSE
BEGIN
    PRINT 'Role Manager já existe';
END
GO

-- 2. Verificar se o utilizador já existe
IF EXISTS (SELECT 1 FROM AppUsers WHERE Email = 'manager@app.co.mz')
BEGIN
    PRINT 'Utilizador manager@app.co.mz já existe. A eliminar utilizador antigo...';

    -- Eliminar relações na tabela AppUserRoles
    DELETE FROM AppUserRoles
    WHERE UserId IN (SELECT Id FROM AppUsers WHERE Email = 'manager@app.co.mz');

    -- Eliminar o utilizador
    DELETE FROM AppUsers WHERE Email = 'manager@app.co.mz';

    PRINT 'Utilizador antigo eliminado';
END
GO

-- 3. Criar o novo utilizador Manager
DECLARE @UserId NVARCHAR(450) = NEWID();
DECLARE @RoleId NVARCHAR(450) = (SELECT Id FROM AppRoles WHERE Name = 'Manager');

-- Password Hash para: #Manager@123
-- Este é o hash gerado pelo ASP.NET Identity PasswordHasher
DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEBvZ8X7qVJKGxJ0YqzGdZ8QZxrxqUJ3F7VZ1LFKYvO7YxMJxrZ5XWqHvN6zKGBQ5kA==';

INSERT INTO AppUsers (
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
    @UserId,                                -- Id
    'manager@app.co.mz',                    -- UserName
    'MANAGER@APP.CO.MZ',                    -- NormalizedUserName
    'manager@app.co.mz',                    -- Email
    'MANAGER@APP.CO.MZ',                    -- NormalizedEmail
    1,                                      -- EmailConfirmed
    @PasswordHash,                          -- PasswordHash
    NEWID(),                                -- SecurityStamp
    NEWID(),                                -- ConcurrencyStamp
    NULL,                                   -- PhoneNumber
    0,                                      -- PhoneNumberConfirmed
    0,                                      -- TwoFactorEnabled
    NULL,                                   -- LockoutEnd
    0,                                      -- LockoutEnabled
    0,                                      -- AccessFailedCount
    'System',                               -- FirstName
    'Manager',                              -- LastName
    '123456789',                            -- Nuit
    'C0000001',                             -- CardCode
    'dark',                                 -- Theme
    'pt-PT',                                -- Language
    GETUTCDATE(),                           -- DateCreated
    GETUTCDATE(),                           -- DateUpdated
    '0001-01-01T00:00:00.0000000+00:00'    -- DateDeleted
);

PRINT 'Utilizador manager@app.co.mz criado com sucesso';
PRINT 'ID do utilizador: ' + @UserId;
GO

-- 4. Associar o utilizador à role Manager
DECLARE @UserId NVARCHAR(450) = (SELECT Id FROM AppUsers WHERE Email = 'manager@app.co.mz');
DECLARE @RoleId NVARCHAR(450) = (SELECT Id FROM AppRoles WHERE Name = 'Manager');

IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AppUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AppUserRoles (UserId, RoleId)
        VALUES (@UserId, @RoleId);

        PRINT 'Utilizador associado à role Manager com sucesso';
    END
    ELSE
    BEGIN
        PRINT 'Utilizador já está associado à role Manager';
    END
END
ELSE
BEGIN
    PRINT 'ERRO: Não foi possível encontrar o UserId ou RoleId';
END
GO

-- 5. Verificar a criação
SELECT
    u.Id,
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    u.EmailConfirmed,
    u.TwoFactorEnabled,
    u.Theme,
    u.Language,
    r.Name AS RoleName
FROM AppUsers u
LEFT JOIN AppUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AppRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'manager@app.co.mz';
GO

PRINT '========================================';
PRINT 'CREDENCIAIS DE LOGIN:';
PRINT 'Email: manager@app.co.mz';
PRINT 'Password: #Manager@123';
PRINT '========================================';
GO
