using AutoCosting.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Tracking
{
    [Table("tblTrackingHeader")]
    public class TrackingHeader
    {
        [Key]
        [Display(Name = "Tracking ID")]
        [DisplayFormat(DataFormatString = "{0:00000000}")]
        public int TrackingID { get; set; }
        [Display(Name = "VIN - Vehículo")]
        [Required(ErrorMessage = "El vehículo es Requerido.")]
        public string VINVehiculo { get; set; }
        [Display(Name = "Vehículo")]
        [ForeignKey("VINVehiculo")]
        public Vehiculo Vehiculo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string Notas { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        [Display(Name = "Costo Total")]
        public double CostoTotal
        {
            get
            {
                double sum = 0;
                if (this.TrackingDetails != null)
                    foreach (TrackingDetail detail in this.TrackingDetails)
                        sum += detail.Costo;
                return sum;
            }
        }
        public IEnumerable<TrackingDetail> TrackingDetails { get; set; }
        [NotMapped]
        public string FechaStr
        {
            get
            {
                return this.Fecha.ToString("dd/MM/yyyy");
            }
        }
        [NotMapped]
        public string TrackingIdStr
        {
            get => this.TrackingID.ToString("00000000");
        }

        [NotMapped]
        public string IndexInfo
        {
            get
            {
                return string.Format("{0} {1} {2}", this.TrackingIdStr, this.Vehiculo != null ? this.Vehiculo.Marca : string.Empty, this.Vehiculo != null ? this.Vehiculo.Modelo : string.Empty);
            }
        }

    }
}
