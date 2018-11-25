using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddHistoryToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblTransHeaderHist",
                columns: table => new
                {
                    TransID = table.Column<int>(nullable: false),
                    VendedorID = table.Column<string>(nullable: false),
                    ClienteID = table.Column<int>(nullable: false),
                    SedeID = table.Column<int>(nullable: false),
                    EmpresaID = table.Column<int>(nullable: false),
                    TipoPago = table.Column<short>(nullable: false),
                    TipoTransaccion = table.Column<short>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Saldo = table.Column<double>(nullable: true),
                    Eliminada = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTransHeaderHist", x => x.TransID);
                    table.ForeignKey(
                        name: "FK_tblTransHeaderHist_tblCliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "tblCliente",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTransHeaderHist_tblEmpleado_VendedorID",
                        column: x => x.VendedorID,
                        principalTable: "tblEmpleado",
                        principalColumn: "Cedula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTransHeaderHist_tblSede_SedeID_EmpresaID",
                        columns: x => new { x.SedeID, x.EmpresaID },
                        principalTable: "tblSede",
                        principalColumns: new[] { "ID", "EmpresaID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReciboHist",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    TransID = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    Abono = table.Column<double>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReciboHist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblReciboHist_tblTransHeaderHist_TransID",
                        column: x => x.TransID,
                        principalTable: "tblTransHeaderHist",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTransDetailHist",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    TransID = table.Column<int>(nullable: false),
                    VINVehiculo = table.Column<string>(nullable: false),
                    PrecioAcordado = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTransDetailHist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblTransDetailHist_tblTransHeaderHist_TransID",
                        column: x => x.TransID,
                        principalTable: "tblTransHeaderHist",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTransDetailHist_tblVehiculo_VINVehiculo",
                        column: x => x.VINVehiculo,
                        principalTable: "tblVehiculo",
                        principalColumn: "VIN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblReciboHist_TransID",
                table: "tblReciboHist",
                column: "TransID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransDetailHist_TransID",
                table: "tblTransDetailHist",
                column: "TransID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransDetailHist_VINVehiculo",
                table: "tblTransDetailHist",
                column: "VINVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransHeaderHist_ClienteID",
                table: "tblTransHeaderHist",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransHeaderHist_VendedorID",
                table: "tblTransHeaderHist",
                column: "VendedorID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransHeaderHist_SedeID_EmpresaID",
                table: "tblTransHeaderHist",
                columns: new[] { "SedeID", "EmpresaID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblReciboHist");

            migrationBuilder.DropTable(
                name: "tblTransDetailHist");

            migrationBuilder.DropTable(
                name: "tblTransHeaderHist");
        }
    }
}
