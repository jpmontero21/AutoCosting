using AutoCosting.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.ViewModel
{
    public class TransaccionViewModel
    {
        public TransaccionHeader Transaccion { get; set; }
        public List<TransaccionDetail> TransaccionDetalles { get; set; }
        //public IEnumerable<Vehiculo> VehiculoList { get; set; }
        public TransaccionDetail TransaccionDetail { get; set; }
    }
}
