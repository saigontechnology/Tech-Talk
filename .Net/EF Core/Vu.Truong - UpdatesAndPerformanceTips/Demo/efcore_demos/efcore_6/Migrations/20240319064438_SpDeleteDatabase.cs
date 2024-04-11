using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcore_demos.Migrations
{
    public partial class SpDeleteDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
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
            ");

            migrationBuilder.Sql($@"
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
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                IF OBJECT_ID('sp_delete_userhistory', 'P') IS NOT NULL
                DROP PROC [dbo].[sp_delete_userhistory]
                GO
            ");

            migrationBuilder.Sql($@"
                IF OBJECT_ID('sp_clean_database', 'P') IS NOT NULL
                DROP PROC [dbo].[sp_clean_database]
                GO
            ");
        }
    }
}
