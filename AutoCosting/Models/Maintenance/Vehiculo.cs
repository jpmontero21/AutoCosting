using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblVehiculo")]
    public class Vehiculo
    {
        [Key]
        [MinLength(17, ErrorMessage = "El VIN debe tener al menos 17 dígitos")]
        [Required(ErrorMessage = "El VIN es requerido.")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string VIN { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El Modelo es requerido.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es requerido.")]
        [Display(Name = "Año")]
        public int Anno { get; set; }

        [Required(ErrorMessage = "La transmisión es requerida.")]
        [Display(Name = "Transmisión")]
        public Transmision Transmision { get; set; }

        [Required(ErrorMessage = "El estilo es requerido.")]
        public string Estilo { get; set; }

        [Required(ErrorMessage = "El tipo de combustible es requerido.")]
        public Combustible Combustible { get; set; }

        [Required(ErrorMessage = "El número de puertas es requerido.")]
        [Display(Name = "Número de puertas")]
        public string NumeroPuertas { get; set; }//combo con 2-3 , 4 o mas

        public string Color { get; set; }

        public string Placa { get; set; }

        [Required(ErrorMessage = "El Kilometraje es requerido.")]
        public int Kilometraje { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        public Estado Estado { get; set; }

        [Required(ErrorMessage = "La Cilindrada es requerida.")]
        public int Cilindrada { get; set; }

        [Display(Name = "Precio mínimo")]
        [Required(ErrorMessage = "El precio mínimo es requerido.")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double PrecioMinimo { get; set; }

        [Display(Name = "Precio Recomendado")]
        [Required(ErrorMessage = "El precio recomendado es requerido.")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double PrecioRecomendado { get; set; }

        [Display(Name = "Apartado")]
        public bool ApartadoYN { get; set; }

        [Display(Name = "Vendido")]
        public bool VendidoYN { get; set; }

        [Required(ErrorMessage = "La fecha de Ingreso es requerida.")]
        [Display(Name = "Fecha de Ingreso")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FechaIngreso { get; set; }

        [NotMapped]
        public string Descripcion
        {
            get
            {
                return string.Format("{0} {1} {2}", this.VIN, this.Marca, this.Modelo);
            }
        }

        // En estos campos se guardan las rutas de cada imagen.
        [Display(Name = "Imagen/Fotografía 1")]
        public string Imagen1 { get; set; }
        [Display(Name = "Imagen/Fotografía 2")]
        public string Imagen2 { get; set; }
        [Display(Name = "Imagen/Fotografía 3")]
        public string Imagen3 { get; set; }
        [Display(Name = "Imagen/Fotografía 4")]
        public string Imagen4 { get; set; }
        [Display(Name = "Imagen/Fotografía 5")]
        public string Imagen5 { get; set; }

        //Equipamiento extra
        [Display(Name = "Dirección Hidráulica")]
        public bool DireccionHidraulicaYN { get; set; }
        [Display(Name = "Cierre Central")]
        public bool CierreCentralYN { get; set; }
        [Display(Name = "Asientos Eléctricos")]
        public bool AsientosElectricosYN { get; set; }
        [Display(Name = "Vidrios Tintados")]
        public bool VidriosTintadosYN { get; set; }
        [Display(Name = "Bolsa(s) de Aire")]
        public bool BolsasDeAireYN { get; set; }
        [Display(Name = "Vidrios Eléctricos")]
        public bool VidriosElectricosYN { get; set; }
        [Display(Name = "Espejos Eléctricos")]
        public bool EspejosElectricosYN { get; set; }
        [Display(Name = "Alarma")]
        public bool AlarmaYN { get; set; }
        [Display(Name = "Frenos ABS")]
        public bool FrenosAbsYN { get; set; }
        [Display(Name = "Aire Acondicionado")]
        public bool AireAcondicionadoYN { get; set; }
        [Display(Name = "Desempañador Trasero")]
        public bool DesempanadorTraseroYN { get; set; }
        [Display(Name = "Sunroof/Techo Panorámico")]
        public bool SunroofTechoPanoramicoYN { get; set; }
        [Display(Name = "Aros de Lujo")]
        public bool ArosDeLujoYN { get; set; }
        [Display(Name = "Turbo")]
        public bool TurboYN { get; set; }
        [Display(Name = "Tapicería de Cuero")]
        public bool TapiceriaDeCueroYN { get; set; }
        [Display(Name = "Halógenos")]
        public bool HalogenosYN { get; set; }
        [Display(Name = "Cassette")]
        public bool CassetteYN { get; set; }
        [Display(Name = "Disco Compacto (DVD)")]
        public bool DiscoCompactoYN { get; set; }
        [Display(Name = "Cruise Control")]
        public bool CruiseControlYN { get; set; }
        [Display(Name = "Radio con USB/AUX")]
        public bool RadioUsbAuxYN { get; set; }
        [Display(Name = "RTV al día")]
        public bool RTValDiaYN { get; set; }
        [Display(Name = "Control de Estabilidad")]
        public bool ControlEstabilidadYN { get; set; }
        [Display(Name = "Control de Descenso")]
        public bool ControlDescensoYN { get; set; }
        [Display(Name = "Caja de Cambios Dual")]
        public bool CajaCambiosDualYN { get; set; }
        [Display(Name = "Cámara de Retroceso")]
        public bool CamaraRetrocesoYN { get; set; }
        [Display(Name = "Sensores de Retroceso")]
        public bool SensoresRetrocesoYN { get; set; }
        [Display(Name = "Sensores Frontales")]
        public bool SensoresFrontalesYN { get; set; }
        [Display(Name = "Control de Radio en el Volante")]
        public bool ControlRadioVolanteYN { get; set; }
        [Display(Name = "Volante Multifuncional")]
        public bool VolanteMultifuncionalYN { get; set; }
        [Display(Name = "Aire Acondicionado Climatizado")]
        public bool AireAcondicionadoClimatizadoYN { get; set; }
        [Display(Name = "Asiento(s) con Memoria")]
        public bool AsientosConMemoriaYN { get; set; }
        [Display(Name = "Retrovisores Auto-Retractiles")]
        public bool RetrovisoreAutoretractilesYN { get; set; }
        [Display(Name = "Luces de Xenon/Bixenon")]
        public bool LucesXenonYN { get; set; }
        [Display(Name = "Sensor de Lluvia")]
        public bool SensorLluviaYN { get; set; }
        [Display(Name = "Llave Inteligente/Botón de Arranque")]
        public bool LlaveInteligenteYN { get; set; }
        [Display(Name = "Monitor de Presión de Llantas")]
        public bool MonitorPresionLlantasYN { get; set; }
        [Display(Name = "Computadora de Viaje")]
        public bool ComputadoraViajeYN { get; set; }
        [Display(Name = "Volante Ajustable")]
        public bool VolanteAjustableYN { get; set; }
        [Display(Name = "Bluetooth")]
        public bool BluetoothYN { get; set; }
    }
}
