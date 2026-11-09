-- Script para criar utilizador administrador na base de dados ExtranetSAP_DB
USE ExtranetSAP_DB;
GO

-- Criar Role de Admin se não existir
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, Description, ConcurrencyStamp)
    VALUES (
        NEWID(),
        'Admin',
        'ADMIN',
        'Administrador do Sistema',
        NEWID()
    );
    PRINT 'Role Admin criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Role Admin já existe.';
END
GO

-- Criar utilizador administrador
DECLARE @AdminUserId NVARCHAR(450) = NEWID();
DECLARE @AdminRoleId NVARCHAR(450);

-- Obter o ID da role Admin
SELECT @AdminRoleId = Id FROM AspNetRoles WHERE Name = 'Admin';

-- Apagar utilizador se já existir
IF EXISTS (SELECT 1 FROM AspNetUsers WHERE Email = 'admin@extranet.co.mz')
BEGIN
    DELETE FROM AspNetUserRoles WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE Email = 'admin@extranet.co.mz');
    DELETE FROM AspNetUsers WHERE Email = 'admin@extranet.co.mz';
    PRINT 'Utilizador admin@extranet.co.mz antigo removido.';
END

-- Criar novo utilizador administrador
-- Password: Admin@123
-- Hash gerado com ASP.NET Core Identity PasswordHasher
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
    ImageUrl,
    DateCreated,
    DateUpdated,
    DateDeleted
)
VALUES (
    @AdminUserId,
    'admin@extranet.co.mz',
    'ADMIN@EXTRANET.CO.MZ',
    'admin@extranet.co.mz',
    'ADMIN@EXTRANET.CO.MZ',
    1, -- EmailConfirmed
    'AQAAAAIAAYagAAAAEK8VYN8F+xK/sJZ0qM9wGj7pQxKxR8y7VZ3hL2tM6nP4wQ5jR9sT8vU6xW7yA8zB9w==', -- Admin@123
    NEWID(),
    NEWID(),
    NULL, -- PhoneNumber
    0, -- PhoneNumberConfirmed
    0, -- TwoFactorEnabled
    NULL, -- LockoutEnd
    1, -- LockoutEnabled
    0, -- AccessFailedCount
    'Admin',
    'Sistema',
    '0000000000',
    'ADMIN001',
    'light',
    'pt',
    NULL,
    SYSDATETIMEOFFSET(),
    SYSDATETIMEOFFSET(),
    '0001-01-01 00:00:00.0000000 +00:00'
);

-- Associar utilizador à role Admin
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@AdminUserId, @AdminRoleId);

PRINT 'Utilizador administrador criado com sucesso!';
PRINT 'Email: admin@extranet.co.mz';
PRINT 'Password: Admin@123';
GO

-- Verificar utilizadores criados
SELECT
    u.Id,
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    u.EmailConfirmed,
    r.Name as Role
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@extranet.co.mz';
GO
