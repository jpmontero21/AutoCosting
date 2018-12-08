using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.HelpersAndValidations
{
    public enum Transmision : short
    { 
        Manual = 1,
        [Display(Name = "Automático")]
        Automatico = 2,
        [Display(Name = "Automático/Dual")]
        Automatico_Dual = 3,
        Otro = 4
    }

    public enum Combustible : short
    {
        Gasolina = 0,
        [Display(Name = "Diésel")]
        Diesel = 1 ,
        [Display(Name = "Híbrido")]
        Hibrido = 2,
        [Display(Name = "Eléctrico")]
        Electrico = 3,
        Otro = 4
    }

    public enum Estado : short
    {
        Excelente = 0,
        Bueno = 1,
        Regular = 2,
        Malo = 3,
        [Display(Name = "En Reparación")]
        EnReparacion = 4
    }

    public enum TipoTransaccion : short
    {
        Venta = 0 ,
        Apartado = 1,
        [Display(Name = "Cotización")]
        Cotizacion = 2
    }

    public enum TipoPago : short
    {
        [Display(Name = "Crédito")]
        Credito = 0,
        Contado = 1
    }

    public enum TipoComision : short
    {
        Interna = 0,
        Externa = 1        
    }

    public enum TipoCaja : short
    {
        Cierre = 0,
        Apertura = 1
    }
    public enum TipoCedula : short
    {
        [Display(Name = "Cédula Física")]
        CedulaFisica = 1,
        [Display(Name = "Cédula Jurídica")]
        CedulaJuridica = 2,
        DIMEX = 3,
        NITE = 4
    }
    public enum Provincia : short
    {
        [Display(Name = "San José")]
        SanJose = 1,
        Alajuela = 2,
        Cartago = 3,
        Heredia = 4,
        Guanacaste = 5,
        Puntarenas = 6,
        [Display(Name = "Limón")]
        Limon = 7
    }
}
