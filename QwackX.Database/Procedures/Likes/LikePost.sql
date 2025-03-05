DROP PROCEDURE [AppUserSchema].[LikePost]
GO
CREATE PROCEDURE [AppUserSchema].[LikePost]
    @UserId INT,
    @EntityId INT,
    @EntityType NVARCHAR(50)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Likes] WHERE [EntityType] = @EntityType AND [EntityId] = @EntityId AND [UserId] = @UserId)
        BEGIN
            INSERT INTO [dbo].[Likes] ([EntityType], [EntityId], [UserId])
            VALUES (@EntityType, @EntityId, @UserId);
        END
    ELSE
        BEGIN
            UPDATE [dbo].[Likes]
            SET [IsDisliked] = CASE WHEN [IsDisliked] = 1 THEN 0 ELSE 1 END
            WHERE [EntityType] = @EntityType AND [EntityId] = @EntityId AND [UserId] = @UserId;
        END
END;
