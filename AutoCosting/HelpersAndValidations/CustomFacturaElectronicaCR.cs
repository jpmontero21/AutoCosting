using FacturaElectronica.ClasesDatos;
using FacturaElectronica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace AutoCosting.HelpersAndValidations
{
    public class CustomFacturaElectronicaCR : FacturaElectronicaCR
    {
        public CustomFacturaElectronicaCR (string numeroConsecutivo, string numeroClave, FacturaElectronica.ClasesDatos.Emisor emisor, FacturaElectronica.ClasesDatos.Receptor receptor,
                                    string condicionVenta, string plazoCredito, string medioPago,
                                    List<LineaDetalle> detalles, string codigoMoneda, decimal tipoCambio) : base (numeroConsecutivo, numeroClave, emisor, receptor, condicionVenta, plazoCredito, medioPago, detalles, codigoMoneda, tipoCambio)
        {

        }

        public async Task ProcesaAsync(string xmlFactura, string directorioSalida, string nombreArchivo)
        {
            //Hay que definir la ruta de salida donde se van a guardar los XML.
            //if (!this.txtFolderSalida.Text.EndsWith("\\"))
            //{
            //    this.txtFolderSalida.Text += "\\";
            //}

            //string directorio = this.txtFolderSalida.Text;
            //string nombreArchivo = this.txtConsecutivo.Text;

            XmlDocument xmlDocSF = new XmlDocument();
            xmlDocSF.LoadXml(xmlFactura);
            xmlDocSF.Save((directorioSalida + (nombreArchivo + "_01_SF.xml"))); // se guarda el xml sin firmar.
            XmlTextWriter xmlTextWriter = new XmlTextWriter((directorioSalida + (nombreArchivo + "_01_SF.xml")), new System.Text.UTF8Encoding(false));
            xmlDocSF.WriteTo(xmlTextWriter);
            xmlTextWriter.Close();
            xmlDocSF = null;

            // se firma el XML
            FirmaServiceReference.FirmaServiceClient firmaClient = new FirmaServiceReference.FirmaServiceClient();
            string xmlFirmado = await firmaClient.FirmarXMLAsync(directorioSalida + nombreArchivo, Emisor.RutaArchivoCertificado, Emisor.PinCertificado);

            //se carga el XML firmado
            XmlDocument xmlElectronica = new XmlDocument();
            //xmlElectronica.Load((directorioSalida + (nombreArchivo + "_02_Firmado.xml")));
            xmlElectronica.LoadXml(xmlFirmado);

            //se instancian las clases Json que se utilizan para enviar la factura
            FacturaElectronica.Emisor myEmisor = new FacturaElectronica.Emisor();
            myEmisor.numeroIdentificacion = Emisor.Identificacion_Numero;
            myEmisor.TipoIdentificacion = Emisor.Identificacion_Tipo;

            FacturaElectronica.Receptor myReceptor = new FacturaElectronica.Receptor();
            if (!string.IsNullOrEmpty(Receptor.Identificacion_Numero))
            {
                myReceptor.sinReceptor = false;
                myReceptor.numeroIdentificacion = Receptor.Identificacion_Numero;
                myReceptor.TipoIdentificacion = Receptor.Identificacion_Tipo;
            }
            else
            {
                myReceptor.sinReceptor = true;
            }

            Recepcion myRecepcion = new Recepcion();
            myRecepcion.emisor = myEmisor;
            myRecepcion.receptor = myReceptor;

            myRecepcion.clave = NumeroClave;
            myRecepcion.fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            myRecepcion.comprobanteXml = Funciones.EncodeStrToBase64(xmlElectronica.OuterXml);

            xmlElectronica = null;

            //se obtiene el "Token" que se utiliza para establecer comunicacion con el API de Hacienda
            string Token = string.Empty;
            Token = getToken(Emisor.UsuarioApi, Emisor.ClaveApi);

            //se envía la factura a Hacienda.
            Comunicacion enviaFactura = new Comunicacion();
            enviaFactura.EnvioDatos(Token, myRecepcion);
            string jsonEnvio = "";
            jsonEnvio = enviaFactura.jsonEnvio;
            string jsonRespuesta = "";
            jsonRespuesta = enviaFactura.jsonRespuesta;
            //txtJSONEnvio.Text = jsonEnvio;
            //txtJSONRespuesta.Text = jsonRespuesta;
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter((directorioSalida
                            + (nombreArchivo + "_03_jsonEnvio.txt")));
            outputFile.Write(jsonEnvio);
            outputFile.Close();
            outputFile = new System.IO.StreamWriter((directorioSalida
                            + (nombreArchivo + "_04_jsonRespuesta.txt")));
            outputFile.Write(jsonRespuesta);
            outputFile.Close();
            if (!(enviaFactura.xmlRespuesta == null))
            {
                //this.txtRespuestaHacienda.Text = enviaFactura.xmlRespuesta.OuterXml;
                enviaFactura.xmlRespuesta.Save((directorioSalida
                                + (nombreArchivo + "_05_RESP.xml")));
            }
            else
            {
                outputFile = new System.IO.StreamWriter((directorioSalida
                                + (nombreArchivo + "_05_RESP_SinRespuesta.txt")));
                outputFile.Write(enviaFactura.mensajeRespuesta);
                outputFile.Close();
                //this.txtRespuestaHacienda.Text = "Consulte en unos minutos, factura se est� procesando.";
                //this.txtRespuestaHacienda.Text = (this.txtRespuestaHacienda.Text + ("\r\n" + ("\r\n" + "Consulte por Clave")));
                //this.txtRespuestaHacienda.Text = (this.txtRespuestaHacienda.Text + ("\r\n" + ("\r\n" + enviaFactura.mensajeRespuesta)));
            }
            //MessageBox.Show(enviaFactura.mensajeRespuesta);
        }
    }
}
