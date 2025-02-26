CREATE PROCEDURE [dbo].[ListPostReplies]
@PostId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [EntityType] = 'Reply' AND [EntityId] = r.[Id] AND [IsDisliked] = 0) AS [LikeCount]
    FROM [dbo].[Replies] r
             JOIN [AppUserSchema].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;
