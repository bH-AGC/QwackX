DROP PROCEDURE [AppUserSchema].[CreateReply]
GO
CREATE PROCEDURE [AppUserSchema].[CreateReply]
    @PostId INT,
    @UserId INT,
    @Content NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO [dbo].[Replies] ([PostId], [UserId], [Content])
    VALUES (@PostId, @UserId, @Content);
END;