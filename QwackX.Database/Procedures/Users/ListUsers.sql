CREATE PROCEDURE AppUserSchema.ListUsers
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, CreatedAt
    FROM AppUserSchema.Users;
END;