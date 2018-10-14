using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblTrabajo")]
    public class Trabajo
    {
        public int ID { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La Descripción es Requerida.")]
        public string Descripcion { get; set; }        
        [Display(Name = "Precio Promedio")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        [Required(ErrorMessage = "El Precio Promedio es Requerido.")]
        public double PrecioPromedio { get; set; }
    }
}
