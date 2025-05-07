using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlanningBook.Themes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[] { "Id", "Description", "Name", "Price", "Type" },
                values: new object[,]
                {
                    { new Guid("1fc13c2a-27e8-45cb-95dd-9dfd924db840"), "Lifetime", "Lifetime", 710m, 0 },
                    { new Guid("371cffea-2b1e-4c4d-aec8-cffd1ae43fef"), "Basic", "Basic", 150m, 0 },
                    { new Guid("b677c4c6-669b-43ca-9897-83b5cb1c0cd9"), "Elite", "Elite", 294m, 0 }
                });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("27784869-292e-47e8-be5f-311d7a4aaf14"), "White", "White", 250m },
                    { new Guid("835bc37f-8891-48da-9f01-4bfcbf50ab13"), "Black", "Black", 150m },
                    { new Guid("eea6122c-c5d8-40f1-a44a-766008241255"), "Black", "Black", 400m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: new Guid("1fc13c2a-27e8-45cb-95dd-9dfd924db840"));

            migrationBuilder.DeleteData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: new Guid("371cffea-2b1e-4c4d-aec8-cffd1ae43fef"));

            migrationBuilder.DeleteData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: new Guid("b677c4c6-669b-43ca-9897-83b5cb1c0cd9"));

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("27784869-292e-47e8-be5f-311d7a4aaf14"));

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("835bc37f-8891-48da-9f01-4bfcbf50ab13"));

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("eea6122c-c5d8-40f1-a44a-766008241255"));
        }
    }
}
