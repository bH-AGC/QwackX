DROP PROCEDURE IF EXISTS [AppUserSchema].[PostsViewsBulkInsert];
GO
CREATE PROCEDURE [AppUserSchema].[PostsViewsBulkInsert]
    @PostViews [AppUserSchema].PostViewType READONLY,
    @RowsAffected INT OUTPUT -- Paramètre de sortie
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    BEGIN TRY
        -- Initialisation du paramètre de sortie
        SET @RowsAffected = 0;

        -- Vérifier si des PostId n'existent pas dans dbo.Posts
        IF EXISTS (
            SELECT 1 FROM @PostViews pv
                              LEFT JOIN dbo.Posts p ON p.Id = pv.PostId
            WHERE p.Id IS NULL
        )
            BEGIN
                -- Log des PostId qui posent problème
                SELECT pv.PostId
                FROM @PostViews pv
                         LEFT JOIN dbo.Posts p ON p.Id = pv.PostId
                WHERE p.Id IS NULL;

                -- Lever une erreur explicite
                THROW 50000, 'Un ou plusieurs PostId n''existent pas dans la table dbo.Posts.', 1;
            END

        -- MERGE : Met à jour ou insère les vues dans la table PostViews
        MERGE INTO [dbo].[PostViews] AS target
        USING (SELECT DISTINCT PostId, UserId, ViewedAt FROM @PostViews) AS source
        ON target.PostId = source.PostId AND target.UserId = source.UserId
        WHEN MATCHED THEN
            UPDATE SET target.ViewedAt = source.ViewedAt
        WHEN NOT MATCHED THEN
            INSERT (PostId, UserId, ViewedAt)
            VALUES (source.PostId, source.UserId, source.ViewedAt);

        -- Mettre à jour le ViewCount dans dbo.Posts après l'insertion/mise à jour de PostViews
        UPDATE p
        SET p.ViewCount = p.ViewCount + 1
        FROM [dbo].[Posts] p
                 INNER JOIN @PostViews pv ON p.Id = pv.PostId;

        -- Mettre à jour le nombre de lignes affectées
        SET @RowsAffected = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        -- Récupération de l'erreur SQL
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Log des erreurs
        PRINT '❌ Erreur lors de l''insertion : ' + @ErrorMessage;

        -- Renvoyer l'erreur
        THROW;
    END CATCH
END;
