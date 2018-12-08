using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.FacturacionElectronica
{
    [Table("tblEmisor")]
    public class Emisor
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Tipo de Identificación es requerido.")]
        [Display(Name = "Tipo de Identificación")]
        public string TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "Número de Identificación es requerido.")]
        [Display(Name = "Número de Identificación")]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "Nombre es requerido.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Provincia es requerida.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "Cantón es requerido.")]
        [Display(Name = "Cantón")]
        public string Canton { get; set; }

        [Required(ErrorMessage = "Distrito es requerido.")]
        public string Distrito { get; set; }

        [Required(ErrorMessage = "Barrio es requerido.")]
        public string Barrio { get; set; }

        [Required(ErrorMessage = "Otras señas es requerido.")]
        [Display(Name = "Otras Señas")]
        public string OtrasSenas { get; set; }

        [DefaultValue("506")]
        [Required(ErrorMessage = "Código País es requerido.")]
        [Display(Name = "Código País")]
        public string CodigoPaisTelefono { get; set; }

        [Required(ErrorMessage = "Número de Teléfono es requerido.")]
        [Display(Name = "Número de Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", ErrorMessage = "Número de teléfono Invalido.")]
        public string NumeroTelefono{ get; set; }

        [Required(ErrorMessage = "Correo Electrónico es requerido.")]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "La dirección de email no es válida.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "Ruta del Certificado es requerida.")]
        [Display(Name = "Ruta del certificado")]
        public string RutaArchivoCertificado { get; set; }

        [Required(ErrorMessage = "PIN del Certificado es requerido.")]
        [Display(Name = "PIN del certificado")]
        public string PinCertificado { get; set; }

        [Required(ErrorMessage = "Usuario del API es requerido.")]
        [Display(Name = "Usuario API")]
        public string UsuarioApi { get; set; }

        [Required(ErrorMessage = "Clave del API es requerida.")]
        [Display(Name = "Clave API")]
        public string ClaveApi { get; set; }

        [Required(ErrorMessage = "Ruta de guardado de archivos es requerida.")]
        [Display(Name = "Ruta de guardado de archivos")]
        public string OutputFolder { get; set; }

    }
}
