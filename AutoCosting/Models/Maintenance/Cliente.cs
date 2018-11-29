using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblCliente")]
    public class Cliente
    {
        public int ID { get; set; }
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
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "La dirección de email no es válida.")]
        public string Email { get; set; }
        public string Notas { get; set; }

        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "La Cédula es Requerida.")]
        public string Cedula { get; set; }

        [NotMapped]
        public string Informacion
        {
            get
            {
                return $"{Cedula} - {Nombre} {Apellido1} {Apellido2}";
            }
        }
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                string nombre = $" {Nombre} {Apellido1} {Apellido2} ";
                return nombre.ToUpper();
            }
        }
        [NotMapped]
        public string CedulaSinGuiones
        {
            get
            {
                return Cedula.Replace("-", string.Empty);
            }
        }
    }
}
