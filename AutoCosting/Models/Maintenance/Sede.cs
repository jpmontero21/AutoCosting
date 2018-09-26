using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Maintenance
{
    [Table("tblSede")]
    public class Sede
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }
        [ForeignKey("EmpresaID")]
        public Empresa Empresa { get; set; }
        [Required]
        public string Nombre { get; set; }        
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", ErrorMessage = "Número de teléfono Invalido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        [DefaultValue(false)]
        [Display(Name = "Usar Cierre de Caja")]
        public bool UsarCierreCajaYN { get; set; }
        [DefaultValue(true)]
        [Display(Name = "Imprimir Logo")]
        public bool ImprimirLogoYN { get; set; }

    }
}
