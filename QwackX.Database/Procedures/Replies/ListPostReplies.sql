DROP PROCEDURE [AppUserSchema].[ListPostReplies]
GO
CREATE PROCEDURE [AppUserSchema].[ListPostReplies]
    @PostId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [EntityType] = 'Reply' AND [EntityId] = r.[Id] AND [IsDisliked] = 0) AS [LikeCount],
           CASE
               WHEN EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = 'Reply' AND [EntityId] = r.[Id] AND [UserId] = @UserId AND [IsDisliked] = 0)
                   THEN CONVERT(BIT, 1)
               ELSE CONVERT(BIT, 0)
               END AS [IsLiked]
    FROM [dbo].[Replies] r
             JOIN [dbo].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;
