CREATE PROCEDURE [AppUserSchema].[DetailUser]
    @UserId INT
AS
BEGIN
    SELECT [Id], [Username], [Email], [PasswordHash], [CreatedAt]
    FROM [dbo].[Users]
    WHERE [Id] = @UserId;
    END;
GO