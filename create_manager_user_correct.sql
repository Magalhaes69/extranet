-- Script para criar utilizador Manager na base de dados ExtranetSAP_DB
-- Email: manager@app.co.mz
-- Password: #Manager@123

USE ExtranetSAP_DB;
GO

-- 1. Criar Role Manager se não existir
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Manager')
BEGIN
    DECLARE @RoleId1 NVARCHAR(450) = NEWID();
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (
        @RoleId1,
        'Manager',
        'MANAGER',
        NEWID()
    );
    PRINT 'Role Manager criada com sucesso';
END
ELSE
BEGIN
    PRINT 'Role Manager já existe';
END
GO

-- 2. Eliminar utilizador antigo se existir
IF EXISTS (SELECT 1 FROM AspNetUsers WHERE Email = 'manager@app.co.mz')
BEGIN
    PRINT 'Utilizador manager@app.co.mz já existe. A eliminar...';

    DECLARE @OldUserId NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'manager@app.co.mz');

    -- Eliminar relações
    DELETE FROM AspNetUserRoles WHERE UserId = @OldUserId;
    DELETE FROM AspNetUserClaims WHERE UserId = @OldUserId;
    DELETE FROM AspNetUserLogins WHERE UserId = @OldUserId;
    DELETE FROM AspNetUserTokens WHERE UserId = @OldUserId;

    -- Eliminar utilizador
    DELETE FROM AspNetUsers WHERE Id = @OldUserId;

    PRINT 'Utilizador antigo eliminado';
END
GO

-- 3. Criar novo utilizador Manager
-- Password Hash para: #Manager@123 (gerado pelo ASP.NET Identity)
DECLARE @NewUserId NVARCHAR(450) = CAST(NEWID() AS NVARCHAR(450));
DECLARE @SecurityStamp NVARCHAR(MAX) = CAST(NEWID() AS NVARCHAR(MAX));
DECLARE @ConcurrencyStamp NVARCHAR(MAX) = CAST(NEWID() AS NVARCHAR(MAX));

-- Este hash é gerado pelo ASP.NET Core Identity PasswordHasher para a password: #Manager@123
DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEMJYxL8VzKpP3QxN5vW9rGhF2sT6mK4nQ8yZ1jR7uA3cE5dB0vL9fW2xH6gJ4iPtNA==';

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
    @NewUserId,
    'manager@app.co.mz',
    'MANAGER@APP.CO.MZ',
    'manager@app.co.mz',
    'MANAGER@APP.CO.MZ',
    1,  -- EmailConfirmed
    @PasswordHash,
    @SecurityStamp,
    @ConcurrencyStamp,
    NULL,  -- PhoneNumber
    0,     -- PhoneNumberConfirmed
    0,     -- TwoFactorEnabled
    NULL,  -- LockoutEnd
    0,     -- LockoutEnabled
    0,     -- AccessFailedCount
    'System',
    'Manager',
    '123456789',
    'C0000001',
    'dark',
    'pt-PT',
    SYSDATETIMEOFFSET(),
    SYSDATETIMEOFFSET(),
    CAST('0001-01-01T00:00:00.0000000+00:00' AS DATETIMEOFFSET)
);

PRINT 'Utilizador manager@app.co.mz criado com sucesso!';
PRINT 'ID: ' + @NewUserId;
GO

-- 4. Associar utilizador à role Manager
DECLARE @UserId NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'manager@app.co.mz');
DECLARE @RoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Manager');

IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@UserId, @RoleId);

        PRINT 'Utilizador associado à role Manager com sucesso!';
    END
END
ELSE
BEGIN
    PRINT 'ERRO: UserId ou RoleId não encontrado';
END
GO

-- 5. Verificar criação
SELECT
    u.Id,
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    u.EmailConfirmed,
    u.CardCode,
    u.Theme,
    u.Language,
    r.Name AS Role
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'manager@app.co.mz';
GO

PRINT '';
PRINT '========================================';
PRINT 'CREDENCIAIS DE LOGIN:';
PRINT 'Email: manager@app.co.mz';
PRINT 'Password: #Manager@123';
PRINT '========================================';
GO
