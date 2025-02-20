CREATE PROCEDURE [dbo].[GetPostsTitles]
AS
BEGIN
    SELECT [Id], [Title]
    FROM [dbo].[Posts]
    WHERE [IsDeleted] = 0;
END;