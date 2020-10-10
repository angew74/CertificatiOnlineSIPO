using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.WS.Business;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Com.Unisys.Logging.Errors;

namespace Com.Unisys.CdR.Certi.WS
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger("Global");
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            BUSListe lst = new BUSListe();
            CacheManager<ProfiloCertificato.CertificatoDataTable>.set(
                CacheKeys.CERTIFICATI_ATTIVI_WS, lst.getCertificatiAttiviList());
            CacheManager<ProfiloCertificato.CertificatoDataTable>.set(
                CacheKeys.CERTIFICATI_ATTIVI_WS, lst.getCertificatiCompletaList());
            CacheManager<ProfiloMotivazione.MotivazioneDataTable>.set(
                CacheKeys.ESENZIONI_ATTIVI_WS, lst.getMotivazioniList());
            CacheManager<ProfiloTipoUso.TipoUsoDataTable>.set(
                CacheKeys.TIPI_USO_WS, lst.getTipiUsoList());
            CacheManager<ProfiloClient.ClientsDataTable>.set(
                CacheKeys.CLIENTS_WS, lst.getClientsList());
            CacheManager<ProfiloDiciturePagamenti.DiciturePagamentiDataTable>.set(
                CacheKeys.DICITURE_PAGAMENTI_WS, lst.getDiciturePagamenti());
            System.Net.ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
        }

        protected void Application_End(object sender, EventArgs e)
        {

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


        public bool RemoteCertificateValidationCallback(Object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            // DANGEROUS!  completely disable SSL validation if the test server has a bad Cert / bad Cert chain
            return true;
        }

    }
}