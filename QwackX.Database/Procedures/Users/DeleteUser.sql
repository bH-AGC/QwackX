CREATE PROCEDURE [AppUserSchema].[DeleteUser]
    @UserId INT
AS
BEGIN
    DELETE FROM [dbo].[Users]
    WHERE [Id] = @UserId;
END;