using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class facturacionElectronica : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaveHacienda",
                table: "tblTransHeader",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnviadaHacienda",
                table: "tblTransHeader",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaveHacienda",
                table: "tblTransHeader");

            migrationBuilder.DropColumn(
                name: "EnviadaHacienda",
                table: "tblTransHeader");
        }
    }
}
