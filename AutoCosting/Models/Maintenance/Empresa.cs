using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblEmpresa")]
    public class Empresa
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", ErrorMessage = "Número de teléfono Invalido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        [Required]
        [Display(Name = "Multi Sede")]
        public bool MultiSedeYN { get; set; }
        [Display(Name = "Correo Electrónico")]
        [EmailAddress]
        public string ContactEmail { get; set; }
    }
}
