using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WaveKit;
using ImageKit;
using log4net;
using System.ComponentModel;
using System.Drawing;
using System.Web.SessionState;
using System.Xml;
using Com.Unisys.CdR.Certi.WebApp.Business;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.Objects.Common;



namespace Com.Unisys.CdR.Certi.WebApp.recupero
{
    public partial class Consegna : Com.Unisys.CdR.Certi.WebApp.common.BasePage
    {
        private static readonly ILog log = LogManager.GetLogger("Consegna");

        protected System.Web.UI.WebControls.LinkButton LinkButton1;
        BUSFlussoRichiesta flu;
        private ProfiloDownload.CertificatiDataTable certs;
        private int validatorControlled = 0;

        // valori variabile validatorControlled
        // 0 -	nessun controllo
        // 1 -	required validator ok
        // 2 -	validator1 ok
        // 3 -	validator1 ko
        // 4 -  validator2 ok
        // 5 -  validator2 ko
        // 6  - validator2 tentato il controllo ma non risponde la base dati
        //		per verificare se esiste il CIU richiesto in archivio


        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Request.Browser.Cookies)
                {
                    info.AddMessage("ATTENZIONE: Per utilizzare questa pagina il browser deve supportare i Cookies.", LivelloMessaggio.WARNING);
                    fase.Value = "-1";
                }
                else
                {
                    if (Session["SESSIONE_ID"] == null)
                        Session["SESSIONE_ID"] = this.Session.SessionID;
                    fase.Value = "0";
                }
            }
            if (Session["SESSIONE_ID"] != null)
            {
                //disabilitazione link e bottoni recupero per eccessivi tentativi errati
                if ((Request.Cookies["certi"] != null) && (int.Parse(Request.Cookies["certi"].Value) > 4))
                {
                    info.AddMessage("Per eccessivo numero di tentativi errati,"
                                + " questo client non potrà più accedere al sistema per 20 minuti.",
                                LivelloMessaggio.WARNING);
                    HyperLink2.Enabled = false;
                    SubmitButton.Enabled = false;
                }

                string cf = Request.ServerVariables["HTTP_IV_USER"];
                if (bool.Parse(ConfigurationManager.AppSettings["TEST"]))
                {
                    cf = ConfigurationManager.AppSettings["TEST_ACCOUNT"];
                }
                if (SessionManager<ProfiloUtente>.exist(SessionKeys.RICHIEDENTE_CERTIFICATI) &&
                        ((ProfiloUtente)SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI)).CodiceFiscale.ToUpper().Equals(cf.ToUpper()))
                    richiedente = SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI);

                if (SessionManager<String>.exist(SessionKeys.ESITO_PAGAMENTO))
                    SessionManager<String>.del(SessionKeys.ESITO_PAGAMENTO);
            }
            else 
            {
                    info.AddMessage("ATTENZIONE: Per visualizzare questa pagina il Browser deve consentire l'uso dei Cookies.",LivelloMessaggio.WARNING);
                    fase.Value = "-1";
            }
        }


        protected override void Page_PreRender(object sender, System.EventArgs e)
        {
            base.Page_PreRender(sender, e);
           /* UnisysPortaleCdR myMaster = (UnisysPortaleCdR)this.Master;
            (myMaster.FindControl("hyp_EMI") as HyperLink).Visible = false;
            (myMaster.FindControl("hyp_EMI") as HyperLink).Enabled = false;
            (myMaster.FindControl("hyp_RIT") as HyperLink).Visible = false;
            (myMaster.FindControl("hyp_RIT") as HyperLink).Enabled = false;*/

            Com.Unisys.Logging.Certi.CertiLogInfo inf;

            if (Page.IsPostBack)
            {
                switch (validatorControlled)
                {
                    // nessun controllo
                    case 0:
                        break;
                    // required validator ok
                    case 1:
                        break;
                    // validator1 ok
                    case 2:
                        this.CodeNumberTextBox.Text = "";
                        break;
                    // 3 -	validator1 ko
                    case 3:
                        this.CodeNumberTextBox.Text = "";
                        info.AddMessage("Il codice alfanumerico inserito non corrisponde a quello presente nell'immagine",
                           LivelloMessaggio.WARNING);
                        break;
                    // 4 -  validator2 ok
                    case 4:
                        if (Request.Cookies["certi"] == null)
                        { addCookie(false); }
                        else
                        { renewCookie(false); }
                        break;
                    // 5 -  validator2 ko
                    case 5:
                        if (Request.Cookies["certi"] == null)
                        { addCookie(true); }
                        else
                        { renewCookie(true); }
                        int num = int.Parse(Response.Cookies["certi"].Value);
                        info.AddMessage(String.Concat("Codice CIU errato."
                                        + " <br/>Dopo 5 tentativi consecutivi questo client"
                                        + " non potrà più accedere al sistema per 20 minuti."
                                        + " <br/>Numero tentativi ancora disponibili (",
                                        (5 - num).ToString(), ")"),
                            LivelloMessaggio.WARNING);
                        if (num > 4)
                        {
                            inf = new Com.Unisys.Logging.Certi.CertiLogInfo();
                            inf.logCode = "BLC";
                            inf.loggingAppCode = "CWA";
                            inf.flussoID = "";
                            inf.clientID = ConfigurationManager.AppSettings["ClientID"];
                            inf.activeObjectCF = Request.ServerVariables["HTTP_IV_USER"].ToUpper();
                            inf.activeObjectIP = Request.ServerVariables["HTTP_IV_REMOTE_ADDRESS"];
                            inf.passiveObjectCF = "";
                            log.Info(inf);

                            HyperLink2.Enabled = false;
                            SubmitButton.Enabled = false;
                        }
                        break;
                    // 6  - validator2 tentato il controllo ma non risponde la base dati
                    //		per verificare se esiste il CIU richiesto in archivio
                    case 6:
                        info.AddMessage("Non è stato possibile stabilire una connessione con il sistema centrale. Per favore ripeti l'operazione.",
                            LivelloMessaggio.WARNING);
                        break;
                }
                base.Page_PreRender(sender, e);
            }

            switch (fase.Value)
            {
                case "-1":
                    pnlCaptcha.Visible = false;
                    pnlCaptchaRo.Visible = false;
                    pnlCertificato.Visible = false;
                    pnlRitiri.Visible = false;
                    break;
                case "0":
                    pnlCaptcha.Visible = true;
                    pnlCaptchaRo.Visible = false;
                    pnlCertificato.Visible = false;
                    pnlRitiri.Visible = false;
                    break;
                case "1":
                    lblCIU.Text = CiuTextbox.Text.Trim();
                    lblTipoCertificato.Text = tipoCertificato.Value;
                    lblIntestatario.Text = intestatario.Value;
                    lblData.Text = dataEmissione.Value;
                    lblCodicePagamento.Text = !String.IsNullOrEmpty(codicePagamento.Value) ?
                        "<b>Codice del pagamento:</b> " + codicePagamento.Value : String.Empty;
                    linkCerti.Enabled = true;
                    linkCerti.Visible = true;
                    pnlCaptcha.Visible = false;
                    pnlCaptchaRo.Visible = true;
                    pnlCertificato.Visible = true;
                    pnlRitiri.Visible = true;
                    break;
                case "2":
                    lblCIU.Text = CiuTextbox.Text.Trim();
                    lblTipoCertificato.Text = tipoCertificato.Value;
                    lblIntestatario.Text = intestatario.Value;
                    lblData.Text = dataEmissione.Value;
                    lblCodicePagamento.Text = "<b>Codice del pagamento:</b> " + codicePagamento.Value;
                    lblSoggetto.Text = "<b>Certificato ritirato da:</b> " + soggetto.Value;
                    linkCerti.Enabled = false;
                    linkCerti.Visible = false;
                    pnlCaptcha.Visible = false;
                    pnlCaptchaRo.Visible = true;
                    pnlCertificato.Visible = true;
                    pnlRitiri.Visible = false;
                    break;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           this.CustomValidator1.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.CustomValidator1_ServerValidate);
           this.CustomValidator2.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.CustomValidator2_ServerValidate);
        }
        #endregion

        protected void linkCerti_Click(object sender, EventArgs e)
        {
            if (fase.Value.Equals("-1"))
                return;
            string cf1 = Request.ServerVariables["HTTP_IV_USER"];
            string ip = Request.ServerVariables["HTTP_IV_REMOTE_ADDRESS"];
            if (bool.Parse(ConfigurationManager.AppSettings["TEST"]))
            {
                cf1 = ConfigurationManager.AppSettings["TEST_ACCOUNT"];
                ip = ConfigurationManager.AppSettings["TEST_IP"];
            }
            flu = new BUSFlussoRichiesta();
            byte[][] resp = flu.GetPDF(cf1.ToUpper(), CiuTextbox.Text.Trim(), ip);
            if (resp[0][0] == 0)
            {
                Com.Unisys.Logging.Certi.CertiLogInfo inf = new Com.Unisys.Logging.Certi.CertiLogInfo();
                inf.logCode = "DOW";
                inf.loggingAppCode = "CWA";
                inf.flussoID = "";
                inf.clientID = ConfigurationManager.AppSettings["ClientID"];
                inf.activeObjectCF = cf1.ToUpper();
                inf.activeObjectIP = ip;
                inf.passiveObjectCF = "";
                log.Info(inf);


                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=certificato.pdf");
                this.Response.BinaryWrite(resp[1]);
                this.Response.End();
            }
        }

        protected void btnRitiro_Click(object sender, EventArgs e)
        {
            if(fase.Value.Equals("-1"))
                return;
            flu = new BUSFlussoRichiesta();
            int n = flu.SignRitirato(CiuTextbox.Text.Trim(), txtSoggetto.Text.Trim().ToUpper());
            if (n > 0)
            {
                Com.Unisys.Logging.Certi.CertiLogInfo inf = new Com.Unisys.Logging.Certi.CertiLogInfo();
                inf.logCode = "RIT";
                inf.loggingAppCode = "CWA";
                inf.flussoID = "";
                inf.clientID = ConfigurationManager.AppSettings["ClientID"];
                inf.activeObjectCF = Request.ServerVariables["HTTP_IV_USER"].ToUpper();
                inf.activeObjectIP = Request.ServerVariables["HTTP_IV_REMOTE_ADDRESS"];
                inf.passiveObjectCF = "";
                log.Info(inf);

                soggetto.Value = txtSoggetto.Text.Trim().ToUpper();
                fase.Value = "2";
            }
        }

        // submit 
        protected void SubmitButton_Click(object sender, System.EventArgs e)
        {
            if (fase.Value.Equals("-1"))
                return;
            Page.Validate();
        }

        // validazione captcha
        private void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (validatorControlled == 0)
            {
                Requiredfieldvalidator1.Validate();
                Requiredfieldvalidator2.Validate();
                if (Requiredfieldvalidator1.IsValid && Requiredfieldvalidator2.IsValid)
                {
                    validatorControlled = 1;
                    args.IsValid = (this.CodeNumberTextBox.Text.Trim().ToUpper().Equals(
                        ImageKit.CaptchaHandler.getCaptchaText().Trim().ToUpper()));
                    validatorControlled = (args.IsValid) ? 2 : 3;
                }
            }
        }

        // validazione CIU
        private void CustomValidator2_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (validatorControlled == 0) CustomValidator1.Validate();
            if (validatorControlled == 2)
            {
                try
                {
                    flu = new BUSFlussoRichiesta();
                    certs = flu.GetCertificatoByCIU(CiuTextbox.Text.Trim());
                    args.IsValid = (certs.Rows.Count > 0) ? true : false;
                    validatorControlled = (args.IsValid) ? 4 : 5;
                    if (args.IsValid)
                    {
                        tipoCertificato.Value = certs[0].T_TIPO_CERTIFICATO;
                        intestatario.Value = certs[0].T_COGNOME_NOME_INTESTATARIO;
                        dataEmissione.Value = certs[0].T_DATA_EMISSIONE.ToString("dd/MM/yyyy");
                        codicePagamento.Value = certs[0].CODICE_PAGAMENTO;
                        soggetto.Value = certs[0].SOGGETTO_RITIRO;

                        fase.Value = ((int)certs[0].STATUS_ID == (int)Status.C_GENERAZIONE_PDF_OK) ? "1" :
                            ((int)certs[0].STATUS_ID == (int)Status.C_RITIRATO) ? "2" : "0";
                    }
                }
                catch (ManagedException)
                {
                    validatorControlled = 6;
                }
                catch (Exception)
                {
                    validatorControlled = 6;
                }
            }
        }

        // nuovo cookie
        private void addCookie(bool block)
        {
            HttpCookie certiCookie = new HttpCookie("certi");
            if (block == true) certiCookie.Value = "1";
            else certiCookie.Value = "0";
            certiCookie.Expires = DateTime.Now.AddMinutes(20);
            Response.Cookies.Add(certiCookie);
        }

        // rinnovo cookie
        private void renewCookie(bool block)
        {
            int counter = 0;
            if (block == true)
            {
                counter = int.Parse(Request.Cookies["certi"].Value);
                counter++;
            }
            Response.Cookies["certi"].Value = (counter).ToString();
            Response.Cookies["certi"].Expires = DateTime.Now.AddMinutes(20);
        }
    }
}
