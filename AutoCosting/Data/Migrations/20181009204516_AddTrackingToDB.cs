using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddTrackingToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblTrackingHeader",
                columns: table => new
                {
                    TrackingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VINVehiculo = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Notas = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTrackingHeader", x => x.TrackingID);
                    table.ForeignKey(
                        name: "FK_tblTrackingHeader_tblVehiculo_VINVehiculo",
                        column: x => x.VINVehiculo,
                        principalTable: "tblVehiculo",
                        principalColumn: "VIN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblTrackingDetail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TrackingId = table.Column<int>(nullable: false),
                    Costo = table.Column<double>(nullable: false),
                    TrabajoId = table.Column<int>(nullable: false),
                    ProveedorId = table.Column<int>(nullable: true),
                    NumeroFactura = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    AddDescripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTrackingDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblTrackingDetail_tblProveedor_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "tblProveedor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblTrackingDetail_tblTrabajo_TrabajoId",
                        column: x => x.TrabajoId,
                        principalTable: "tblTrabajo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTrackingDetail_tblTrackingHeader_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "tblTrackingHeader",
                        principalColumn: "TrackingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblTrackingDetail_ProveedorId",
                table: "tblTrackingDetail",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTrackingDetail_TrabajoId",
                table: "tblTrackingDetail",
                column: "TrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTrackingDetail_TrackingId",
                table: "tblTrackingDetail",
                column: "TrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTrackingHeader_VINVehiculo",
                table: "tblTrackingHeader",
                column: "VINVehiculo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblTrackingDetail");

            migrationBuilder.DropTable(
                name: "tblTrackingHeader");
        }
    }
}
