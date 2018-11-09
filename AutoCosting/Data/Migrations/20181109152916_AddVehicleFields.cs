using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoCosting.Data.Migrations
{
    public partial class AddVehicleFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AireAcondicionadoClimatizadoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AireAcondicionadoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AlarmaYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ArosDeLujoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AsientosConMemoriaYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AsientosElectricosYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BluetoothYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BolsasDeAireYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CajaCambiosDualYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CamaraRetrocesoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CassetteYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CierreCentralYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ComputadoraViajeYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ControlDescensoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ControlEstabilidadYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ControlRadioVolanteYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CruiseControlYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DesempañadorTraseroYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DireccionHidraulicaYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DiscoCompactoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EspejosElectricosYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FrenosAbsYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HalogenosYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Imagen1",
                table: "tblVehiculo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagen2",
                table: "tblVehiculo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagen3",
                table: "tblVehiculo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagen4",
                table: "tblVehiculo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagen5",
                table: "tblVehiculo",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LlaveInteligenteYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LucesXenonYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MonitorPresionLlantasYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RTValDiaYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RadioUsbAuxYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RetrovisoreAutoretractilesYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SensorLluviaYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SensoresFrontalesYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SensoresRetrocesoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SunroofTechoPanoramicoYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TapiceriaDeCueroYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TurboYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VidriosElectricosYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VidriosTintadosYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VolanteAjustableYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VolanteMultifuncionalYN",
                table: "tblVehiculo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AireAcondicionadoClimatizadoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "AireAcondicionadoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "AlarmaYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "ArosDeLujoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "AsientosConMemoriaYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "AsientosElectricosYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "BluetoothYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "BolsasDeAireYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "CajaCambiosDualYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "CamaraRetrocesoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "CassetteYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "CierreCentralYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "ComputadoraViajeYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "ControlDescensoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "ControlEstabilidadYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "ControlRadioVolanteYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "CruiseControlYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "DesempañadorTraseroYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "DireccionHidraulicaYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "DiscoCompactoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "EspejosElectricosYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "FrenosAbsYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "HalogenosYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "Imagen1",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "Imagen2",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "Imagen3",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "Imagen4",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "Imagen5",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "LlaveInteligenteYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "LucesXenonYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "MonitorPresionLlantasYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "RTValDiaYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "RadioUsbAuxYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "RetrovisoreAutoretractilesYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "SensorLluviaYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "SensoresFrontalesYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "SensoresRetrocesoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "SunroofTechoPanoramicoYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "TapiceriaDeCueroYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "TurboYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "VidriosElectricosYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "VidriosTintadosYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "VolanteAjustableYN",
                table: "tblVehiculo");

            migrationBuilder.DropColumn(
                name: "VolanteMultifuncionalYN",
                table: "tblVehiculo");
        }
    }
}
