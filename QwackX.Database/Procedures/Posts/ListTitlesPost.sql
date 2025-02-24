CREATE PROCEDURE [dbo].[ListPostsTitles]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.[Id], p.[Title], p.[CreatedAt], u.[Username], p.[UserId],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [PostId] = p.[Id]) AS [LikeCount],
           (SELECT COUNT(*) FROM [dbo].[Replies] WHERE [PostId] = p.[Id] AND [IsDeleted] = 0) AS [ReplyCount]
    FROM [dbo].[Posts] p
             JOIN [AppUserSchema].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[IsDeleted] = 0
    ORDER BY p.[CreatedAt] DESC;
END;
