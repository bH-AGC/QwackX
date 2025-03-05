CREATE PROCEDURE [AppUserSchema].[LoginUser]
    @Email NVARCHAR(255),
    @Password NVARCHAR(20)
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM [dbo].[Users]
    WHERE Email = @Email AND PasswordHash = dbo.CreatePasswd(@Password);
END;