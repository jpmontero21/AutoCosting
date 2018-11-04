using AutoCosting.HelpersAndValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Calc
{
    public class CalcFinanciamiento
    {
        public string Vehiculo { get; set; }
        [Required(ErrorMessage = "El precio es requerido.")]
        public double? Precio { get; set; }
        [Display(Name = "Tasa de Interés Anual (%)")]
        [Required(ErrorMessage = "El porcentaje de interés es requerido.")]
        public double? Interes { get; set; }
        [Required(ErrorMessage = "La cantidad de cuotas es requerida.")]
        [Display(Name = "Cantidad de Cuotas")]
        public int? NumeroCuotas { get; set; }
        [Display(Name = "Tarifa Adicional")]
        public double? TarifaAddicional { get; set; }        
        public double? Prima { get; set; }
        public double? Descuento { get; set; }
        public string Resultado { get; set; }

        public void Calcular()
        {
            double precioTotal = this.Precio.GetValueOrDefault(0);
            double cuota = 0;

            precioTotal += this.TarifaAddicional.GetValueOrDefault(0);
            precioTotal -= this.Descuento.GetValueOrDefault(0);
            precioTotal -= this.Prima.GetValueOrDefault(0);
            //A = P*(r(1+r)^n)/((1+r)^n -1)
            //P = Capital Principal
            //A = Pago Mensual
            //r = Tasa de Interes
            //n = Total de Meses
            double r = (Interes.GetValueOrDefault(0)/100) / 12;
            double n = this.NumeroCuotas.GetValueOrDefault(0);
            cuota = precioTotal * ((r * Math.Pow( 1 + r, n)) / 
                                    (Math.Pow(1+r,n) - 1));
            Resultado = "La cuota Mensual aproximada es de: " + cuota.ToString("#,###.00");
        }
    }
}
