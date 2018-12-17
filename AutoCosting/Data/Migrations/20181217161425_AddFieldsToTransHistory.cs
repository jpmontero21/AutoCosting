using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddFieldsToTransHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AceptadaHacienda",
                table: "tblTransHeaderHist",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaveHacienda",
                table: "tblTransHeaderHist",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnviadaHacienda",
                table: "tblTransHeaderHist",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AceptadaHacienda",
                table: "tblTransHeaderHist");

            migrationBuilder.DropColumn(
                name: "ClaveHacienda",
                table: "tblTransHeaderHist");

            migrationBuilder.DropColumn(
                name: "EnviadaHacienda",
                table: "tblTransHeaderHist");
        }
    }
}
