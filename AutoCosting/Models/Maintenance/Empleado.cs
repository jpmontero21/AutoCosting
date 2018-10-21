using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblEmpleado")]
    public class Empleado
    {
        [Key]
        [Display(Name = "Cédula")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "La Cédula es requerida.")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Primer Apellido es requerido.")]
        [Display(Name = "Primer Apellido")]
        public string Apellido1 { get; set; }
        [Display(Name = "Segundo Apellido")]
        public string Apellido2 { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", ErrorMessage = "Número de teléfono Invalido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        public string Notas { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FechaNacimiento { get; set; }
        [Display(Name = "Descripción del Puesto")]
        public string DescripcionPuesto { get; set; }
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                return string.Format("{0} - {1} {2}", this.Cedula, this.Nombre, this.Apellido1);
            }
        }
    }
}
