using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Transaction
{
    [Table("tblTransDetail")]
    public class TransaccionDetail
    {
        public int ID { get; set; }
        public int TransID { get; set; }
        public string VINVechiculo { get; set; }
        [NotMapped]//read only solo para referencia
        public double PrecioMinimo { get; set; }
        public double PrecioAcordado { get; set; }

    }
}
