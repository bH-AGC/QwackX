CREATE PROCEDURE [AppUserSchema].[DeleteUser]
    @UserId INT
AS
BEGIN
    DELETE FROM [AppUserSchema].[Users]
    WHERE [Id] = @UserId;
END;