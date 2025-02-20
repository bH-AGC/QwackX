CREATE PROCEDURE [AppUserSchema].[DeleteUser]
    @UserId INT
AS
BEGIN
    -- Supprimer un utilisateur de la table Users de manière définitive
    DELETE FROM [AppUserSchema].[Users]
    WHERE [Id] = @UserId;
END;