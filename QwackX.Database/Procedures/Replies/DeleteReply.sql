CREATE PROCEDURE [dbo].[DeleteReply]
    @ReplyId INT
AS
BEGIN
    UPDATE [dbo].[Replies]
    SET [IsDeleted] = 1
    WHERE [Id] = @ReplyId;
END;