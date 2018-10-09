using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddVehiculoToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblVehiculo",
                columns: table => new
                {
                    VIN = table.Column<string>(nullable: false),
                    Marca = table.Column<string>(nullable: false),
                    Modelo = table.Column<string>(nullable: false),
                    Anno = table.Column<int>(nullable: false),
                    Transmision = table.Column<short>(nullable: false),
                    Estilo = table.Column<string>(nullable: false),
                    Combustible = table.Column<short>(nullable: false),
                    NumeroPuertas = table.Column<string>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Placa = table.Column<string>(nullable: true),
                    Kilometraje = table.Column<int>(nullable: false),
                    Estado = table.Column<short>(nullable: false),
                    Cilindrada = table.Column<int>(nullable: false),
                    PrecioMinimo = table.Column<double>(nullable: false),
                    PrecioRecomendado = table.Column<double>(nullable: false),
                    ApartadoYN = table.Column<bool>(nullable: false),
                    FechaIngreso = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehiculo", x => x.VIN);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblVehiculo");
        }
    }
}
