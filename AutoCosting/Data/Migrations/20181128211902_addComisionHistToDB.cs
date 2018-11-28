using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class addComisionHistToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblComisionHist",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    TransID = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Monto = table.Column<double>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    TipoComision = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblComisionHist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblComisionHist_tblTransHeaderHist_TransID",
                        column: x => x.TransID,
                        principalTable: "tblTransHeaderHist",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblComisionHist_TransID",
                table: "tblComisionHist",
                column: "TransID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblComisionHist");
        }
    }
}
