DROP PROCEDURE IF EXISTS [dbo].[PostsViewsBulkInsert];
GO
CREATE PROCEDURE [dbo].[PostsViewsBulkInsert]
    @PostViews dbo.PostViewType READONLY,
    @RowsAffected INT OUTPUT -- Paramètre de sortie
AS
BEGIN
    SET NOCOUNT ON;

    -- Initialisation du paramètre de sortie
    SET @RowsAffected = 0;

    MERGE INTO dbo.PostViews AS target
    USING (SELECT DISTINCT PostId, UserId, ViewedAt FROM @PostViews) AS source
    ON target.PostId = source.PostId AND target.UserId = source.UserId
    WHEN MATCHED THEN
        UPDATE SET target.ViewedAt = source.ViewedAt
    WHEN NOT MATCHED THEN
        INSERT (PostId, UserId, ViewedAt)
        VALUES (source.PostId, source.UserId, source.ViewedAt);

    -- Mettre à jour le nombre de lignes affectées
    SET @RowsAffected = (SELECT COUNT(*) FROM dbo.PostViews WHERE PostId IN (SELECT PostId FROM @PostViews));
END;
