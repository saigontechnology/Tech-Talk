using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanningBook.Themes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_seed_data_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("eea6122c-c5d8-40f1-a44a-766008241255"),
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rain", "Rain" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("eea6122c-c5d8-40f1-a44a-766008241255"),
                columns: new[] { "Description", "Name" },
                values: new object[] { "Black", "Black" });
        }
    }
}
