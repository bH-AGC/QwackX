DROP PROCEDURE [dbo].[ListPostsTitles]
GO
CREATE PROCEDURE [dbo].[ListPostsTitles]
@UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.[Id], p.[Title], p.[CreatedAt], u.[Username], p.[UserId], p.[ViewCount],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [EntityType] = 'Post' AND [EntityId] = p.[Id] AND [IsDisliked] = 0) AS [LikeCount],
           (SELECT COUNT(*) FROM [dbo].[Replies] WHERE [PostId] = p.[Id] AND [IsDeleted] = 0) AS [ReplyCount],
           CASE
               WHEN EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = 'Post' AND [EntityId] = p.[Id] AND [UserId] = @UserId AND [IsDisliked] = 0)
                   THEN 1
               ELSE 0
               END AS [IsLiked]
    FROM [dbo].[Posts] p
             JOIN [AppUserSchema].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[IsDeleted] = 0
    ORDER BY p.[CreatedAt] DESC;
END;
