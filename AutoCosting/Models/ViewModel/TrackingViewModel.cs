using AutoCosting.Models.Maintenance;
using AutoCosting.Models.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.ViewModel
{
    public class TrackingViewModel
    {
        public TrackingHeader Tracking { get; set; }        
        public List<TrackingDetail> TrackingDetalles { get; set; }
        //public IEnumerable<Vehiculo> VehiculoList { get; set; }
        public TrackingDetail TrackingDetail { get; set; }
    }
}
