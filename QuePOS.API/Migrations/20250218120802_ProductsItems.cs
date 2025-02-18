using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuePOS.API.Migrations
{
    /// <inheritdoc />
    public partial class ProductsItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Base_unit_bc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mp_prod_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seg1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seg2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seg3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Var = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Man = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sub_brn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc_sourced = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc_algo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Base_unit_count = table.Column<int>(type: "int", nullable: true),
                    Vol_base_unit = table.Column<double>(type: "float", nullable: true),
                    Tot_vol = table.Column<double>(type: "float", nullable: true),
                    Uom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItems");
        }
    }
}
