CREATE PROCEDURE [AppUserSchema].[CreateUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        IF LEN(TRIM(@Username)) = 0
            BEGIN
                RAISERROR (N'Invalid value in @Nom', 16, 1);
                RETURN;
            END

        INSERT INTO AppUserSchema.Users (Username, Email, PasswordHash)
        VALUES (@Username, @Email, dbo.CreatePasswd(@PasswordHash));
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN
    END CATCH
END;

-- BEGIN
--     INSERT INTO [AppUserSchema].[Users] ([Username], [Email], [PasswordHash])
--     VALUES (@Username, @Email, @PasswordHash);
-- END;