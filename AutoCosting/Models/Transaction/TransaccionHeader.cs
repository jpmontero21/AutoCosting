using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Transaction
{
    [Table("tblTransHeader")]
    public class TransaccionHeader
    {
        [Key]
        public int TransID { get; set; }
        public string VendedorID { get; set; }
        public int ClienteID { get; set; }
        public int SedeID { get; set; }
        public TipoPago TipoPago { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }
        public bool ApartadoVencidoYN { get; set; }

        public double Saldo { get; set; }
        public DateTime Fecha { get; set; }
        public double Total { get; set; }

    }
}
