using AutoCosting.Models.Maintenance;
using AutoCosting.Models.Tracking;
using AutoCosting.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.ViewModel
{
    public class IndexViewModel
    {
        public List<Vehiculo> Vehiculos { get; set; }
        public List<TransaccionHeader> Transaccions { get; set; }
        public List<TrackingHeader> Trackings { get; set; }
    }
}
