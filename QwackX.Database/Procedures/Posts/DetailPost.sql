DROP PROCEDURE [dbo].[DetailPost]
GO
CREATE PROCEDURE [dbo].[DetailPost]
    @PostId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.[Id], p.[Title], p.[Description], p.[CreatedAt], u.[Username], p.[UserId],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [EntityType] = 'Post' AND [EntityId] = p.[Id] AND [IsDisliked] = 0) AS [LikeCount],
           CASE
               WHEN EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = 'Post' AND [EntityId] = p.[Id] AND [UserId] = @UserId AND [IsDisliked] = 0)
                   THEN 1
               ELSE 0
               END AS [IsLiked]
    FROM [dbo].[Posts] p
             JOIN [AppUserSchema].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[Id] = @PostId AND p.[IsDeleted] = 0;
END;
