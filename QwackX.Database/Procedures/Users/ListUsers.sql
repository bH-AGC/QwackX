CREATE PROCEDURE AppUserSchema.ListUsers
AS
BEGIN
    -- Sélectionner tous les utilisateurs de la table Users
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM AppUserSchema.Users;
END;