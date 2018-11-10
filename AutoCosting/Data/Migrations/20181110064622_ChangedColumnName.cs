using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class ChangedColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesempañadorTraseroYN",
                table: "tblVehiculo",
                newName: "DesempanadorTraseroYN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesempanadorTraseroYN",
                table: "tblVehiculo",
                newName: "DesempañadorTraseroYN");
        }
    }
}
