CREATE PROCEDURE [AppUserSchema].[LoginUser]
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM AppUserSchema.Users
    WHERE Email = @Email;
END