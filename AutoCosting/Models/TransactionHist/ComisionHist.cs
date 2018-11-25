using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.TransactionHist
{
    [Table("tblComisionHist")]
    public class ComisionHist
    {
        //hay que quitar el identity antes de la migration
        public int ID { get; set; }
        [Display(Name = "Transacción")]
        [ForeignKey("TransID")]
        public TransaccionHeaderHist Parent { get; set; }
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
    }
}
