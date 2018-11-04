using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddCedulaUniqueContraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "tblCliente",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_tblCliente_Cedula",
                table: "tblCliente",
                column: "Cedula",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblCliente_Cedula",
                table: "tblCliente");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "tblCliente",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
