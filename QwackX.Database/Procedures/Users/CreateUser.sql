CREATE PROCEDURE [AppUserSchema].[CreateUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    INSERT INTO [AppUserSchema].[Users] ([Username], [Email], [PasswordHash])
    VALUES (@Username, @Email, @PasswordHash);
END;