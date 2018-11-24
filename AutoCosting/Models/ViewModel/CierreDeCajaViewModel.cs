using AutoCosting.Models.CierreAperturaCaja;
using AutoCosting.Models.Maintenance;
using AutoCosting.Models.Receipts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.ViewModel
{
    public class CierreDeCajaViewModel
    {
        public Caja CierreCaja { get; set; }
        public Caja AperturaCaja { get; set; }
        public List<Recibo> Recibos { get; set; }
        public Empresa Empresa { get; set; }
        public Recibo Recibo { get; set; }
    }
}
