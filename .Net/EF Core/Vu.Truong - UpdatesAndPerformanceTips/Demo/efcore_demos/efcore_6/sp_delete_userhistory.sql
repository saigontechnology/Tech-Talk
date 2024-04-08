IF OBJECT_ID('sp_delete_userhistory', 'P') IS NOT NULL
DROP PROC [dbo].[sp_delete_userhistory]
GO

CREATE PROCEDURE [dbo].[sp_delete_userhistory]
AS
BEGIN
    DECLARE @sql VARCHAR(MAX)

    BEGIN TRANSACTION

        ALTER TABLE [dbo].[Users] SET ( SYSTEM_VERSIONING = OFF )

        SET @sql = 'DELETE FROM [dbo].[UsersHistory] WITH (TABLOCKX)'
        EXEC (@sql)

        ALTER TABLE [dbo].[Users] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UsersHistory]))

    COMMIT TRANSACTION
END
GO