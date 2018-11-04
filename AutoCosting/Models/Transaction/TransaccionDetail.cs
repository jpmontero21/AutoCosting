using AutoCosting.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Transaction
{
    [Table("tblTransDetail")]
    public class TransaccionDetail
    {
        public int ID { get; set; }
        [Display(Name = "Número de Transacción")]
        [DisplayFormat(DataFormatString = "{0:00000000}")]
        public int TransID { get; set; }

        [Display(Name = "Transacción")]
        [ForeignKey("TransID")]
        TransaccionHeader Parent { get; set; }

        [Required(ErrorMessage = "El Vehículo es requerido.")]
        public string VINVehiculo { get; set; }

        [Display(Name = "Vehículo")]
        [ForeignKey("VINVehiculo")]
        public Vehiculo Vehiculo { get; set; }
        [NotMapped]//read only solo para referencia
        public double PrecioMinimo { get; set; }

        [Required(ErrorMessage = "El precio acordado es requerido.")]
        [Display(Name = "Precio Acordado")]
        public double? PrecioAcordado { get; set; }


        
        
        
        
    }
}
