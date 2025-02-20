CREATE PROCEDURE [dbo].[GetPostDetails]
    @PostId INT
AS
BEGIN
    -- Détails du post
    SELECT p.[Id], p.[Title], p.[Description], p.[CreatedAt], u.[Username],
           (SELECT COUNT(*)
            FROM [dbo].[Likes] WHERE [PostId] = p.[Id]) AS [LikeCount]
    FROM [dbo].[Posts] p
             JOIN [AppUserSchema].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[Id] = @PostId AND p.[IsDeleted] = 0;

    -- Réponses du post
    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username]
    FROM [dbo].[Replies] r
             JOIN [AppUserSchema].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;