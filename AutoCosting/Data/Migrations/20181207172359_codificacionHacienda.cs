using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class codificacionHacienda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodificacionMH",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    idProvincia = table.Column<string>(nullable: true),
                    nombreProvincia = table.Column<string>(nullable: true),
                    idCanton = table.Column<string>(nullable: true),
                    nombreCanton = table.Column<string>(nullable: true),
                    idDistrito = table.Column<string>(nullable: true),
                    nombreDistrito = table.Column<string>(nullable: true),
                    idBarrio = table.Column<string>(nullable: true),
                    nombreBarrio = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodificacionMH", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodificacionMH");
        }
    }
}
