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
        [Display(Name = "Tracking ID")]
        [DisplayFormat(DataFormatString = "{0:00000000}")]
        public int TrackingId { get; set; }
        [Display(Name = "Tracking")]
        [ForeignKey("TrackingId")]
        public TrackingHeader Parent { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double Costo { get; set; }
        [Display(Name = "Trabajo")]
        [Required(ErrorMessage = "El trabajo es requerido")]
        public int TrabajoId { get; set; }
        [ForeignKey("TrabajoId")]
        public Trabajo Trabajo { get; set; }
        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; }
        [Display(Name = "Numero de Fáctura")]
        public string NumeroFactura { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Descripción Adicional")]
        public string AddDescripcion { get; set; }
        
    }
}
