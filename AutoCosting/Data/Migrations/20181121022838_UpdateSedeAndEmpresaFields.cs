using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class UpdateSedeAndEmpresaFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImprimirLogoYN",
                table: "tblSede");

            migrationBuilder.DropColumn(
                name: "UsarCierreCajaYN",
                table: "tblSede");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "tblSede",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreContacto",
                table: "tblSede",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ImprimirLogoYN",
                table: "tblEmpresa",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "tblSede");

            migrationBuilder.DropColumn(
                name: "NombreContacto",
                table: "tblSede");

            migrationBuilder.DropColumn(
                name: "ImprimirLogoYN",
                table: "tblEmpresa");

            migrationBuilder.AddColumn<bool>(
                name: "ImprimirLogoYN",
                table: "tblSede",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsarCierreCajaYN",
                table: "tblSede",
                nullable: false,
                defaultValue: false);
        }
    }
}
