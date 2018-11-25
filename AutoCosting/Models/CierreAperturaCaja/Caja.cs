using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.CierreAperturaCaja
{
    [Table("tblCaja")]
    public class Caja
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime? Fecha { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        [Required(ErrorMessage = "El monto es requerido.")]
        public double? Monto { get; set; }

        [Required(ErrorMessage = "El tipo es requerido.")]
        public TipoCaja Tipo { get; set; }

        [Required(ErrorMessage = "El Usuario es requerido.")]
        public string Usuario { get; set; }

        [Display(Name = "Número de Caja")]
        public string NumeroCaja { get; set; }

        public string Observaciones { get; set; }

    }
}
