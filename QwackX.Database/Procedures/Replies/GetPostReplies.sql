CREATE PROCEDURE [dbo].[GetPostReplies]
    @PostId INT
AS
BEGIN
    SELECT r.[Id], r.[Content], r.[CreatedAt], u.[Username]
    FROM [dbo].[Replies] r
             JOIN [AppUserSchema].[Users] u ON r.[UserId] = u.[Id]
    WHERE r.[PostId] = @PostId AND r.[IsDeleted] = 0;
END;