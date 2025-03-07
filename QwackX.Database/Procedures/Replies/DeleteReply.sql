DROP PROCEDURE [AppUserSchema].[DeleteReply]
GO
CREATE PROCEDURE [AppUserSchema].[DeleteReply]
    @ReplyId INT
AS
BEGIN
    UPDATE [dbo].[Replies]
    SET [IsDeleted] = 1
    WHERE [Id] = @ReplyId;
END;