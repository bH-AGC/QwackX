DROP PROCEDURE IF EXISTS [dbo].[DetailPost];
GO
CREATE PROCEDURE [AppUserSchema].[DetailPost]
    @PostId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.[Id],
        p.[Title],
        p.[Description],
        p.[CreatedAt],
        u.[Username],
        p.[UserId],
        -- Nombre de likes
        (SELECT COUNT(*)
         FROM [dbo].[Likes]
         WHERE [EntityType] = 'Post'
           AND [EntityId] = p.[Id]
           AND [IsDisliked] = 0) AS [LikeCount],
        -- Indicateur si l'utilisateur a aimé le post
        CASE
            WHEN EXISTS (SELECT 1
                         FROM [dbo].[Likes]
                         WHERE [EntityType] = 'Post'
                           AND [EntityId] = p.[Id]
                           AND [UserId] = @UserId
                           AND [IsDisliked] = 0)
                THEN 1
            ELSE 0
            END AS [IsLiked],
        -- Nombre de vues
        p.[ViewCount],
        -- Indicateur si l'utilisateur a déjà vu ce post
        CASE
            WHEN EXISTS (SELECT 1
                         FROM [dbo].[PostViews]
                         WHERE [PostId] = p.[Id]
                           AND [UserId] = @UserId)
                THEN 1
            ELSE 0
            END AS [HasViewed]
    FROM [dbo].[Posts] p
             JOIN [dbo].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[Id] = @PostId AND p.[IsDeleted] = 0;
END;
