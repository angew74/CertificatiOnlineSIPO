using System;
using System.Xml;
using System.Web.Services.Protocols;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.WS
{
    public static class ExceptionUty
    {
        private const string CODICE_ELEMENT = "Codice";
        private const string DESCRIZIONE_ELEMENT = "Descrizione";
        private const string NAMESPACE = "http://www.comune.roma.it/certificati/";

        public static XmlNode SetDetails(string codiceInternalError)
        {
            ListaErroriSOAP SOAPErrorDataset =
                CacheManager<ListaErroriSOAP>.get(CacheKeys.LISTA_ERRORI_SOAP, VincoloType.FILESYSTEM);
            ListaErroriSOAP.ErroreRow[] errorRows = SOAPErrorDataset.Errore.Select("InternalError = '" + codiceInternalError + "'") as ListaErroriSOAP.ErroreRow[];

            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element,
                 SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);
            XmlNode codiceElement =
              doc.CreateNode(XmlNodeType.Element, CODICE_ELEMENT, NAMESPACE);
            codiceElement.InnerText = codiceInternalError;
            node.AppendChild(codiceElement);
            XmlNode descrizioneElement = doc.CreateNode(XmlNodeType.Element, DESCRIZIONE_ELEMENT, NAMESPACE);
            descrizioneElement.InnerText = errorRows[0].SOAPDescription;
            node.AppendChild(descrizioneElement);

            return node;
        }

        public static XmlQualifiedName SetSOAPFault(string codiceInternalError)
        {
            XmlQualifiedName faultType = null;

            ListaErroriSOAP SOAPErrorDataset =
                CacheManager<ListaErroriSOAP>.get(CacheKeys.LISTA_ERRORI_SOAP, VincoloType.FILESYSTEM);
            ListaErroriSOAP.ErroreRow[] errorRows = SOAPErrorDataset.Errore.Select("InternalError = '" + codiceInternalError + "'") as ListaErroriSOAP.ErroreRow[];

            if (errorRows.Length != 0)
            {
                switch (errorRows[0].SOAPActor)
                {
                    case "Client":
                        faultType = SoapException.ClientFaultCode;
                        break;

                    case "Server":
                        faultType = SoapException.ServerFaultCode;
                        break;

                    default:
                        break;
                }
            }
            return faultType;   
        }
    }
}
