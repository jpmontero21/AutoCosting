using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class UpdateUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTrackingHeader_tblVehiculo_VINVehiculo",
                table: "tblTrackingHeader");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "EmpleadoID");

            migrationBuilder.AlterColumn<string>(
                name: "VINVehiculo",
                table: "tblTrackingHeader",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblTrackingHeader_tblVehiculo_VINVehiculo",
                table: "tblTrackingHeader",
                column: "VINVehiculo",
                principalTable: "tblVehiculo",
                principalColumn: "VIN",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTrackingHeader_tblVehiculo_VINVehiculo",
                table: "tblTrackingHeader");

            migrationBuilder.RenameColumn(
                name: "EmpleadoID",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.AlterColumn<string>(
                name: "VINVehiculo",
                table: "tblTrackingHeader",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblTrackingHeader_tblVehiculo_VINVehiculo",
                table: "tblTrackingHeader",
                column: "VINVehiculo",
                principalTable: "tblVehiculo",
                principalColumn: "VIN",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
