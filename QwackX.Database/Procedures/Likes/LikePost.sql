CREATE PROCEDURE [dbo].[LikePost]
    @PostId INT,
    @UserId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [PostId] = @PostId AND [UserId] = @UserId)
        BEGIN
            INSERT INTO [dbo].[Likes] ([PostId], [UserId])
            VALUES (@PostId, @UserId);
        END
END;