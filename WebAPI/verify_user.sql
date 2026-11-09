-- Script para verificar utilizador criado
USE ExtranetSAP_DB;
GO

-- Ver todos os utilizadores
SELECT
    u.Id,
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    u.CardCode,
    u.EmailConfirmed,
    u.PasswordHash,
    r.Name as Role
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id;
GO

-- Ver todas as roles
SELECT * FROM AspNetRoles;
GO
