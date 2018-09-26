using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoCosting.Data.Migrations
{
    public partial class addSedeToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSede",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpresaID = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Direccion = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    UsarCierreCajaYN = table.Column<bool>(nullable: false),
                    ImprimirLogoYN = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSede", x => new { x.ID, x.EmpresaID });
                    table.UniqueConstraint("AK_tblSede_EmpresaID_ID", x => new { x.EmpresaID, x.ID });
                    table.ForeignKey(
                        name: "FK_tblSede_tblEmpresa_EmpresaID",
                        column: x => x.EmpresaID,
                        principalTable: "tblEmpresa",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSede");
        }
    }
}
