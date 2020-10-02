using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using log4net;
using Com.Unisys.Logging;
using Com.Unisys.Data;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.WS.Business
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
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadCertificatiAttivi();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message                    + " di caricamento dei certificati disponibili.",
                    "ERR_135",
                    "Certi.WSOmnia.Business.BUSListe",
                    "getCertificatiAttiviList",
                    "accesso in base dati per recuperare i certificati disponibili",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// Metodo per la richiesta in base dati dei dati di tutti i certificati presenti in DB
        /// </summary>
        /// <remarks></remarks>
        /// <returns><c>DataTable</c>Oggetto contenente tutti i certificati</returns>
        public ProfiloCertificato.CertificatoDataTable getCertificatiCompletaList()
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadCertificatiCompleta();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message
                    + " di caricamento dei certificati disponibili.",
                    "ERR_136",
                    "Certi.WSOmnia.Business.BUSListe",
                    "getCertificatiAttiviList",
                    "accesso in base dati per recuperare i certificati disponibili",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
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
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) di"
                    + " caricamento delle motivazioni per l'esenzione dal bollo disponibili. dettagli: " + ex.Message,
                    "ERR_138",
                    "Certi.WSOmnia.Business.BUSListe",
                    "getMotivazioniList",
                    "accesso in base dati per recuperare le motivazioni per l'esenzione dal bollo",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProfiloTipoUso.TipoUsoDataTable getTipiUsoList()
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadTipiUso();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) di"
                    + " caricamento dei tipi di uso dei certificati. dettagli: " + ex.Message,
                    "ERR_140",
                    "Certi.WSOmnia.Business.BUSListe",
                    "getTipiUsoList",
                    "accesso in base dati per recuperare il tipo di uso dei certificati",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) di"
                    + " caricamento dei clients. dettagli: " + ex.Message,
                    "ERR_141",
                    "Certi.WSOmnia.Business.BUSListe",
                    "getClientsList",
                    "accesso in base dati per recuperare i clients disponibili",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProfiloDiciturePagamenti.DiciturePagamentiDataTable getDiciturePagamenti()
        {
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).ListaSemplice.LoadDiciturePagamenti();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) di"
                    + " caricamento delle diciture dei pagamenti. dettagli: " + ex.Message,
                    "ERR_143",
                    "Certi.WSOmnia.Business.BUSListe",
                    "getDiciturePagamenti",
                    "accesso in base dati per recuperare le diciture dei pagamenti disponibili",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
        }

    }
}
