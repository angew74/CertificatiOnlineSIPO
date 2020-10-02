using System;
using Com.Unisys.CdR.Certi.WebApp.Business;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects.Common;
using log4net;
using Com.Unisys.Logging.Errors;
using System.Web.UI;
using System.Configuration;

namespace Com.Unisys.CdR.Certi.WebApp
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger("Global");
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();           
            BUSListe lst = new BUSListe(); 
            CacheManager<ProfiloCertificato.CertificatoDataTable>.set(
               CacheKeys.CERTIFICATI_ATTIVI, lst.getCertificatiAttiviList());
            CacheManager<ProfiloCertificato.CertificatoDataTable>.set(
                CacheKeys.CERTIFICATI_ATTIVI_AVVOCATI, lst.getCertificatiCompleta());
            CacheManager<ProfiloMotivazione.MotivazioneDataTable>.set(
                CacheKeys.MOTIVAZIONI_ATTIVI, lst.getMotivazioniList());
            CacheManager<ProfiloClient.ClientsDataTable>.set(
                CacheKeys.CLIENTS, lst.getClientsList());
            
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //se il sistema è in manutenzione l'utente viene ridiretto alla pagina informativa e solo gli accountabilitati allamanutenzione potranno operare
            bool manu = bool.Parse(ConfigurationManager.AppSettings["MANUTENZIONE"]);
            if (manu) 
            {
                string accounts = ConfigurationManager.AppSettings["MANUTENZIONE_ALLOWED_ACCOUNTS"];
                string[] names = accounts.Split(',');
                bool auth = false;
                if (Request.ServerVariables["HTTP_IV_USER"] != null)
                {
                    foreach (string cf in names)
                    {
                        if (cf.ToUpper().Equals(Request.ServerVariables["HTTP_IV_USER"].ToUpper()))     
                            auth = true;
                    }
                }
                if (!auth)
                    Response.Redirect(System.Web.VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["MANUTENZIONE_INFO_PAGE"]));
            }

            //se sono in test simulo gli headers del portale
            if (bool.Parse(ConfigurationManager.AppSettings["TEST"])) 
            { 
                   //Request.ServerVariables["HTTP_IV_USER"]=ConfigurationManager.AppSettings["TEST_ACCOUNT");
                   //Request.ServerVariables["HTTP_IV_REMOTE_ADDRESS"] = "10.10.10.10";
            }
            if (Request.ServerVariables["HTTP_IV_USER"] != null)
            {
                log.Debug("intercettato utente " + Request.ServerVariables["HTTP_IV_USER"]);
               // Response.Redirect("/servizi/certificati/emissione/Emissione.aspx");
            }
            else
            {
               
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            ErrorLogInfo error = new ErrorLogInfo();
            if (ex != null)
            {
                error.freeTextDetails = ex.Message;
                if (ex.InnerException != null)
                {
                    error.freeTextDetails += ex.InnerException.Message;
                }
            }
            error.logCode = "ERR999";
            error.loggingAppCode = "CWA";
            error.loggingTime = System.DateTime.Now;
            error.uniqueLogID = System.DateTime.Now.Ticks.ToString();          
            log.Error(error);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
           
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
           
        }
    }
}