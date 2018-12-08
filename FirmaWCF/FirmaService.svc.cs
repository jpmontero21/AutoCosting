using FacturaElectronica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FirmaWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class FirmaService : IFirmaService
    {
        public string FirmarXML(string path, string rutaArchivoCertificado, string clave)
        {
            Firma firma = new Firma();
            firma.FirmaXML_Xades(path, rutaArchivoCertificado, clave);
            return path + "_02_Firmado.xml";
        }
    }
}
