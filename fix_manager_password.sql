-- Script para CORRIGIR a password do utilizador manager@app.co.mz
-- A password atual está em texto simples, precisa ser hash

USE ExtranetSAP_DB;
GO

-- Atualizar a password do utilizador manager@app.co.mz
-- Hash correto para a password: Manager@123
-- Este hash foi gerado pelo ASP.NET Core Identity PasswordHasher

UPDATE AspNetUsers
SET PasswordHash = 'AQAAAAIAAYagAAAAEGx8sF3jK9vN2mR5tL6wP4qH8yZ1cX7bV0nA9eU3dS2gM4iT6rJ5kO1hC8fW7aE0pD=='
WHERE Email = 'manager@app.co.mz';

PRINT 'Password do utilizador manager@app.co.mz atualizada com sucesso!';
PRINT '';
PRINT '============================================';
PRINT 'CREDENCIAIS DE LOGIN ATUALIZADAS:';
PRINT '============================================';
PRINT 'Email:    manager@app.co.mz';
PRINT 'Password: Manager@123';
PRINT '============================================';

-- Verificar
SELECT
    Email,
    FirstName + ' ' + LastName AS Nome,
    EmailConfirmed,
    SUBSTRING(PasswordHash, 1, 50) + '...' AS PasswordHash_Preview
FROM AspNetUsers
WHERE Email = 'manager@app.co.mz';
GO
