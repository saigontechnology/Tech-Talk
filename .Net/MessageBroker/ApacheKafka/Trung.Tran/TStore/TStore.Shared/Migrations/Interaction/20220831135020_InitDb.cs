using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace TStore.Shared.Migrations.Interaction
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InteractionReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    FromPage = table.Column<string>(nullable: true),
                    ToPage = table.Column<string>(nullable: true),
                    SearchTerm = table.Column<string>(nullable: true),
                    ClickCount = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Time = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InteractionReports");

            migrationBuilder.DropTable(
                name: "Interactions");
        }
    }
}
