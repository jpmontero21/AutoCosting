using AutoCosting.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Tracking
{
    [Table("tblTrackingDetail")]
    public class TrackingDetail
    {
        public int ID { get; set; }
        public int TrackingId { get; set; }
        [ForeignKey("TrackingId")]
        public TrackingHeader Parent { get; set; }

        public double Costo { get; set; }
        public int TrabajoId { get; set; }
        [ForeignKey("TrabajoId")]
        public Trabajo Trabajo { get; set; }
        public int? ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; }
        [Display(Name = "Numero de Fáctura")]
        public string NumeroFactura { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Descripción Addicional")]
        public string AddDescripcion { get; set; }
    }
}
