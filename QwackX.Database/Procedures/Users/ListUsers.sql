CREATE PROCEDURE AppUserSchema.ListUsers
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM [dbo].[Users];
END;