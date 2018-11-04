using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddTransactionToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblTransHeader",
                columns: table => new
                {
                    TransID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    table.PrimaryKey("PK_tblTransHeader", x => x.TransID);
                    table.ForeignKey(
                        name: "FK_tblTransHeader_tblCliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "tblCliente",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTransHeader_tblEmpleado_VendedorID",
                        column: x => x.VendedorID,
                        principalTable: "tblEmpleado",
                        principalColumn: "Cedula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTransHeader_tblSede_SedeID_EmpresaID",
                        columns: x => new { x.SedeID, x.EmpresaID },
                        principalTable: "tblSede",
                        principalColumns: new[] { "ID", "EmpresaID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTransDetail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransID = table.Column<int>(nullable: false),
                    VINVehiculo = table.Column<string>(nullable: false),
                    PrecioAcordado = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTransDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tblTransDetail_tblTransHeader_TransID",
                        column: x => x.TransID,
                        principalTable: "tblTransHeader",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTransDetail_tblVehiculo_VINVehiculo",
                        column: x => x.VINVehiculo,
                        principalTable: "tblVehiculo",
                        principalColumn: "VIN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblTransDetail_TransID",
                table: "tblTransDetail",
                column: "TransID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransDetail_VINVehiculo",
                table: "tblTransDetail",
                column: "VINVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransHeader_ClienteID",
                table: "tblTransHeader",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransHeader_VendedorID",
                table: "tblTransHeader",
                column: "VendedorID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTransHeader_SedeID_EmpresaID",
                table: "tblTransHeader",
                columns: new[] { "SedeID", "EmpresaID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblTransDetail");

            migrationBuilder.DropTable(
                name: "tblTransHeader");
        }
    }
}
