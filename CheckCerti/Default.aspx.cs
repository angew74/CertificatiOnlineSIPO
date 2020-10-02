using System;
using System.Configuration;
using System.Web.UI;
using CheckCerti.WSCertificati;
using Microsoft.Web.Services2.Security.Tokens;
using Unisys.CdR.Servizi;
using System.Web.Services.Protocols;

namespace CheckCerti
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                txtWebService.Text = ConfigurationManager.AppSettings["UrlWebService"];
                txtCodiceFiscale.Text = ConfigurationManager.AppSettings["CodiceFiscaleintestatario"];
                txtClientId.Text = ConfigurationManager.AppSettings["clientid"];
                txtUrlPagamenti.Text = ConfigurationManager.AppSettings["UrlControlloPagamenti"];
                txtSistema.Text = ConfigurationManager.AppSettings["Sistema"];
                txtRichiedente.Text = ConfigurationManager.AppSettings["CodiceFiscalerichiedente"];
                txtUserName.Text = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            }

        }

        protected void btnCercaPersona_OnClick(object sender, EventArgs e)
        {

           CertiService ws = new CertiService();
           Risultato.Text = string.Empty;
            ws.Url = txtWebService.Text;
            string key = ConfigurationManager.AppSettings["Chiave"];
            string username = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            string password = Encryption.Decrypt(ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["Chiave"]);   
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            UsernameToken myToken = new UsernameToken(username, Com.Unisys.CdR.Certi.Utils.Config.PlainToSHA1(password), PasswordOption.SendPlainText);
            ws.RequestSoapContext.Security.Tokens.Add(myToken);
            try
            {
                string resp = ws.ricercaPersona(int.Parse(txtClientId.Text), txtCodiceFiscale.Text);
                Risultato.Text ="Chiamata risposta: " + resp;
            }
            catch (Exception ex)
            {
                Risultato.Text = ex.Message;
            }
        }

        protected void btnCercaComponenti_OnClick(object sender, EventArgs e)
        {
            CertiService ws = new CertiService();   
            ws.Url = txtWebService.Text;
            Risultato.Text = string.Empty;
            string key = ConfigurationManager.AppSettings["Chiave"];
            string username = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            string password = Encryption.Decrypt(ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["Chiave"]);
            try
            {
                System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                UsernameToken myToken = new UsernameToken(username, Com.Unisys.CdR.Certi.Utils.Config.PlainToSHA1(password), PasswordOption.SendPlainText);
                ws.RequestSoapContext.Security.Tokens.Add(myToken);        
                ComponenteFamigliaType[] resp = ws.ricercaComponentiFamiglia(int.Parse(txtClientId.Text), txtCodiceFiscale.Text);
                if (resp.Length > 0)
                {
                    for (int i = 0; i < resp.Length; i++)
                    {
                       Risultato.Text += "Componente \n";
                       Risultato.Text += resp[0].codiceFiscale + " ";
                       Risultato.Text += resp[0].codiceIndividuale + " ";
                       Risultato.Text += resp[0].cognome + " " + resp[0].nome + " ";
                       Risultato.Text += resp[0].rapportoParentela + "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Risultato.Text = ex.Message;
            }

        }

        protected void btnCheckPagamenti_OnClick(object sender, EventArgs e)
        {
           
            try
            {
                
                string[] listaCIU = new string[]{ConfigurationManager.AppSettings["CIU"]};
                Com.Unisys.CdR.Certi.WebApp.Business.ProxyPagamentiWS.Pagamento_Certificati ck = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyPagamentiWS.Pagamento_Certificati();
                ck.Url = ConfigurationManager.AppSettings["UrlControlloPagamenti"];
                System.Xml.XmlNode rp = ck.ListaTransazioni(listaCIU);
                Risultato.Text = "Controllo pagamenti: " + rp.InnerXml;
            }
            catch (Exception ex)
            {
                Risultato.Text = ex.Message;
            }
        }

        protected void btnCheck_Credenziali_OnClick(object sender, EventArgs e)
        {

            CertiService ws = new CertiService();
            Risultato.Text = string.Empty;
            ws.Url = txtWebService.Text;
            string key = ConfigurationManager.AppSettings["Chiave"];
            string username = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            string password = Encryption.Decrypt(ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["Chiave"]);
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            UsernameToken myToken = new UsernameToken(username, Com.Unisys.CdR.Certi.Utils.Config.PlainToSHA1(password), PasswordOption.SendPlainText);
            ws.RequestSoapContext.Security.Tokens.Add(myToken);          
            TransactionRequestType tt = new TransactionRequestType();
            tt.codiceFiscaleRichiedente = ConfigurationManager.AppSettings["CodiceFiscalerichiedente"];
            tt.codiceFiscaleIntestatario = ConfigurationManager.AppSettings["CodiceFiscaleintestatario"];
            tt.idTransazione = "00708002011012500030003";
            tt.idPod = "";
            tt.sistema =ConfigurationManager.AppSettings["Sistema"];         
            bool cc = false;           
            try
            {
                CredenzialiType resp = ws.richiestaCredenziali(tt, out cc);
                Risultato.Text += "Codice fiscale intestatario: " + resp.transactionData.codiceFiscaleIntestatario + "\n";
                Risultato.Text += "Codice fiscale richiedente: " + resp.transactionData.codiceFiscaleRichiedente + "\n";
                Risultato.Text += "Id transazione:  " +  resp.transactionData.idTransazione + "\n";
                Risultato.Text +="Id flusso: " +  resp.idFlusso;
                txtIDFlusso.Text = resp.idFlusso;
            }
            catch (SoapException ex)
            {
                Risultato.Text = ex.Message;
            }
        }

        protected void btnCheckEmettibilita_OnClick(object sender, EventArgs e)
        {
            CertiService ws = new CertiService();
            ws.Url = txtWebService.Text;
            Risultato.Text = string.Empty;
            string key = ConfigurationManager.AppSettings["Chiave"];
            string username = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            string password = Encryption.Decrypt(ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["Chiave"]);
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            UsernameToken myToken = new UsernameToken(username, Com.Unisys.CdR.Certi.Utils.Config.PlainToSHA1(password), PasswordOption.SendPlainText);
            ws.RequestSoapContext.Security.Tokens.Add(myToken);      
            CredenzialiType cc = new CredenzialiType();
            cc.transactionData = new TransactionRequestType();
            cc.transactionData.codiceFiscaleRichiedente = txtRichiedente.Text;
            cc.transactionData.codiceFiscaleIntestatario = txtCodiceFiscale.Text;
            cc.transactionData.idTransazione = "00708002011012500030003";
            cc.transactionData.idPod = "";
            cc.transactionData.sistema = ConfigurationManager.AppSettings["Sistema"];
            cc.idFlusso = txtIDFlusso.Text;    
            InfoCertificatoType[] ic = new InfoCertificatoType[1];
            ic[0] = new InfoCertificatoType();
            ic[0].idNomeCertificato = "C0001";
            ic[0].IdUso = "3";    
            try
            {
                ws.verificaEmettibilita(cc, ref ic);
            }
            catch (Exception ex)
            {
                if (ex.GetType() ==typeof(Com.Unisys.Logging.ManagedException))
                {
                    Risultato.Text = (ex as Com.Unisys.Logging.ManagedException).InnerExceptionMessage;
                }
                else
                {
                    Risultato.Text = ex.Message;
                }
            }
        }

        protected void btnRichiesta_OnClick(object sender, EventArgs e)
        {
            CertiService ws = new CertiService();
            ws.Url = txtWebService.Text;
            Risultato.Text = string.Empty;
            string key = ConfigurationManager.AppSettings["Chiave"];
            string username = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            string password = Encryption.Decrypt(ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["Chiave"]);
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            UsernameToken myToken = new UsernameToken(username, Com.Unisys.CdR.Certi.Utils.Config.PlainToSHA1(password), PasswordOption.SendPlainText);
            ws.RequestSoapContext.Security.Tokens.Add(myToken);
            CredenzialiType cc = new CredenzialiType();
            cc.transactionData = new TransactionRequestType();
            cc.transactionData.codiceFiscaleRichiedente = txtRichiedente.Text;
            cc.transactionData.codiceFiscaleIntestatario = txtCodiceFiscale.Text; 
            cc.transactionData.idTransazione = "00708002011012500030003";
            cc.transactionData.idPod = "";
            cc.transactionData.sistema = "PCOM";
            cc.idFlusso = txtIDFlusso.Text;           
            InfoCertificatoType[] ic = new InfoCertificatoType[1];
            ic[0] = new InfoCertificatoType();
            ic[0].idNomeCertificato = "C0001";
            ic[0].IdUso = "3";
            ic[0].idMotivoEsenzione = "0";
            CertificatoType[] resp = null;
            try
            {

                resp = ws.richiestaCertificati(cc, ic);
                for (int i = 0; i < resp.Length; i++)
                {
                   string a= resp[i].IdDocumento;
                }

            }
            catch (Exception ex)
            {
               Risultato.Text = ex.Message;
               return;
            }
            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");

            this.Response.BinaryWrite(resp[0].bin);
            this.Response.End();

        }

        protected void btnRecupera_Click(object sender, EventArgs e)
        {
            CertiService ws = new CertiService();
            ws.Url = txtWebService.Text;
            Risultato.Text = string.Empty;
            string key = ConfigurationManager.AppSettings["Chiave"];
            string username = Encryption.Decrypt(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["Chiave"]);
            string password = Encryption.Decrypt(ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["Chiave"]);
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            UsernameToken myToken = new UsernameToken(username, Com.Unisys.CdR.Certi.Utils.Config.PlainToSHA1(password), PasswordOption.SendPlainText);
            ws.RequestSoapContext.Security.Tokens.Add(myToken);
            CredenzialiType cc = new CredenzialiType();
            cc.transactionData = new TransactionRequestType();
            byte[][] resp = ws.recuperaDocumento(int.Parse(txtClientId.Text), txtCodiceFiscale.Text, txtCIU.Text);
            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=certificato.pdf");
            this.Response.BinaryWrite(resp[1]);
            this.Response.End();

        }
    }
}
