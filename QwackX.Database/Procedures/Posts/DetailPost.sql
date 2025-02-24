CREATE PROCEDURE [dbo].[DetailPost]
@PostId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.[Id], p.[Title], p.[Description], p.[CreatedAt], u.[Username], p.[UserId],
           (SELECT COUNT(*) FROM [dbo].[Likes] WHERE [PostId] = p.[Id]) AS [LikeCount]
    FROM [dbo].[Posts] p
             JOIN [AppUserSchema].[Users] u ON p.[UserId] = u.[Id]
    WHERE p.[Id] = @PostId AND p.[IsDeleted] = 0;
END;
