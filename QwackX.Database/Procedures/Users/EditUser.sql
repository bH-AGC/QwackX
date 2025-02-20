CREATE PROCEDURE [AppUserSchema].[EditUser]
    @UserId  INT,
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255)
AS
BEGIN 
    UPDATE [AppUserSchema].[Users] 
    SET 
       [Username] = @Username, 
       [Email] = @Email, 
       [PasswordHash] = @PasswordHash 
    WHERE Id = @UserId; 
END