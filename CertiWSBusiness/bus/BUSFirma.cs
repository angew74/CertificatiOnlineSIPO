using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.Logging;
using log4net;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Xml.Linq;
using System.Linq;
using Com.Unisys.CdR.Certi.WS.Business.firma;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using Com.Unisys.Logging.Errors;
using System.Net.Security;
using System.Web.Services.Protocols;
using Com.Unisys.CdR.Certi.WS.Business.bus;
using System.CodeDom;
using Org.BouncyCastle.Utilities.Encoders;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSFirma
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BUSFirma));

        /// <summary>
        /// 
        /// </summary>
        public BUSFirma()
        {
        }
     
     /*  
        /// <summary>
        /// Richiesta firma digitale nuovo metodo con invocazione web services
        /// </summary>
        /// <param name="r">Riga dati certificato</param>
        /// <returns>Il documento pdf come array di byte</returns>
        public byte[] SetFirmaWS(ProfiloRichiesta.CertificatiRow r)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(r.XML_CERTIFICATO);
            return SetFirmaWS(r, doc);
        }
     */

        /// <summary>
        /// Richiesta firma digitale con invocazione web services
        /// </summary>
        /// <param name="r">Riga dati certificato</param>
        /// <param name="doc">dati certificato come documento xml</param>
        /// <returns>Il documento pdf come array di byte</returns>
        /// 
        /*
        public byte[] SetFirmaWS(ProfiloRichiesta.CertificatiRow r, XmlDocument doc)
        {
            byte[] pdfByteFirma = new byte[0];
            string chiave = r.T_TIPO_CERTIFICATO;
            string foglio = String.Empty;
            FogliDecoderConfigSection section = (FogliDecoderConfigSection)ConfigurationManager.GetSection("FogliDecoder");
            if (section != null)
            {
                for (int i = 0; i < section.FogliDecoderItems.Count; i++)
                    if (section.FogliDecoderItems[i].Name.Equals(chiave))
                    {
                        foglio = section.FogliDecoderItems[i].Value;
                    }
            }            

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?>"
              , doc.DocumentElement.OuterXml);

            string timb = String.Concat("<TD:Global xmlns:TD=\"",
                ConfigurationManager.AppSettings["TagTimbro"), "\"><TD:XSL_Def><TD:orig>",
                ConfigurationManager.AppSettings["DownloadTimbro"), "</TD:orig></TD:XSL_Def></TD:Global>");
            doc.DocumentElement.InnerXml = String.Concat(timb,
                doc.DocumentElement.InnerXml);


            XmlElement elemP = doc.CreateElement("codicePagamento");
            string pag = r.CODICE_PAGAMENTO;

            ProfiloTipoUso.TipoUsoRow[] tips = (ProfiloTipoUso.TipoUsoRow[])(
                (CacheManager<ProfiloTipoUso.TipoUsoDataTable>.get(CacheKeys.TIPI_USO_WS, VincoloType.NONE)).Select(
                "ID=" + r.TIPO_USO_ID));

            if (tips[0].Descrizione != "SENZA DIRITTI DI SEGRETERIA")
            {
                //per bypassare poste che non restituisce xml di pagamento
                ProfiloClient.ClientsRow[] client = CacheManager<ProfiloClient.ClientsDataTable>.get(
                    CacheKeys.CLIENTS_WS, VincoloType.NONE).Select("ID = " + r.RichiesteRow.CLIENT_ID) as
                    ProfiloClient.ClientsRow[];
                if (string.Equals(client[0].Public_ID.ToUpper(), "PCOM"))
                {
                    string importoPagato = null;
                    rendicontazioneRiconciliatiType type = SerializationHelper.DeserializeFromXmlString<rendicontazioneRiconciliatiType>(r.XML_PAGAMENTO);
                    importoPagato = type.importo.ToString();                    
                    if (string.IsNullOrEmpty(importoPagato))
                        throw new InvalidOperationException("Nodo importo pagato non presente nell'xml");

                    pag = String.Concat(pag, "(", importoPagato, " Euro)");

                }
                else
                { pag = "pagato allo sportello"; }
            }
            else
            {
                pag = "esente da pagamento";
            }

            elemP.InnerXml = pag;
            doc.DocumentElement.SelectSingleNode("//indice").AppendChild(elemP);
            WebFormatter wf = new WebFormatter();
            SP proxyTimbro = new SP();
            string url = ConfigurationManager.AppSettings["UrlTimbroWS");

            try
            {

                log.Debug("prima di timbro");
                log.Debug(ConfigurationManager.AppSettings["UrlTimbroWS"));
                proxyTimbro.Url = url;
                ServiziTimbroEnum enumeratore = (ServiziTimbroEnum)Enum.Parse(typeof(ServiziTimbroEnum), r.T_BACKEND_ID);
                string service = ((int)enumeratore).ToString();
                X509Certificate cert = new X509Certificate();
                cert = SelectCertificate(ConfigurationManager.AppSettings["certNameTimbro"));
                if (cert != null)
                {
                    proxyTimbro.ClientCertificates.Add(cert);
                    log.Debug("ho caricato il certificato numero certificati :" + cert.Subject + " " + cert.GetName());
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    log.Debug("creato request: " + url);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    XmlDocument xdoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = xdoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = xdoc.DocumentElement;
                    xdoc.InsertBefore(xmlDeclaration, root);
                    XmlElement element1 = xdoc.CreateElement(string.Empty, "request", string.Empty);
                    xdoc.AppendChild(element1);
                    SPServiceResponse response = proxyTimbro.securizeXMLWithClosingDocument(service, Encoding.UTF8.GetBytes(doc.InnerXml), Encoding.UTF8.GetBytes(xdoc.InnerXml), null);
                    log.Debug("ho chiamato: ");
                    pdfByteFirma = response.securedDocument;
                }
                else
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: Certificato non trovato",
                    "ERR_01B",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + "CIU: " + r.CIU,
                    null);
                    log.Error(mex);
                    throw mex;
                }
                log.Debug("dopo di timbro");
            }
            catch (ManagedException ex)
            {

                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_011",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + "CIU: " + r.CIU,
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_01B",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + "CIU: " + r.CIU,
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                     "ERR_01B",
                     "Certi.WS.Business.BUSManager.SetFirmaWS ",
                     " Dettagli: " + "CIU: " + r.CIU,
                     ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            return pdfByteFirma;
        }*/

        public byte[] SetFirmaDOUSipo(byte[] pdf)
        {
            DOU2 proxyTimbro = new DOU2();
            string url = ConfigurationManager.AppSettings["UrlTimbroWS"];
            byte[] pdfByteFirma = new byte[0];
            try
            {

                log.Debug("prima di timbro");
                log.Debug(ConfigurationManager.AppSettings["UrlTimbroWS"]);
                proxyTimbro.Url = url;
                X509Certificate certRequest = new X509Certificate();
               // X509Certificate certRequest = new X509Certificate(bytes, pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                certRequest = SelectCertificate(ConfigurationManager.AppSettings["certNameTimbro"]);
                if (certRequest != null)
                {
                    proxyTimbro.ClientCertificates.Add(certRequest);
                    log.Debug("ho caricato il certificato numero certificati :" + certRequest.Subject + " " + certRequest.GetName());
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    log.Debug("creato request: " + url);              
                    XmlDocument xml = new XmlDocument();
                    xml.CreateElement("request");
                    byte[] metadata = SerializationHelper.ToBas64EncodeXml(xml.OuterXml);
                    int idc =int.Parse(ConfigurationManager.AppSettings["idcertificatodou"]);
                    if (ConfigurationManager.AppSettings["useproxy"] == "1")
                    {
                        proxyTimbro.Proxy = new WebProxy(ConfigurationManager.AppSettings["ipproxy"], int.Parse(ConfigurationManager.AppSettings["portproxy"]));
                        log.Debug("dentro use proxy");
                    }
                    Param param = new Param();
                   // param.coords = new Coords();
                   // param.coords.x = 725;
                   // param.coords.y = 50;
                    DOUResponse response = proxyTimbro.createAndUploadWithParam(pdf,idc, null,param, metadata,"","","");
                    log.Debug("ho chiamato: ");
                    pdfByteFirma = response.pdf;
                    int status = response.status;
                    var s = response.reason;
                    if(status != 0)
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + s,
                    "ERR_011",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + s,
                    null);
                        log.Error(mex);
                        throw mex;
                    }
                }
                else
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: Certificato non trovato",
                    "ERR_01B",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: Certificato autenticazione non caricato " + ConfigurationManager.AppSettings["certNameTimbro"],
                    null);
                    log.Error(mex);
                    throw mex;
                }
                log.Debug("dopo di timbro");
            }
            catch (ManagedException ex)
            {

                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_011",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + ex.Message,
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_01B",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + ex.Message,
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                     "ERR_01B",
                     "Certi.WS.Business.BUSManager.SetFirmaWS ",
                     " Dettagli: " + ex.Message,
                     ex.InnerException);
                log.Error(mex);
                throw mex;
            }

            return pdfByteFirma;
        }

        public byte[] SetFirmaDouFendSipo(byte[] pdf)
        {
            DouFend proxyTimbro = new DouFend();
            string url = ConfigurationManager.AppSettings["UrlTimbroWS"];
            byte[] pdfByteFirma = new byte[0];
            try
            {

                log.Debug("prima di timbro");
                log.Debug(ConfigurationManager.AppSettings["UrlTimbroWS"]);
                proxyTimbro.Url = url;
                X509Certificate certRequest = new X509Certificate();
                // X509Certificate certRequest = new X509Certificate(bytes, pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                certRequest = SelectCertificate(ConfigurationManager.AppSettings["certNameTimbro"]);
                if (certRequest != null)
                {
                    proxyTimbro.ClientCertificates.Add(certRequest);
                    log.Debug("ho caricato il certificato numero certificati :" + certRequest.Subject + " " + certRequest.GetName());
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    log.Debug("creato request: " + url);   
                    int idc = int.Parse(ConfigurationManager.AppSettings["idcertificatodou"]);
                    if (ConfigurationManager.AppSettings["useproxy"] == "1")
                    {
                        proxyTimbro.Proxy = new WebProxy(ConfigurationManager.AppSettings["ipproxy"], int.Parse(ConfigurationManager.AppSettings["portproxy"]));
                        log.Debug("dentro use proxy");
                    }
                    InputParameters param = new InputParameters();
                    param.idService =idc.ToString();
                    Logo logo = new Logo();
                    logo.paramX = "20";
                    logo.paramY = "700";
                    logo.imagePosition = "R";
                    string _b64 = Convert.ToBase64String(File.ReadAllBytes(ConfigurationManager.AppSettings["imageTimbro"]));
                    logo.image = _b64;
                    param.logo = logo; 
                    string base64StringPDF = Convert.ToBase64String(pdf);
                    param.file = base64StringPDF;
                    Signature signature = new Signature();
                    signature.signType = "AUTOMATIC";
                    param.signature = signature;
                    //using (FileStream fileStream = new FileStream(@"C:\Users\Nick\Documents\CertificatiOnLINE\CertificatiSIPO\NuovoTimbro\fileprova.pdf", FileMode.OpenOrCreate))
                    //{
                    //    fileStream.Write(pdf, 0, pdf.Length);
                    //}
                    var responseDouFend =  proxyTimbro.markAndSignPdf(param);
                    //using (FileStream fileStream = new FileStream(@"C:\Users\Nick\Documents\CertificatiOnLINE\CertificatiSIPO\NuovoTimbro\fileprova1.pdf", FileMode.OpenOrCreate))
                    //{
                    //    byte[] pdfByte = Convert.FromBase64String(param.file);
                    //    fileStream.Write(pdfByte, 0, pdfByte.Length);
                    //}
                    // DOUResponse response = proxyTimbro.createAndUploadWithParam(pdf, idc, null, param, metadata, "", "", "");
                    log.Debug("ho chiamato: ");                   
                    int? status = responseDouFend.status;
                    var s = responseDouFend.reason;
                    if (status != 0)
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + s,
                    "ERR_011",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + s,
                    null);
                        log.Error(mex);
                        throw mex;
                    }
                    else
                    {
                        pdfByteFirma = Convert.FromBase64String(responseDouFend.fileB64content);
                    }
                }
                else
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: Certificato non trovato",
                    "ERR_01B",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: Certificato autenticazione non caricato " + ConfigurationManager.AppSettings["certNameTimbro"],
                    null);
                    log.Error(mex);
                    throw mex;
                }
                log.Debug("dopo di timbro");
            }
            catch (ManagedException ex)
            {

                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_011",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + ex.Message,
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_01B",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + ex.Message,
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                     "ERR_01B",
                     "Certi.WS.Business.BUSManager.SetFirmaWS ",
                     " Dettagli: " + ex.Message,
                     ex.InnerException);
                log.Error(mex);
                throw mex;
            }

            return pdfByteFirma;
        }


        /*
        public byte[] SetFirmaWSSipo(byte[] pdf, string idcertificato)
        {
            SP proxyTimbro = new SP();
            string url = ConfigurationManager.AppSettings["UrlTimbroWS");
            byte[] pdfByteFirma = new byte[0];
    
                try
                {

                    log.Debug("prima di timbro");
                    log.Debug(ConfigurationManager.AppSettings["UrlTimbroWS"));
                    proxyTimbro.Url = url;
                    ServiziTimbroEnum enumeratore = (ServiziTimbroEnum)Enum.Parse(typeof(ServiziTimbroEnum), idcertificato);
                    string service = ((int)enumeratore).ToString();
                    X509Certificate cert = new X509Certificate();
                    cert = SelectCertificate(ConfigurationManager.AppSettings["certNameTimbro"));
                    if (cert != null)
                    {
                        proxyTimbro.ClientCertificates.Add(cert);
                        log.Debug("ho caricato il certificato numero certificati :" + cert.Subject + " " + cert.GetName());
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        log.Debug("creato request: " + url);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        SPServiceResponse response = proxyTimbro.securizePDF(service, pdf, null);
                        log.Debug("ho chiamato: ");
                        pdfByteFirma = response.securedDocument;
                    }
                    else
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: Certificato non trovato",
                        "ERR_01B",
                        "Certi.WS.Business.BUSManager.SetFirmaWS ",
                        " Dettagli: " + "IDCERTIFICATO: " + idcertificato,
                        null);
                        log.Error(mex);
                        throw mex;
                    }
                    log.Debug("dopo di timbro");
                }
                catch (ManagedException ex)
                {

                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                        "ERR_011",
                        "Certi.WS.Business.BUSManager.SetFirmaWS ",
                        " Dettagli: " + "IDCERTIFICATO: " + idcertificato,
                        ex.InnerException);
                    log.Error(mex);
                    throw mex;
                }
                catch (SoapException ex)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                        "ERR_01B",
                        "Certi.WS.Business.BUSManager.SetFirmaWS ",
                        " Dettagli: " + "IDCERTIFICATO: " + idcertificato,
                        ex.InnerException);
                    log.Error(mex);
                    throw mex;
                }
                catch (Exception ex)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Apposizione Timbro dettagli: " + ex.Message,
                         "ERR_01B",
                         "Certi.WS.Business.BUSManager.SetFirmaWS ",
                         " Dettagli: " + "IDCERTIFICATO: " + idcertificato,
                         ex.InnerException);
                    log.Error(mex);
                    throw mex;
                }

                return pdfByteFirma;
            }
    
        */
        public static X509Certificate SelectCertificate(string subjectName)
        {

            // CAPICOM.StoreClass store=new StoreClass();
            X509Certificate certificato = null;
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                // store.Open(CAPICOM.CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE,certStore,CAPICOM.CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_ONLY);
                store.Open(OpenFlags.ReadOnly);
                // 	foreach(Certificate cert in store.Certificates)
                foreach (X509Certificate2 cert in store.Certificates)
                {

                    if (cert.FriendlyName.IndexOf(subjectName) >= 0)
                    {
                        certificato = cert;
                    }

                }
            }
            catch (COMException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (SelectCertificate) Apposizione Timbro dettagli: " + ex.Message,
                    "ERR_01C",
                    "Certi.WS.Business.BUSManager.SetFirmaWS ",
                    " Dettagli: " + "CIU: ",
                    ex.InnerException);
                log.Error(mex);
                throw mex;
            }
            finally
            {
                store.Close();
            }

            return certificato;
        }
    }

}
