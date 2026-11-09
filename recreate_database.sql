-- Script para recriar a base de dados ExtranetSAP_DB do zero
USE master;
GO

-- Fechar todas as conexões ativas
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'ExtranetSAP_DB')
BEGIN
    ALTER DATABASE ExtranetSAP_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ExtranetSAP_DB;
END
GO

-- Criar nova base de dados
CREATE DATABASE ExtranetSAP_DB;
GO

PRINT 'Base de dados ExtranetSAP_DB recriada com sucesso!';
GO
