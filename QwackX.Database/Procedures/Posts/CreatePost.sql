CREATE PROCEDURE [dbo].[CreatePost]
    @UserId INT,
    @Title NVARCHAR(255),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Posts] ([UserId], [Title], [Description])
    VALUES (@UserId, @Title, @Description);
END;