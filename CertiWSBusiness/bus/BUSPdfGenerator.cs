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
using System.Xml.Linq;
using System.Linq;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSPdfGenerator
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BUSPdfGenerator));
        /// <summary>
        /// Costruttore
        /// </summary>
        public BUSPdfGenerator()
        {
        }

        /// <summary>
        /// Generazione documento pdf
        /// </summary>
        /// <param name="r">Riga dati certificato</param>
        /// <returns>Esito operazione booleano</returns>
        public bool MakePdf(ProfiloRichiesta.CertificatiRow r, string sistema, int idclient, InfoCertificatoType cert)
        {
            bool resp = false;
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

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(r.XML_CERTIFICATO);

            // TO_DO DA VERIFICARE CAMBIA XML PAGAMENTO
            XmlElement elemP = xd.CreateElement("codicePagamento");
            string pag = r.CODICE_PAGAMENTO;
            string pas = r.T_TIPO_USO;


            ProfiloTipoUso.TipoUsoRow[] tips = (ProfiloTipoUso.TipoUsoRow[])(
                (CacheManager<ProfiloTipoUso.TipoUsoDataTable>.get(CacheKeys.TIPI_USO_WS, VincoloType.NONE)).Select(
                "ID=" + r.TIPO_USO_ID));
            ProfiloClient.ClientsRow[] client = (ProfiloClient.ClientsRow[])(
                CacheManager<ProfiloClient.ClientsDataTable>.get(CacheKeys.CLIENTS_WS, VincoloType.NONE).Select(
                String.Format("Public_ID = '{0}'", sistema.ToUpper())));

            bool dynamicClient = (client[0].Flag_Portale == 1);
            bool pap = (client[0].ID == 4);
            //metodo vecchio
            //if (tips[0].Descrizione != "SENZA DIRITTI DI SEGRETERIA")
            //{
            //    if (string.Equals(sistema.ToUpper(), "PCOM"))
            //    {
            //        XmlDocument xi = new XmlDocument();
            //        xi.LoadXml(r.XML_PAGAMENTO);
            //        pag = String.Concat(pag, "(", xi.SelectSingleNode("//datiTransazione/datiPagamento/importoPagato").InnerText, " Euro)");
            //    }
            //    else //if (string.Equals(sistema.ToUpper(), "PGOV"))
            //    { pag = "pagato allo sportello"; }
            //}
            //else
            //{
            //    pag = "esente da pagamento";
            //}

            if (((tips[0].Descrizione.Contains("<var>"))) && (pap) && (r.T_TIPO_CERTIFICATO.ToLower() == "iscrizione lista generale"))
            {
                // pag = String.Empty;
                pas = tips[0].Descrizione.Replace("<var>", cert.dicituraVariabile);
                pag = string.Empty;
                XmlElement elemTipUso = xd.CreateElement("txtTipoUsoCertificato");
                elemTipUso.InnerXml = pas;
                xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemTipUso);
            }

            else if (dynamicClient)
            {

                if (String.Equals(tips[0].Descrizione, "SENZA DIRITTI DI SEGRETERIA"))
                {
                    pag = String.Empty;

                }
                else
                {
                    if (string.IsNullOrEmpty(r.XML_PAGAMENTO))
                    {
                        throw new InvalidOperationException("Xml pagamento vuoto");
                    }

                    // TO_DO da verificare cambia xml pagamento N.R. 05/2020
                    XDocument xi = XDocument.Parse(r.XML_PAGAMENTO);
                    XElement importoPagato;
                    if (xi.Root.Name.LocalName.Equals("datiTransazione", StringComparison.InvariantCultureIgnoreCase))
                    {
                        importoPagato = (from n in xi.Root.DescendantsAndSelf()
                                                  where n.Name.LocalName.Equals("importoPagato", StringComparison.InvariantCultureIgnoreCase)
                                                  select n).FirstOrDefault();
                    }
                    else
                    {
                        importoPagato = (from n in xi.Root.DescendantsAndSelf()
                                         where n.Name.LocalName.Equals("totale-pagato", StringComparison.InvariantCultureIgnoreCase)
                                         select n).FirstOrDefault();

                    }

                    if (importoPagato == null)
                        throw new InvalidOperationException("Nodo importo pagato non presente nell'xml");

                    pag = String.Format("{0} ({1} Euro)", pag, importoPagato.Value);
                }
            }
            else
            {
                ProfiloDiciturePagamenti.DiciturePagamentiRow[] rowDiciture = (ProfiloDiciturePagamenti.DiciturePagamentiRow[])(
                        CacheManager<ProfiloDiciturePagamenti.DiciturePagamentiDataTable>.get(
                        CacheKeys.DICITURE_PAGAMENTI_WS, VincoloType.NONE).Select(
                        String.Format("Id_Client = {0} and Id_Tipo_Uso = {1}", client[0].ID, r.TIPO_USO_ID)));
                pag = rowDiciture[0].Txt_Tipo_Pagamento;
            }

            elemP.InnerXml = pag;
            xd.DocumentElement.SelectSingleNode("//indice").AppendChild(elemP);

            try
            {
                IFormatter fo = FormatterProvider.formatDocument("PDF");
                System.IO.MemoryStream mPdf = fo.formatData(xd, SetLayoutFrags(chiave, idclient));
                IDictionary<string, string> d = new Dictionary<string, string>();
                d.Add("subject", r.T_TIPO_CERTIFICATO);
                d.Add("author", "Roma Capitale");
                d.Add("creator", "Certificati Online");
                r.T_DOCUMENTO = PdfModifier.SetMetadati(mPdf, d);
                resp = true;
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness)"
                    + " di generazione certificato con ID = " + r.ID + " " + ex.Message,
                    "ERR_149",
                    "Certi.WS.Business.BUSManager",
                    "MakePdf",
                    "Generazione PDF",
                    string.Empty,
                    "CIU: " + r.CIU,
                     ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }

        /// <summary>
        /// Preparazione lista con i frammenti del foglio di trasformazione xslt
        /// </summary>
        /// <param name="frag">frammenti di xml che compongono il foglio xslt</param>
        /// <returns>Lista di stringhe</returns>
        private IList<string> SetLayoutFrags(string frag, int client)
        {
            IList<string> frags = new List<string>();
            frags.Add("Testa");
            frags.Add(frag);
            if (client == 3)
            {
                frags.Add("Coda_PA");
            }
            else
            {
                frags.Add("Coda");
            }
            return frags;
        }
    }

}
