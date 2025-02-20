-- Déconnecter tous les utilisateurs de la base de données QwackX
USE master;
GO
ALTER DATABASE QwackX SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Supprimer la base de données QwackX
DROP DATABASE QwackX;
GO

-- Recréer la base de données QwackX
CREATE DATABASE QwackX;
GO
