using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.HelpersAndValidations
{
    public enum Transmision : short
    { 
        Manual = 1,
        Automatico = 2,
        Otro = 3
    }

    public enum Combustible : short
    {
        Gasolina = 0,
        Disel = 1 ,
        Hibrido = 2,
        Electrico = 3,
        Otro = 4
    }

    public enum Estado : short
    {
        Excelente = 0,
        Bueno = 1,
        Regular = 2,
        Malo = 3,
        EnReparacion = 4
    }

    public enum TipoTransaccion : short
    {
        Venta = 0 ,
        Apartado = 1,
        Cotizacion = 2
    }

    public enum TipoPago : short
    {
        Credito = 0,
        Contado = 1
    }
}
