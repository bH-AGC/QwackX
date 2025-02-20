CREATE PROCEDURE [dbo].[DeletePost]
    @PostId INT
AS
BEGIN
    UPDATE [dbo].[Posts]
    SET [IsDeleted] = 1
    WHERE [Id] = @PostId;
END;