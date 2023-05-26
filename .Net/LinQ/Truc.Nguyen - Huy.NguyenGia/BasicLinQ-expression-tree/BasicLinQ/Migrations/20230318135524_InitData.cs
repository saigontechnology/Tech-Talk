using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicLinQ.Migrations
{
    public partial class InitData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "supplier 1" },
                    { 2, "supplier 2" },
                    { 3, "supplier 3" },
                    { 4, "supplier 4" },
                    { 5, "supplier 5" },
                    { 6, "supplier 6" },
                    { 7, "supplier 7" },
                    { 8, "supplier 8" },
                    { 9, "supplier 9" },
                    { 10, "supplier 10" },
                    { 11, "supplier 11" },
                    { 12, "supplier 12" },
                    { 13, "supplier 13" },
                    { 14, "supplier 14" },
                    { 15, "supplier 15" },
                    { 16, "supplier 16" },
                    { 17, "supplier 17" },
                    { 18, "supplier 18" },
                    { 19, "supplier 19" },
                    { 20, "supplier 20" },
                    { 21, "supplier 21" },
                    { 22, "supplier 22" },
                    { 23, "supplier 23" },
                    { 24, "supplier 24" },
                    { 25, "supplier 25" },
                    { 26, "supplier 26" },
                    { 27, "supplier 27" },
                    { 28, "supplier 28" },
                    { 29, "supplier 29" },
                    { 30, "supplier 30" },
                    { 31, "supplier 31" },
                    { 32, "supplier 32" },
                    { 33, "supplier 33" },
                    { 34, "supplier 34" },
                    { 35, "supplier 35" },
                    { 36, "supplier 36" },
                    { 37, "supplier 37" },
                    { 38, "supplier 38" },
                    { 39, "supplier 39" },
                    { 40, "supplier 40" },
                    { 41, "supplier 41" },
                    { 42, "supplier 42" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 43, "supplier 43" },
                    { 44, "supplier 44" },
                    { 45, "supplier 45" },
                    { 46, "supplier 46" },
                    { 47, "supplier 47" },
                    { 48, "supplier 48" },
                    { 49, "supplier 49" },
                    { 50, "supplier 50" },
                    { 51, "supplier 51" },
                    { 52, "supplier 52" },
                    { 53, "supplier 53" },
                    { 54, "supplier 54" },
                    { 55, "supplier 55" },
                    { 56, "supplier 56" },
                    { 57, "supplier 57" },
                    { 58, "supplier 58" },
                    { 59, "supplier 59" },
                    { 60, "supplier 60" },
                    { 61, "supplier 61" },
                    { 62, "supplier 62" },
                    { 63, "supplier 63" },
                    { 64, "supplier 64" },
                    { 65, "supplier 65" },
                    { 66, "supplier 66" },
                    { 67, "supplier 67" },
                    { 68, "supplier 68" },
                    { 69, "supplier 69" },
                    { 70, "supplier 70" },
                    { 71, "supplier 71" },
                    { 72, "supplier 72" },
                    { 73, "supplier 73" },
                    { 74, "supplier 74" },
                    { 75, "supplier 75" },
                    { 76, "supplier 76" },
                    { 77, "supplier 77" },
                    { 78, "supplier 78" },
                    { 79, "supplier 79" },
                    { 80, "supplier 80" },
                    { 81, "supplier 81" },
                    { 82, "supplier 82" },
                    { 83, "supplier 83" },
                    { 84, "supplier 84" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 85, "supplier 85" },
                    { 86, "supplier 86" },
                    { 87, "supplier 87" },
                    { 88, "supplier 88" },
                    { 89, "supplier 89" },
                    { 90, "supplier 90" },
                    { 91, "supplier 91" },
                    { 92, "supplier 92" },
                    { 93, "supplier 93" },
                    { 94, "supplier 94" },
                    { 95, "supplier 95" },
                    { 96, "supplier 96" },
                    { 97, "supplier 97" },
                    { 98, "supplier 98" },
                    { 99, "supplier 99" },
                    { 100, "supplier 100" },
                    { 101, "supplier 101" },
                    { 102, "supplier 102" },
                    { 103, "supplier 103" },
                    { 104, "supplier 104" },
                    { 105, "supplier 105" },
                    { 106, "supplier 106" },
                    { 107, "supplier 107" },
                    { 108, "supplier 108" },
                    { 109, "supplier 109" },
                    { 110, "supplier 110" },
                    { 111, "supplier 111" },
                    { 112, "supplier 112" },
                    { 113, "supplier 113" },
                    { 114, "supplier 114" },
                    { 115, "supplier 115" },
                    { 116, "supplier 116" },
                    { 117, "supplier 117" },
                    { 118, "supplier 118" },
                    { 119, "supplier 119" },
                    { 120, "supplier 120" },
                    { 121, "supplier 121" },
                    { 122, "supplier 122" },
                    { 123, "supplier 123" },
                    { 124, "supplier 124" },
                    { 125, "supplier 125" },
                    { 126, "supplier 126" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 127, "supplier 127" },
                    { 128, "supplier 128" },
                    { 129, "supplier 129" },
                    { 130, "supplier 130" },
                    { 131, "supplier 131" },
                    { 132, "supplier 132" },
                    { 133, "supplier 133" },
                    { 134, "supplier 134" },
                    { 135, "supplier 135" },
                    { 136, "supplier 136" },
                    { 137, "supplier 137" },
                    { 138, "supplier 138" },
                    { 139, "supplier 139" },
                    { 140, "supplier 140" },
                    { 141, "supplier 141" },
                    { 142, "supplier 142" },
                    { 143, "supplier 143" },
                    { 144, "supplier 144" },
                    { 145, "supplier 145" },
                    { 146, "supplier 146" },
                    { 147, "supplier 147" },
                    { 148, "supplier 148" },
                    { 149, "supplier 149" },
                    { 150, "supplier 150" },
                    { 151, "supplier 151" },
                    { 152, "supplier 152" },
                    { 153, "supplier 153" },
                    { 154, "supplier 154" },
                    { 155, "supplier 155" },
                    { 156, "supplier 156" },
                    { 157, "supplier 157" },
                    { 158, "supplier 158" },
                    { 159, "supplier 159" },
                    { 160, "supplier 160" },
                    { 161, "supplier 161" },
                    { 162, "supplier 162" },
                    { 163, "supplier 163" },
                    { 164, "supplier 164" },
                    { 165, "supplier 165" },
                    { 166, "supplier 166" },
                    { 167, "supplier 167" },
                    { 168, "supplier 168" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 169, "supplier 169" },
                    { 170, "supplier 170" },
                    { 171, "supplier 171" },
                    { 172, "supplier 172" },
                    { 173, "supplier 173" },
                    { 174, "supplier 174" },
                    { 175, "supplier 175" },
                    { 176, "supplier 176" },
                    { 177, "supplier 177" },
                    { 178, "supplier 178" },
                    { 179, "supplier 179" },
                    { 180, "supplier 180" },
                    { 181, "supplier 181" },
                    { 182, "supplier 182" },
                    { 183, "supplier 183" },
                    { 184, "supplier 184" },
                    { 185, "supplier 185" },
                    { 186, "supplier 186" },
                    { 187, "supplier 187" },
                    { 188, "supplier 188" },
                    { 189, "supplier 189" },
                    { 190, "supplier 190" },
                    { 191, "supplier 191" },
                    { 192, "supplier 192" },
                    { 193, "supplier 193" },
                    { 194, "supplier 194" },
                    { 195, "supplier 195" },
                    { 196, "supplier 196" },
                    { 197, "supplier 197" },
                    { 198, "supplier 198" },
                    { 199, "supplier 199" },
                    { 200, "supplier 200" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "Name", "SupplierId" },
                values: new object[,]
                {
                    { 1, "fruit", 1 },
                    { 2, "electronic", 2 },
                    { 3, "household", 3 },
                    { 4, "misc", 4 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Name", "SupplierId" },
                values: new object[,]
                {
                    { 1, 1, "apple", 1 },
                    { 2, 1, "banana", 1 },
                    { 3, 1, "orange", 1 },
                    { 4, 1, "grape", 1 },
                    { 5, 1, "mango", 1 },
                    { 6, 2, "television", 2 },
                    { 7, 2, "laptop", 2 },
                    { 8, 2, "keyboard", 2 },
                    { 9, 2, "monitor", 2 },
                    { 10, 3, "knife", 3 },
                    { 11, 3, "spoon", 3 },
                    { 12, 3, "bowl", 3 },
                    { 13, 3, "chopsticks", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_SupplierId",
                table: "ProductCategories",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
