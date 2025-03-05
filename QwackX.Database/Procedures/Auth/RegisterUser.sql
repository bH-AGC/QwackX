DROP PROCEDURE [AppUserSchema].[RegisterUser]
GO
CREATE PROCEDURE [AppUserSchema].[RegisterUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @Password NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        IF LEN(TRIM(@Username)) = 0
            BEGIN
                RAISERROR (N'Invalid value in @Username', 16, 1);
                RETURN;
            END

        IF LEN(TRIM(@Email)) = 0 OR CHARINDEX('@', @Email) = 0
            BEGIN
                RAISERROR (N'Invalid value in @Email', 16, 1);
                RETURN;
            END

        IF LEN(@Password) < 6
            BEGIN
                RAISERROR (N'Password must be at least 6 characters long', 16, 1);
                RETURN;
            END

        INSERT INTO [dbo].[Users] (Username, Email, PasswordHash)
        VALUES (@Username, @Email, dbo.CreatePasswd(@Password));

        SELECT Id, Username, Email, CreatedAt
        FROM [dbo].[Users]
        WHERE Email = @Email;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END;
