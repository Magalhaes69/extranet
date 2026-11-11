-- Script para confirmar o email do utilizador manager@app.co.mz
-- Problema: O login está bloqueado porque EmailConfirmed = false

USE ExtranetSAP_DB;
GO

-- Primeiro, verificar o estado atual do utilizador
PRINT '============================================';
PRINT 'ESTADO ATUAL DO UTILIZADOR';
PRINT '============================================';

SELECT
    Id,
    Email,
    UserName,
    FirstName + ' ' + LastName AS Nome,
    EmailConfirmed,
    TwoFactorEnabled,
    LockoutEnabled,
    AccessFailedCount,
    CASE
        WHEN LockoutEnd IS NOT NULL AND LockoutEnd > GETUTCDATE()
        THEN 'BLOQUEADO até ' + CONVERT(VARCHAR, LockoutEnd, 120)
        ELSE 'Não bloqueado'
    END AS EstadoBloqueio
FROM AspNetUsers
WHERE Email = 'manager@app.co.mz';

PRINT '';
PRINT '============================================';
PRINT 'A ATUALIZAR EmailConfirmed PARA TRUE...';
PRINT '============================================';

-- Atualizar o campo EmailConfirmed para true
UPDATE AspNetUsers
SET EmailConfirmed = 1
WHERE Email = 'manager@app.co.mz';

-- Também garantir que o utilizador não está bloqueado
UPDATE AspNetUsers
SET
    LockoutEnd = NULL,
    AccessFailedCount = 0
WHERE Email = 'manager@app.co.mz';

PRINT 'Email confirmado com sucesso!';
PRINT '';

-- Verificar o estado final
PRINT '============================================';
PRINT 'ESTADO FINAL DO UTILIZADOR';
PRINT '============================================';

SELECT
    Id,
    Email,
    UserName,
    FirstName + ' ' + LastName AS Nome,
    EmailConfirmed,
    TwoFactorEnabled,
    LockoutEnabled,
    AccessFailedCount
FROM AspNetUsers
WHERE Email = 'manager@app.co.mz';

PRINT '';
PRINT '============================================';
PRINT 'AGORA PODE FAZER LOGIN COM:';
PRINT '============================================';
PRINT 'Email:    manager@app.co.mz';
PRINT 'Password: Manager@123';
PRINT '============================================';

GO
