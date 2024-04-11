IF OBJECT_ID('sp_clean_database', 'P') IS NOT NULL
DROP PROC [dbo].[sp_clean_database]
GO

CREATE PROCEDURE [dbo].[sp_clean_database]
AS
BEGIN
    DECLARE @sql VARCHAR(MAX)

    BEGIN TRANSACTION

        ALTER TABLE [dbo].[Users] SET ( SYSTEM_VERSIONING = OFF )

		TRUNCATE TABLE [dbo].[Users]

		ALTER TABLE [dbo].[Users] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UsersHistory]))

		EXEC [dbo].[sp_delete_userhistory]

		TRUNCATE TABLE [dbo].[Products]

    COMMIT TRANSACTION
END
GO