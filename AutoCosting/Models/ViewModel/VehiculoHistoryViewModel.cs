using AutoCosting.Models.Maintenance;
using AutoCosting.Models.Tracking;
using AutoCosting.Models.Transaction;
using AutoCosting.Models.TransactionHist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.ViewModel
{
    public class VehiculoHistoryViewModel
    {
        public Vehiculo Vehiculo { get; set; }
        public TrackingHeader Tracking { get; set; }

        public List<TransaccionHeader> TransaccionHeaderList { get; set; }
        public List<TransaccionHeaderHist> TransaccionHeaderHistList { get; set; }
        

    }
}
