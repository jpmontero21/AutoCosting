using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.FacturacionElectronica
{
    public class CodificacionMH
    {
        public int ID { get; set; }

        public string idProvincia { get; set; }
        public string nombreProvincia { get; set; }
        public string idCanton { get; set; }
        public string nombreCanton { get; set; }
        public string idDistrito { get; set; }
        public string nombreDistrito { get; set; }
        public string idBarrio { get; set; }
        public string nombreBarrio { get; set; }
    }
}
