using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class facturacionElectronica2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AceptadaHacienda",
                table: "tblTransHeader",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AceptadaHacienda",
                table: "tblTransHeader");
        }
    }
}
