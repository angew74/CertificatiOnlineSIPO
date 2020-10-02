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

namespace Com.Unisys.CdR.Certi.WS
{
    public class Global : System.Web.HttpApplication
    {

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

        public bool RemoteCertificateValidationCallback(Object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            // DANGEROUS!  completely disable SSL validation if the test server has a bad Cert / bad Cert chain
            return true;
        }

    }
}