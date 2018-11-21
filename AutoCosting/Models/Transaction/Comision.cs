using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Transaction
{
    [Table("tblComision")]
    public class Comision
    {
        public int ID { get; set; }
        [Display(Name = "Transacción")]
        [ForeignKey("TransID")]
        public TransaccionHeader Parent { get; set; }
        [Display(Name = "Número de Transacción")]
        public int TransID { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        [Required(ErrorMessage = "El Monto es requerido.")]
        public double? Monto { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Tipo de Comisión")]
        public TipoComision TipoComision { get; set; }

        /*[Display(Name = "Empleado")]
        [ForeignKey("EmpleadoID")]
        public Empleado Empleado { get; set; }
        [Display(Name = "Empleado ID")]
        public string EmpleadoID { get; set; }*/
    }
}
