DROP PROCEDURE [AppUserSchema].[EditUser]
GO
CREATE PROCEDURE [AppUserSchema].[EditUser]
    @UserId  INT,
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN 
    UPDATE [AppUserSchema].[Users] 
    SET 
       [Username] = @Username, 
       [Email] = @Email, 
       [PasswordHash] = dbo.CreatePasswd(@Password) 
    WHERE Id = @UserId; 
END