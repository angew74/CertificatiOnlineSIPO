using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Configuration;
using Com.Unisys.CdR.Certi.WS.Dati;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.Logging;
using log4net;
using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.WS.Business.bus;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSXmlGenerator
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BUSXmlGenerator));
        /// <summary>
        /// Costruttore
        /// </summary>
        public BUSXmlGenerator()
        {
        }

        /// <summary>
        /// Generazione documento xml
        /// </summary>
        /// <param name="r">riga contenente i dati certificato</param>
        /// <param name="datiMapper">risposta dati dal backend</param>
        /// <param name="dtEmissione">data ora estrazione dati (data emissione)</param>
        /// <returns>Documento xml contente i dati del certificato richiesto</returns>
        public XmlDocument MakeXml(ProfiloRichiesta.CertificatiRow r, string datiMapper, DateTime dtEmissione, int clientID, string ufficioID)
        {
            XmlDocument xd = new XmlDocument();
            string chiave = String.Empty;
            FogliCertiConfigSection section = (FogliCertiConfigSection)ConfigurationManager.GetSection("FogliCerti");
            if (section != null)
            {
                for (int i = 0; i < section.FogliCertiItems.Count; i++)
                    if (section.FogliCertiItems[i].Name.Equals(r.T_TIPO_CERTIFICATO))
                    {
                        chiave = section.FogliCertiItems[i].Value;
                    }
            }


            CacheKeys key = (CacheKeys)Enum.Parse(typeof(CacheKeys), String.Concat(chiave, "_TO_XML"));
            XslCompiledTransform xslToXml = CacheManager<XslCompiledTransform>.get(key, VincoloType.FILESYSTEM);

            try
            {
                xd = MapperDeserializer.PlainToVerificaXML(datiMapper, xslToXml);
                ProfiloTipoUso.TipoUsoRow[] tips = (ProfiloTipoUso.TipoUsoRow[])(
                (CacheManager<ProfiloTipoUso.TipoUsoDataTable>.get(CacheKeys.TIPI_USO_WS, VincoloType.NONE)).Select(
                "ID=" + r.TIPO_USO_ID));

                switch (tips[0].Descrizione)
                {
                    case "SENZA DIRITTI DI SEGRETERIA":
                        if (r.TIPO_CERTIFICATO_ID != 1 && r.TIPO_CERTIFICATO_ID != 2 && r.TIPO_CERTIFICATO_ID != 3 && r.TIPO_CERTIFICATO_ID != 21)
                        { goto case "CARTA BOLLATA"; }
                        break;
                    case "CARTA SEMPLICE":
                    case "CARTA BOLLATA":
                        string query = String.Format("Id_Client = {0} and Id_Tipo_Uso = {1}", clientID, r.TIPO_USO_ID);
                        ProfiloDiciturePagamenti.DiciturePagamentiRow[] rowDicitura = 
                            (ProfiloDiciturePagamenti.DiciturePagamentiRow[])(
                            CacheManager<ProfiloDiciturePagamenti.DiciturePagamentiDataTable>.get(
                            CacheKeys.DICITURE_PAGAMENTI_WS, VincoloType.NONE).Select(query));
                        XmlElement elemT = xd.CreateElement("txtTipoUsoCertificato");
                        elemT.InnerText = rowDicitura[0].Txt_Tipo_Uso_Certificato;
                        xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemT);
                        break;
                }

                //metodo vecchio
                //if (r.TIPO_USO_ID == 2) //(int)IDUsoType.BOLLO)
                //{
                //    XmlElement elemT = xd.CreateElement("txtTipoUsoCertificato");
                //    elemT.InnerText = Config.ReadSetting("RiscossoBollo");
                //    xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemT);
                //}
                //else if (r.TIPO_USO_ID == 1) //(int)IDUsoType.SEMPL)
                //{
                //    XmlElement elemT = xd.CreateElement("txtTipoUsoCertificato");
                //    elemT.InnerText = Config.ReadSetting("RiscossoSempl");
                //    xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemT);
                //}
                //else if (r.TIPO_USO_ID == 3 &&
                //    (r.TIPO_CERTIFICATO_ID != 1 && r.TIPO_CERTIFICATO_ID != 2 && r.TIPO_CERTIFICATO_ID != 3))
                //{
                //    XmlElement elemT = xd.CreateElement("txtTipoUsoCertificato");
                //    elemT.InnerText = ConfigurationManager.AppSettings.Get("RiscossoSenzaDirittiSegr");
                //    xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemT);
                //}

                XmlElement elemD = xd.CreateElement("dataEmissione");
                elemD.InnerText = dtEmissione.ToString("dd/MM/yyyy");
                xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemD);

                System.Globalization.CultureInfo cult = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.TextInfo textInfo = cult.TextInfo;
                string descrizione = (CacheManager<ProfiloClient.ClientsDataTable>.get(
                    CacheKeys.CLIENTS_WS, VincoloType.NONE).Select("ID = " + clientID)[0]
                    as ProfiloClient.ClientsRow).Descrizione;

                XmlElement elemP = (XmlElement)xd.DocumentElement.SelectSingleNode("//indice/ufficioRichiesta");
                if (elemP == null)
                {
                    elemP = xd.CreateElement("ufficioRichiesta");
                    elemP.InnerText = textInfo.ToTitleCase(descrizione.ToLower()) + " " + ufficioID;
                    xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemP);
                }
                else
                {
                    elemP.InnerText = textInfo.ToTitleCase(descrizione.ToLower()) + " " + ufficioID;
                }
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_170",
                    "Certi.WS.Business.BUSManager",
                    "MakeXml",
                    "Generazione PDF",
                    string.Empty,
                    "CIU :" + r.CIU,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return xd;
        }



        /// <summary>
        /// Generazione ciu e inserimento di quest'ultimo nel documento xml di partenza 
        /// </summary>
        /// <param name="docIn">documento xml sul quale generare il ciu</param>
        /// <returns>Documento xml di ritorno (aggiornato con il ciu prodotto)</returns>
        public XmlDocument GetCIU(XmlDocument docIn)
        {
            XmlDocument docOut = new XmlDocument();
            try
            {
                Ticket tk = TicketHelper.Instance.getNewTicket(docIn.OuterXml);
                docOut = (XmlDocument)docIn.Clone();
                XmlElement elemC = docOut.CreateElement("ciu");
                elemC.InnerText = tk.CIU.ToString();
                docOut.DocumentElement.SelectSingleNode("//indice").AppendChild(elemC);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore generazione CIU dettagli:" + ex.Message,
                    "ERR_172",
                    "Certi.WS.Business.BUSXmlGenerator",
                    "GetCIU",
                    "Generazione CIU ed inserimento in xml di ritorno",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return docOut;
        }
    }
}
