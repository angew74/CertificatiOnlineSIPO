using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using log4net;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.WebApp.Business;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.WebApp.emissione
{
    public partial class Ritiro : Com.Unisys.CdR.Certi.WebApp.common.BasePage
    {
        private static readonly ILog log = LogManager.GetLogger("Ritiro");
        BUSFlussoRichiesta flu;

        /// <summary>
        /// Verifica la presenza del richiedente in sessione
        /// La prima volta vengono ricaricati i dati del richiedente dal portale comunale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRichiedenteFromSession();
            if (!IsPostBack)
            {
                LoadRitiro();
            } 

            if (SessionManager<String>.exist(SessionKeys.ESITO_PAGAMENTO))
                SessionManager<String>.del(SessionKeys.ESITO_PAGAMENTO);
        }

        /// <summary>
        /// Gestisce la visualizzazione dei vari oggetti in pagina 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_PreRender(object sender, EventArgs e)
        {
            base.Page_PreRender(sender, e);
            UnisysPortaleCdR myMaster = (UnisysPortaleCdR)this.Master;
            (myMaster.FindControl("hyp_CON") as HyperLink).Visible = false;
            (myMaster.FindControl("hyp_CON") as HyperLink).Enabled = false;
            // tolto da Nico 2/2011 
            /*if (String.IsNullOrEmpty(richiedente.CodiceIndividuale))
            {
                pnlCertificati.Visible = false;
            }*/

            if (rptCertificati.Items.Count > 0)
            {
                lblPresenza.Visible = false;
            }
            else
            {
                /* certificati non presenti */
                lblPresenza.Text = "Non ci sono certificati da ritirare";
                lblPresenza.CssClass = "titoloCertiError";
            }
        }


        /// <summary>
        /// Caricamento dati del richiedente dalla sessione
        /// </summary>
        private void LoadRichiedenteFromSession()
        {
            string cf = Request.ServerVariables["HTTP_IV_USER"];
            if (bool.Parse(Config.ReadSetting("TEST")))
            {
                cf = Config.ReadSetting("TEST_ACCOUNT");
            }
            if (String.IsNullOrEmpty(cf))
                base.LogOff();
            bool ricercaSession = SessionManager<ProfiloUtente>.exist(SessionKeys.RICHIEDENTE_CERTIFICATI);
            if (!ricercaSession)
            {
                Response.Redirect("Emissione.aspx");
            }
            else
            {
                richiedente = SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI);
                if (!(richiedente.CodiceFiscale.ToUpper().Equals(cf.ToUpper())))
                    Response.Redirect("Emissione.aspx");
            }
        }


        /// <summary>
        /// Caricamento certificati emessi per il richiedente connesso
        /// </summary>
        private void LoadRitiro()
        {
            flu = new BUSFlussoRichiesta();
            BusFlussoPagamento flussoPagamento = new BusFlussoPagamento();
            try
            {
                // modifica NR 15/05/2020             
                log.Debug("prima di controlla pagamento rest");
                rptCertificati.DataSource = flussoPagamento.ControllaPagamento(richiedente.CodiceFiscale, Config.ReadSetting("ClientID"), "");
                rptCertificati.DataBind();
            }
            catch (ManagedException ex)
            {
                info.AddMessage(ex.Message, LivelloMessaggio.WARNING);
                //info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                //                    LivelloMessaggio.WARNING);
                throw;
            }
        }


        /// <summary>
        /// Recupero documento richiesto in storage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkProduci_Click(object sender, EventArgs e)
        {
            Button lb = (Button)sender;
            string ciu = lb.CommandArgument.ToString();
            flu = new BUSFlussoRichiesta();
            try
            {
                // N.R. MODIFICATO IL SERVIZIO 09/2020
                byte[][] resp = flu.GetPDF(richiedente.CodiceFiscale, ciu, richiedente.IndirizzoIP);              
                if (resp[0][0] != 0 || resp[1].Length == 0)
                {                   
                    if (flu.ExecuteAfterPaymentRichiesta(richiedente.CodiceFiscale, ciu, richiedente.IndirizzoIP))
                        resp = flu.GetPDF(richiedente.CodiceFiscale, ciu, richiedente.IndirizzoIP);                   

                }

                if (resp[0][0] != 0 || resp[1].Length == 0)
                {
                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                    LivelloMessaggio.WARNING);
                }
                else
                {
                    Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                    info.freeTextDetails = "CIU: " + ciu;
                    info.logCode = "PRO";
                    info.loggingAppCode = "CWA";
                    info.flussoID = String.Empty;
                    info.clientID = Config.ReadSetting("ClientID");
                    info.activeObjectCF = richiedente.CodiceFiscale;
                    info.activeObjectIP = richiedente.IndirizzoIP;
                    info.passiveObjectCF = String.Empty;
                    log.Info(info);

                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AppendHeader("Content-Disposition", "attachment; filename=certificato.pdf");
                    this.Response.BinaryWrite(resp[1]);
                    this.Response.End();
                }

            }
            catch (ManagedException ex)
            {
                info.AddMessage(ex.Message, LivelloMessaggio.WARNING);
                log.Error(ex);
                //info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                //                  LivelloMessaggio.WARNING);
            }
        }


        /// <summary>
        /// Recupero documento richiesto in storage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkRecup_Click(object sender, EventArgs e)
        {
            Button lb = (Button)sender;
            string ciu = lb.CommandArgument.ToString();
            flu = new BUSFlussoRichiesta();
            try
            {
                byte[][] resp = flu.GetPDF(richiedente.CodiceFiscale, ciu, richiedente.IndirizzoIP);
                if (resp[0][0] != 0 || resp[1].Length == 0)
                {
                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                    LivelloMessaggio.WARNING);
                }
                else
                {
                    Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                    info.freeTextDetails = "CIU: " + ciu;
                    info.logCode = "REC";
                    info.loggingAppCode = "CWA";
                    info.flussoID = String.Empty;
                    info.clientID = Config.ReadSetting("ClientID");
                    info.activeObjectCF = richiedente.CodiceFiscale;
                    info.activeObjectIP = richiedente.IndirizzoIP;
                    info.passiveObjectCF = String.Empty;
                    log.Info(info);

                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AppendHeader("Content-Disposition", "attachment; filename=certificato.pdf");
                    this.Response.BinaryWrite(resp[1]);
                    this.Response.End();
                }
            }
            catch (ManagedException)
            {
                info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                    LivelloMessaggio.WARNING);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptCertificati_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            decimal stat = -1;
            decimal tpUso = -1;
            string ciu = String.Empty;
            string codPag = String.Empty;
            string data = String.Empty;
            string strInt = String.Empty;
            string strCIU = String.Empty;
            string strPag = String.Empty;
            string strData = String.Empty;
            bool firstTrip = true;

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                ProfiloDownload.CertificatiRow dtr = (ProfiloDownload.CertificatiRow)(e.Item.DataItem);
                stat = dtr.STATUS_ID;
                tpUso = dtr.TIPO_USO_ID;

                /* data, ciu,link produci - recupera*/
                if (stat == (int)Status.C_GENERAZIONE_PDF_OK || stat == (int)Status.C_RITIRATO)
                {
                    data = dtr.T_DATA_EMISSIONE.ToString("dd/MM/yyyy HH:mm:ss");
                    ciu = dtr.CIU;
                    strData = String.Concat("Emesso il: <b>", data, "</b><br/>");
                    strCIU = String.Concat("CIU : <b>", ciu, "</b><br/>");
                    firstTrip = false;
                }
                else
                {
                    data = dtr.T_DATA_EMISSIONE.ToString("dd/MM/yyyy HH:mm:ss");
                    strData = String.Concat("Richiesto il: <b>", data, "</b><br/>");
                    strCIU = String.Concat("CIU : <b>NON ANCORA DISPONIBILE", "</b><br/>");
                }

                /* codice pagamento */
                if (tpUso == 3) //da sistemare
                {
                    codPag = String.Empty;
                    strPag = String.Concat("Codice pagamento: <b>ESENTE DA PAGAMENTO", "</b><br/>");
                }
                else if (stat == (int)Status.C_RICHIESTA_PAGAMENTO)
                {
                    codPag = String.Empty;
                    strPag = String.Concat("Codice pagamento: <b>NON ANCORA RICEVUTO", "</b><br/>");
                }
                else
                {
                    codPag = dtr.CODICE_PAGAMENTO;
                    strPag = String.Concat("Codice pagamento: <b>", codPag, "</b><br/>");
                }
                Label l = (Label)e.Item.FindControl("lblDettagli");
                strInt = String.Concat("Intestatario: <b>",
                        dtr.T_COGNOME_NOME_INTESTATARIO,
                         "</b><br/>");
                l.Text = String.Concat(strInt, strData, strCIU, strPag).ToString();

                Button lbp = (Button)e.Item.FindControl("linkProduci");

                if (firstTrip && (tpUso == 3 || !String.IsNullOrEmpty(codPag)))
                {
                    lbp.Enabled = true;
                    lbp.CssClass = "extButton";
                }
                else
                {
                    lbp.Enabled = false;
                    lbp.CssClass = "extButtonDisable";
                }

                Button lbr = (Button)e.Item.FindControl("linkRecup");

                if (!firstTrip && (tpUso == 3 || !String.IsNullOrEmpty(codPag)))
                {
                    lbr.Enabled = true;
                    lbr.CssClass = "extButton";
                }
                else
                {
                    lbr.Enabled = false;
                    lbr.CssClass = "extButtonDisable";
                }
            }
        }

        protected string GetProduttoreCertificato(string xmlCertificato)
        {
            string produttore = string.Empty;
            if (!string.IsNullOrEmpty(xmlCertificato))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlCertificato);
                string[] prodotto = xmlDoc.SelectSingleNode("//indice/ufficioRichiesta").InnerText.Split(new char[] { ' ' });
                produttore = "(" + prodotto[0] + ")";
            }
            
            return produttore;
        }

        protected void aggiornaPagamento_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}
