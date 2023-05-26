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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, false, "supplier 1" },
                    { 2, false, "supplier 2" },
                    { 3, false, "supplier 3" },
                    { 4, false, "supplier 4" },
                    { 5, false, "supplier 5" },
                    { 6, false, "supplier 6" },
                    { 7, false, "supplier 7" },
                    { 8, false, "supplier 8" },
                    { 9, false, "supplier 9" },
                    { 10, false, "supplier 10" },
                    { 11, false, "supplier 11" },
                    { 12, false, "supplier 12" },
                    { 13, false, "supplier 13" },
                    { 14, false, "supplier 14" },
                    { 15, false, "supplier 15" },
                    { 16, false, "supplier 16" },
                    { 17, false, "supplier 17" },
                    { 18, false, "supplier 18" },
                    { 19, false, "supplier 19" },
                    { 20, false, "supplier 20" },
                    { 21, false, "supplier 21" },
                    { 22, false, "supplier 22" },
                    { 23, false, "supplier 23" },
                    { 24, false, "supplier 24" },
                    { 25, false, "supplier 25" },
                    { 26, false, "supplier 26" },
                    { 27, false, "supplier 27" },
                    { 28, false, "supplier 28" },
                    { 29, false, "supplier 29" },
                    { 30, false, "supplier 30" },
                    { 31, false, "supplier 31" },
                    { 32, false, "supplier 32" },
                    { 33, false, "supplier 33" },
                    { 34, false, "supplier 34" },
                    { 35, false, "supplier 35" },
                    { 36, false, "supplier 36" },
                    { 37, false, "supplier 37" },
                    { 38, false, "supplier 38" },
                    { 39, false, "supplier 39" },
                    { 40, false, "supplier 40" },
                    { 41, false, "supplier 41" },
                    { 42, false, "supplier 42" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 43, false, "supplier 43" },
                    { 44, false, "supplier 44" },
                    { 45, false, "supplier 45" },
                    { 46, false, "supplier 46" },
                    { 47, false, "supplier 47" },
                    { 48, false, "supplier 48" },
                    { 49, false, "supplier 49" },
                    { 50, false, "supplier 50" },
                    { 51, false, "supplier 51" },
                    { 52, false, "supplier 52" },
                    { 53, false, "supplier 53" },
                    { 54, false, "supplier 54" },
                    { 55, false, "supplier 55" },
                    { 56, false, "supplier 56" },
                    { 57, false, "supplier 57" },
                    { 58, false, "supplier 58" },
                    { 59, false, "supplier 59" },
                    { 60, false, "supplier 60" },
                    { 61, false, "supplier 61" },
                    { 62, false, "supplier 62" },
                    { 63, false, "supplier 63" },
                    { 64, false, "supplier 64" },
                    { 65, false, "supplier 65" },
                    { 66, false, "supplier 66" },
                    { 67, false, "supplier 67" },
                    { 68, false, "supplier 68" },
                    { 69, false, "supplier 69" },
                    { 70, false, "supplier 70" },
                    { 71, false, "supplier 71" },
                    { 72, false, "supplier 72" },
                    { 73, false, "supplier 73" },
                    { 74, false, "supplier 74" },
                    { 75, false, "supplier 75" },
                    { 76, false, "supplier 76" },
                    { 77, false, "supplier 77" },
                    { 78, false, "supplier 78" },
                    { 79, false, "supplier 79" },
                    { 80, false, "supplier 80" },
                    { 81, false, "supplier 81" },
                    { 82, false, "supplier 82" },
                    { 83, false, "supplier 83" },
                    { 84, false, "supplier 84" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 85, false, "supplier 85" },
                    { 86, false, "supplier 86" },
                    { 87, false, "supplier 87" },
                    { 88, false, "supplier 88" },
                    { 89, false, "supplier 89" },
                    { 90, false, "supplier 90" },
                    { 91, false, "supplier 91" },
                    { 92, false, "supplier 92" },
                    { 93, false, "supplier 93" },
                    { 94, false, "supplier 94" },
                    { 95, false, "supplier 95" },
                    { 96, false, "supplier 96" },
                    { 97, false, "supplier 97" },
                    { 98, false, "supplier 98" },
                    { 99, false, "supplier 99" },
                    { 100, false, "supplier 100" },
                    { 101, false, "supplier 101" },
                    { 102, false, "supplier 102" },
                    { 103, false, "supplier 103" },
                    { 104, false, "supplier 104" },
                    { 105, false, "supplier 105" },
                    { 106, false, "supplier 106" },
                    { 107, false, "supplier 107" },
                    { 108, false, "supplier 108" },
                    { 109, false, "supplier 109" },
                    { 110, false, "supplier 110" },
                    { 111, false, "supplier 111" },
                    { 112, false, "supplier 112" },
                    { 113, false, "supplier 113" },
                    { 114, false, "supplier 114" },
                    { 115, false, "supplier 115" },
                    { 116, false, "supplier 116" },
                    { 117, false, "supplier 117" },
                    { 118, false, "supplier 118" },
                    { 119, false, "supplier 119" },
                    { 120, false, "supplier 120" },
                    { 121, false, "supplier 121" },
                    { 122, false, "supplier 122" },
                    { 123, false, "supplier 123" },
                    { 124, false, "supplier 124" },
                    { 125, false, "supplier 125" },
                    { 126, false, "supplier 126" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 127, false, "supplier 127" },
                    { 128, false, "supplier 128" },
                    { 129, false, "supplier 129" },
                    { 130, false, "supplier 130" },
                    { 131, false, "supplier 131" },
                    { 132, false, "supplier 132" },
                    { 133, false, "supplier 133" },
                    { 134, false, "supplier 134" },
                    { 135, false, "supplier 135" },
                    { 136, false, "supplier 136" },
                    { 137, false, "supplier 137" },
                    { 138, false, "supplier 138" },
                    { 139, false, "supplier 139" },
                    { 140, false, "supplier 140" },
                    { 141, false, "supplier 141" },
                    { 142, false, "supplier 142" },
                    { 143, false, "supplier 143" },
                    { 144, false, "supplier 144" },
                    { 145, false, "supplier 145" },
                    { 146, false, "supplier 146" },
                    { 147, false, "supplier 147" },
                    { 148, false, "supplier 148" },
                    { 149, false, "supplier 149" },
                    { 150, false, "supplier 150" },
                    { 151, false, "supplier 151" },
                    { 152, false, "supplier 152" },
                    { 153, false, "supplier 153" },
                    { 154, false, "supplier 154" },
                    { 155, false, "supplier 155" },
                    { 156, false, "supplier 156" },
                    { 157, false, "supplier 157" },
                    { 158, false, "supplier 158" },
                    { 159, false, "supplier 159" },
                    { 160, false, "supplier 160" },
                    { 161, false, "supplier 161" },
                    { 162, false, "supplier 162" },
                    { 163, false, "supplier 163" },
                    { 164, false, "supplier 164" },
                    { 165, false, "supplier 165" },
                    { 166, false, "supplier 166" },
                    { 167, false, "supplier 167" },
                    { 168, false, "supplier 168" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 169, false, "supplier 169" },
                    { 170, false, "supplier 170" },
                    { 171, false, "supplier 171" },
                    { 172, false, "supplier 172" },
                    { 173, false, "supplier 173" },
                    { 174, false, "supplier 174" },
                    { 175, false, "supplier 175" },
                    { 176, false, "supplier 176" },
                    { 177, false, "supplier 177" },
                    { 178, false, "supplier 178" },
                    { 179, false, "supplier 179" },
                    { 180, false, "supplier 180" },
                    { 181, false, "supplier 181" },
                    { 182, false, "supplier 182" },
                    { 183, false, "supplier 183" },
                    { 184, false, "supplier 184" },
                    { 185, false, "supplier 185" },
                    { 186, false, "supplier 186" },
                    { 187, false, "supplier 187" },
                    { 188, false, "supplier 188" },
                    { 189, false, "supplier 189" },
                    { 190, false, "supplier 190" },
                    { 191, false, "supplier 191" },
                    { 192, false, "supplier 192" },
                    { 193, false, "supplier 193" },
                    { 194, false, "supplier 194" },
                    { 195, false, "supplier 195" },
                    { 196, false, "supplier 196" },
                    { 197, false, "supplier 197" },
                    { 198, false, "supplier 198" },
                    { 199, false, "supplier 199" },
                    { 200, false, "supplier 200" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "IsDeleted", "Name", "SupplierId" },
                values: new object[,]
                {
                    { 1, false, "fruit", 1 },
                    { 2, false, "electronic", 2 },
                    { 3, false, "household", 3 },
                    { 4, false, "misc", 4 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "IsDeleted", "Name", "SupplierId" },
                values: new object[,]
                {
                    { 1, 1, false, "apple", 1 },
                    { 2, 1, false, "banana", 1 },
                    { 3, 1, false, "orange", 1 },
                    { 4, 1, false, "grape", 1 },
                    { 5, 1, false, "mango", 1 },
                    { 6, 2, false, "television", 2 },
                    { 7, 2, false, "laptop", 2 },
                    { 8, 2, false, "keyboard", 2 },
                    { 9, 2, false, "monitor", 2 },
                    { 10, 3, false, "knife", 3 },
                    { 11, 3, false, "spoon", 3 },
                    { 12, 3, false, "bowl", 3 },
                    { 13, 3, false, "chopsticks", 3 }
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
