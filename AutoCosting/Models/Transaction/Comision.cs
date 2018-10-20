using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Transaction
{
    [Table("tblComision")]
    public class Comision
    {
        public int ID { get; set; }
        public int TransID { get; set; }
        public string Nombre { get; set; }
        public double Monto { get; set; }
        public string Descripcion { get; set; }
        public TipoComision TipoComision { get; set; }
    }
}
