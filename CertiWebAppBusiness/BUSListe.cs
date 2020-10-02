using System;
using System.Collections; 
using System.Collections.Generic;
using System.Text;
using System.Data; 
using log4net;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.Data;
using System.Configuration;

namespace Com.Unisys.CdR.Certi.WebApp.Business
{
    public class BUSListe
    {
        private static readonly ILog log = LogManager.GetLogger("BUSListe");
        /// <summary>
        /// Metodo per la richiesta in base dati dei dati di certificati attivi presenti in DB
        /// </summary>
        /// <remarks></remarks>
        /// <returns><c>DataTable</c>Oggetto contenente i certificati attivi</returns>
        public ProfiloCertificato.CertificatoDataTable getCertificatiAttiviList()
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadCertificatiAttivi(int.Parse(ConfigurationManager.AppSettings["ClientID"]));
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli: "+ ex.Message,
                            "ERR_124",
                            "Certi.WebApp.Business.BUSListe",
                            "getCertificatiAttiviList",
                            "Accesso in base dati per recuperare i certificati disponibili",
                            "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                            string.Empty,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// Metodo per la richiesta in base dati dei dati di tutti i certificati presenti in DB
        /// </summary>
        /// <remarks></remarks>
        /// <returns><c>DataTable</c>Oggetto contenente i certificati attivi</returns>
        public ProfiloCertificato.CertificatoDataTable getCertificatiCompleta()
        {
            try
            {
                
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadCertificatiCompleta(int.Parse(ConfigurationManager.AppSettings["ClientID"]));
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_125",
                            "Certi.WebApp.Business.BUSListe",
                            "getCertificatiAttiviList",
                            "Accesso in base dati per recuperare i certificati disponibili",
                            "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                            string.Empty,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// Metodo per la richiesta in base dati delle motivazioni presenti in DB
        /// </summary>
        /// <returns></returns>
        public ProfiloMotivazione.MotivazioneDataTable getMotivazioniList()
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadMotivazioni();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_127",
                            "Certi.WebApp.Business.BUSListe",
                            "getMotivazioniList",
                            "Accesso in base dati per recuperare le motivazioni per l'esenzione dal bollo",
                            "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                            string.Empty,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }


        public ProfiloClient.ClientsDataTable getClientsList()
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadClients(); 
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_128",
                            "Certi.WebApp.Business.BUSListe",
                            "getClientsList",
                            "Accesso in base dati per recuperare la lista dei clients",
                            "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                            string.Empty,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }



        public bool SaveMotivazioni(string idFlusso, ArrayList motivazioni)
        {
            return true;
        }

        /// <summary>
        /// recupero dalla base dati dei pdf già richiesti dal utente connesso
        /// </summary>
        /// <returns></returns>
        public ProfiloDownload.CertificatiDataTable GetElencoDownload(string codFis, int tipoRitiro, int clientID)
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadCertificati4Download(codFis, tipoRitiro, clientID);
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_130",
                            "Certi.WebApp.Business.BUSListe",
                            "GetElencoDownload",
                            "Accesso in base dati per recuperare i pdf pronti da scaricare",
                            "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                            "ActiveObjectCF: " + codFis,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }

        public ProfiloDownload.CertificatiDataTable GetCertificatoDownload(string ciu)
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadCertificato4DownloadByCIU(ciu);
            }
            catch (ManagedException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_131",
                            "Certi.WebApp.Business.BUSListe",
                            "GetCertificatoDownload",
                            "Accesso in base dati per recuperare il pdf pronto con ciu dato",
                            "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                            "CIU: " + ciu,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }
    }
}
