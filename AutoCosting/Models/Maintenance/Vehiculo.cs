using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblVehiculo")]
    public class Vehiculo
    {
        [Key]
        [Required(ErrorMessage = "El VIN es requerido.")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string VIN { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El Modelo es requerido.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es requerido.")]
        [Display(Name = "Año")]
        public int Anno { get; set; }

        [Required(ErrorMessage = "La transmisión es requerida.")]
        [Display(Name = "Transmisión")]
        public Transmision Transmision { get; set; }

        [Required(ErrorMessage = "El estilo es requerido.")]
        public string Estilo { get; set; }

        [Required(ErrorMessage = "El tipo de combustible es requerido.")]
        public Combustible Combustible { get; set; }

        [Required(ErrorMessage = "El número de puertas es requerido.")]
        [Display(Name = "Número de puertas")]
        public string NumeroPuertas { get; set; }//combo con 2-3 , 4 o mas

        public string Color { get; set; }

        public string Placa { get; set; }

        [Required(ErrorMessage = "El Kilometraje es requerido.")]
        public int Kilometraje { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        public Estado Estado { get; set; }

        [Required(ErrorMessage = "La Cilindrada es requerida.")]
        public int Cilindrada { get; set; }

        [Display(Name = "Precio mínimo")]
        [Required(ErrorMessage = "El precio mínimo es requerido.")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double PrecioMinimo { get; set; }

        [Display(Name = "Precio Recomendado")]
        [Required(ErrorMessage = "El precio recomendado es requerido.")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double PrecioRecomendado { get; set; }

        [Display(Name = "Apartado")]
        [DefaultValue(false)]
        public bool ApartadoYN { get; set; }

        [Required(ErrorMessage = "La fecha de Ingreso es requerida.")]
        [Display(Name = "Fecha de Ingreso")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FechaIngreso { get; set; }

        [NotMapped]
        public string Descripcion
        {
            get
            {
                return string.Format("{0} {1} {2}", this.VIN, this.Marca, this.Modelo);
            }
        }
    }
}
