using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcore_demos.Migrations
{
    /// <inheritdoc />
    public partial class PrimitiveCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DaysVisited",
                table: "Users",
                type: "varchar(4028)",
                unicode: false,
                maxLength: 4028,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderHistories",
                table: "Users",
                type: "varchar(4028)",
                unicode: false,
                maxLength: 4028,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysVisited",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrderHistories",
                table: "Users");
        }
    }
}
