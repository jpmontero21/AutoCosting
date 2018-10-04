using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblProveedor")]
    public class Proveedor
    {
        public int ID { get; set; }
        [Display(Name = "Nombre del Contacto")]
        [Required(ErrorMessage = "El Nombre del Contacto es requerido.")]
        public string NombreContacto { get; set; }
        [Display(Name = "Nombre de la Empresa")]
        public string NombreEmpresa { get; set; }
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
    }
}
