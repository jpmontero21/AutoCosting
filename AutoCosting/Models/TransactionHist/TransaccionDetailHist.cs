using AutoCosting.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.TransactionHist
{
    [Table("tblTransDetailHist")]
    public class TransaccionDetailHist
    {
        //hay que quitar el identity antes de la migration
        public int ID { get; set; }
        [Display(Name = "Número de Transacción")]
        [DisplayFormat(DataFormatString = "{0:00000000}")]
        public int TransID { get; set; }

        [Display(Name = "Transacción")]
        [ForeignKey("TransID")]
        public TransaccionHeaderHist Parent { get; set; }

        [Required(ErrorMessage = "El Vehículo es requerido.")]
        [Display(Name = "VIN - Vehículo")]
        public string VINVehiculo { get; set; }

        [Display(Name = "Vehículo")]
        [ForeignKey("VINVehiculo")]
        public Vehiculo Vehiculo { get; set; }
        [NotMapped]//read only solo para referencia
        public double PrecioMinimo { get; set; }
        [NotMapped]//read only solo para referencia
        public double PrecioRecomendado { get; set; }
        [Required(ErrorMessage = "El precio acordado es requerido.")]
        [Display(Name = "Precio Acordado")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double? PrecioAcordado { get; set; }
    }
}
