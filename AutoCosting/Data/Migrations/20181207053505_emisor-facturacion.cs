using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class emisorfacturacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEmisor",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TipoIdentificacion = table.Column<string>(nullable: false),
                    NumeroIdentificacion = table.Column<string>(nullable: false),
                    Provincia = table.Column<string>(nullable: false),
                    Canton = table.Column<string>(nullable: false),
                    Distrito = table.Column<string>(nullable: false),
                    Barrio = table.Column<string>(nullable: false),
                    OtrasSenas = table.Column<string>(nullable: false),
                    CodigoPaisTelefono = table.Column<string>(nullable: false, defaultValue: "506"),
                    NumeroTelefono = table.Column<string>(nullable: false),
                    CorreoElectronico = table.Column<string>(nullable: false),
                    RutaArchivoCertificado = table.Column<string>(nullable: false),
                    PinCertificado = table.Column<string>(nullable: false),
                    UsuarioApi = table.Column<string>(nullable: false),
                    ClaveApi = table.Column<string>(nullable: false),
                    OutputFolder = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmisor", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEmisor");
        }
    }
}
