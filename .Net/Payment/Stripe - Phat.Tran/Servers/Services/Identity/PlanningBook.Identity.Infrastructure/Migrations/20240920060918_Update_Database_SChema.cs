using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlanningBook.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Database_SChema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AccountTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Accounts",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Accounts",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<int>(
                name: "AppliedEntity",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId1",
                table: "AccountTokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AccountTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "AccountTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AccountTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpirationDate",
                table: "AccountTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AccountTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId1",
                table: "AccountRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId1",
                table: "AccountLogins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId1",
                table: "AccountClaims",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AccountPersons",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPersons", x => new { x.AccountId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_AccountPersons_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RevokedTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedTokens", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "AppliedEntity", "ConcurrencyStamp", "Name", "NormalizedName", "RoleType" },
                values: new object[,]
                {
                    { new Guid("281c5aa9-f0bd-42bc-9c9f-060895fb4187"), 1, null, "p_EndUser", "p_enduser", 4001 },
                    { new Guid("4155f55f-b7e7-4529-865b-63f2ea7865fa"), 0, null, "a_SysAdmin", "a_sysadmin", 1 },
                    { new Guid("a5a8bd75-c04b-4ad8-b1b7-b1db912ae8ef"), 1, null, "p_Staff", "p_staff", 3000 },
                    { new Guid("f8183db5-a09e-41d8-939b-c188a6247651"), 0, null, "a_User", "a_user", 1001 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTokens_AccountId1",
                table: "AccountTokens",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_AccountId1",
                table: "AccountRoles",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLogins_AccountId1",
                table: "AccountLogins",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_AccountClaims_AccountId1",
                table: "AccountClaims",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountClaims_Accounts_AccountId1",
                table: "AccountClaims",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLogins_Accounts_AccountId1",
                table: "AccountLogins",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId1",
                table: "AccountRoles",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTokens_Accounts_AccountId1",
                table: "AccountTokens",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountClaims_Accounts_AccountId1",
                table: "AccountClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountLogins_Accounts_AccountId1",
                table: "AccountLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId1",
                table: "AccountRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountTokens_Accounts_AccountId1",
                table: "AccountTokens");

            migrationBuilder.DropTable(
                name: "AccountPersons");

            migrationBuilder.DropTable(
                name: "RevokedTokens");

            migrationBuilder.DropIndex(
                name: "IX_AccountTokens_AccountId1",
                table: "AccountTokens");

            migrationBuilder.DropIndex(
                name: "IX_AccountRoles_AccountId1",
                table: "AccountRoles");

            migrationBuilder.DropIndex(
                name: "IX_AccountLogins_AccountId1",
                table: "AccountLogins");

            migrationBuilder.DropIndex(
                name: "IX_AccountClaims_AccountId1",
                table: "AccountClaims");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("281c5aa9-f0bd-42bc-9c9f-060895fb4187"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4155f55f-b7e7-4529-865b-63f2ea7865fa"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a5a8bd75-c04b-4ad8-b1b7-b1db912ae8ef"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f8183db5-a09e-41d8-939b-c188a6247651"));

            migrationBuilder.DropColumn(
                name: "AppliedEntity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpirationDate",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "AccountRoles");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "AccountLogins");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "AccountClaims");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AccountTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Accounts",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Accounts",
                newName: "CreatedById");
        }
    }
}
