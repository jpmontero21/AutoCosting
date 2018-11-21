using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class addComisionToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblComision",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransID = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Monto = table.Column<double>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    TipoComision = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblComision", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblComision_tblTransHeader_TransID",
                        column: x => x.TransID,
                        principalTable: "tblTransHeader",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblComision_TransID",
                table: "tblComision",
                column: "TransID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblComision");
        }
    }
}
