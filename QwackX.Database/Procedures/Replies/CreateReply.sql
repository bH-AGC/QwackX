DROP PROCEDURE [dbo].[CreateReply]
GO
CREATE PROCEDURE [dbo].[CreateReply]
    @PostId INT,
    @UserId INT,
    @Content NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Replies] ([PostId], [UserId], [Content])
    VALUES (@PostId, @UserId, @Content);
END;