using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Oracle.DataAccess.Client;
using log4net;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.DataLayer.Contract;
using Com.Unisys.Data;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    /// <summary>
    /// Classe di accesso allo storage dati con la finalità di recuperare le informazioni sui 
    /// certificati presenti. Essa estende OracleDao e implementa l'interfaccia IDAOListaSemplice
    /// </summary>
    public class DAOListaSemplice : Com.Unisys.Data.Oracle10.OracleDao<OracleSessionManager, ISession>, IDAOListaSemplice
    {

        private static readonly ILog log = LogManager.GetLogger("DAOListaSemplice");

        #region IDAOListaSemplice Members

        /// <summary>
        /// Costruttore - restituisce il dao implementato
        /// </summary>
        /// <param name="DaoContext"><c>OracleSessionManager</c></param>
        public DAOListaSemplice(OracleSessionManager session) : base(session) { }


        /// <summary>
        /// Restituisce un datatable con la lista dei certificati presenti in base dati e le informazioni 
        /// che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista certificati attivi</returns>
        public ProfiloCertificato.CertificatoDataTable LoadCertificatiAttivi(int clientID)
        {
            ProfiloCertificato.CertificatoDataTable tb = new ProfiloCertificato.CertificatoDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT TIPI_CERTIFICATO.ID AS CERTID, TIPI_CERTIFICATO.BACKEND_ID,"
                + " TIPI_CERTIFICATO.PUBLIC_ID, CERTI_ATTIVA.TIPO_USO_ID, TIPI_CERTIFICATO.DESCRIZIONE AS NOME,"
                + " CERTI_ATTIVA.ATTIVO, TIPI_CERTIFICATO.ID_ALIAS AS ALIAS"
                + " FROM TIPI_CERTIFICATO JOIN CERTI_ATTIVA ON TIPI_CERTIFICATO.ID = CERTI_ATTIVA.TIPO_CERTIFICATO_ID"
                + " WHERE CERTI_ATTIVA.CLIENT_ID= " + clientID + " AND CERTI_ATTIVA.ATTIVO=1"
                + " ORDER BY NOME, TIPO_USO_ID";
            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_102",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadCertificatiAttivi",
                    "Caricamento certificati attivi Data Layer Dettagli : " + ex.Message + " - " + ex.InnerException,
                    "ClientID: " + clientID.ToString(),
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                log.Debug(ex.InnerException + " " + ex.Message + " " + ex.Source);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;

        }

        /// <summary>
        /// Restituisce un datatable con la lista dei certificati presenti in base dati e le informazioni 
        /// che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista certificati attivi</returns>
        public ProfiloCertificato.CertificatoDataTable LoadCertificatiAttivi()
        {
            ProfiloCertificato.CertificatoDataTable tb = new ProfiloCertificato.CertificatoDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT TIPI_CERTIFICATO.ID AS CERTID, TIPI_CERTIFICATO.BACKEND_ID,"
                + " TIPI_CERTIFICATO.PUBLIC_ID, CERTI_ATTIVA.TIPO_USO_ID, TIPI_CERTIFICATO.DESCRIZIONE AS NOME,"
                + " CERTI_ATTIVA.CLIENT_ID, CERTI_ATTIVA.ATTIVO"
                + " FROM TIPI_CERTIFICATO JOIN CERTI_ATTIVA ON TIPI_CERTIFICATO.ID = CERTI_ATTIVA.TIPO_CERTIFICATO_ID"
                + " WHERE CERTI_ATTIVA.ATTIVO=1"
                + " ORDER BY NOME, TIPO_USO_ID";
            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) Dettagli: " + ex.Message,
                    "ERR_092",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadCertificatiAttivi",
                    "Caricamento certificati attivi Data Layer",
                    string.Empty,
                    string.Empty,
                    null);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                log.Debug(ex.InnerException + " " + ex.Message + " " + ex.Source);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;

        }

        /// <summary>
        /// Restituisce un datatable con la lista completa dei certificati presenti in base dati e le informazioni 
        /// che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista certificati</returns>
        public ProfiloCertificato.CertificatoDataTable LoadCertificatiCompleta(int clientID)
        {
            ProfiloCertificato.CertificatoDataTable tb = new ProfiloCertificato.CertificatoDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT TIPI_CERTIFICATO.ID AS CERTID, TIPI_CERTIFICATO.BACKEND_ID,"
                + " TIPI_CERTIFICATO.PUBLIC_ID, CERTI_ATTIVA.TIPO_USO_ID, TIPI_CERTIFICATO.DESCRIZIONE AS NOME,"
                + " CERTI_ATTIVA.ATTIVO, TIPI_CERTIFICATO.ID_ALIAS AS ALIAS"
                + " FROM TIPI_CERTIFICATO JOIN CERTI_ATTIVA ON TIPI_CERTIFICATO.ID = CERTI_ATTIVA.TIPO_CERTIFICATO_ID"
                + " WHERE CERTI_ATTIVA.CLIENT_ID= " + clientID
                + " AND CERTI_ATTIVA.ATTIVO=1"
                + " ORDER BY NOME, TIPO_USO_ID";
            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_012",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadCertificatiCompleta",
                    "Caricamento certificati Data Layer",
                    "ClientID: " + clientID.ToString(),
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                log.Debug(ex.InnerException + " " + ex.Message + " " + ex.Source);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;

        }

        /// <summary>
        /// Restituisce un datatable con la lista completa dei certificati presenti in base dati e le informazioni 
        /// che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista certificati attivi</returns>
        public ProfiloCertificato.CertificatoDataTable LoadCertificatiCompleta()
        {
            ProfiloCertificato.CertificatoDataTable tb = new ProfiloCertificato.CertificatoDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT TIPI_CERTIFICATO.ID AS CERTID, TIPI_CERTIFICATO.BACKEND_ID,"
                + "TIPI_CERTIFICATO.PUBLIC_ID, CERTI_ATTIVA.TIPO_USO_ID, TIPI_CERTIFICATO.DESCRIZIONE AS NOME,"
                + " CERTI_ATTIVA.CLIENT_ID, CERTI_ATTIVA.ATTIVO"
                + " FROM TIPI_CERTIFICATO JOIN CERTI_ATTIVA ON TIPI_CERTIFICATO.ID = CERTI_ATTIVA.TIPO_CERTIFICATO_ID"
                + " WHERE CERTI_ATTIVA.ATTIVO=1"
                + " ORDER BY NOME, TIPO_USO_ID";
            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_022",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadCertificatiCompleta",
                    "Caricamento certificati Data Layer",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                log.Debug(ex.InnerException + " " + ex.Message + " " + ex.Source);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        public ProfiloDownload.CertificatiDataTable LoadCertificato4DownloadByCIU(string ciu)
        {
            ProfiloDownload.CertificatiDataTable tb = new ProfiloDownload.CertificatiDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT CERTIFICATI.ID, CERTIFICATI.RICHIESTA_ID,"
                                + "CERTIFICATI.TIPO_CERTIFICATO_ID, CERTIFICATI.TIPO_USO_ID, CERTIFICATI.CIU,"
                                + "CERTIFICATI.STATUS_ID, CERTIFICATI.XML_CERTIFICATO,"
                                + "CERTIFICATI.CODICE_PAGAMENTO, CERTIFICATI.XML_PAGAMENTO,"
                                + "CERTIFICATI.DATA_EMISSIONE, CERTIFICATI.SOGGETTO_RITIRO,"
                                + " RICHIESTE.CODICE_FISCALE_INTESTATARIO AS T_CODICE_FISCALE_INTESTATARIO, "
                                + " RICHIESTE.COGNOME_INTESTATARIO || ' ' || RICHIESTE.NOME_INTESTATARIO AS T_COGNOME_NOME_INTESTATARIO,"
                                + " TIPI_CERTIFICATO.DESCRIZIONE AS T_TIPO_CERTIFICATO,"
                                + " CERTIFICATI.DATA_EMISSIONE AS T_DATA_EMISSIONE"
                                + " FROM CERTIFICATI JOIN RICHIESTE ON "
                                + " CERTIFICATI.RICHIESTA_ID= RICHIESTE.ID"
                                + " JOIN TIPI_CERTIFICATO ON"
                                + " CERTIFICATI.TIPO_CERTIFICATO_ID = TIPI_CERTIFICATO.ID"
                                + " JOIN TAB_INDEX ON"
                                + " CERTIFICATI.CIU = TAB_INDEX.CIU"
                                + " WHERE CERTIFICATI.CIU='" + ciu + "'";
            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_042",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadCertificato4DownloadByCIU",
                    "Recupero certificato con ciu",
                    string.Empty,
                    "CIU: " + ciu,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cf"></param>
        /// <param name="tipoRitiro"></param>
        /// <returns></returns>
        public ProfiloDownload.CertificatiDataTable LoadCertificati4Download(string cf, int tipoRitiro, int clientID)
        {
            // NR MODIFICATA 08/02/2013
            string sqlString = String.Empty;
            string extSqlString = String.Empty;
            // DISTINCT PUNTATA
            string sqlString1 = "SELECT CERTIFICATI.ID, CERTIFICATI.RICHIESTA_ID,"
                                + "CERTIFICATI.TIPO_CERTIFICATO_ID, CERTIFICATI.TIPO_USO_ID, CERTIFICATI.CIU,"
                                + "CERTIFICATI.STATUS_ID,"
                               // + "NVL(CERTIFICATI.CODICE_PAGAMENTO,' ') AS CODICE_PAGAMENTO , "
                                + " CERTIFICATI.CODICE_PAGAMENTO, " 
                                + "CERTIFICATI.DATA_EMISSIONE, "
                                + "CERTIFICATI.XML_CERTIFICATO, "
                                + " RICHIESTE.CODICE_FISCALE_INTESTATARIO AS T_CODICE_FISCALE_INTESTATARIO, "
                                + " RICHIESTE.COGNOME_INTESTATARIO || ' ' || RICHIESTE.NOME_INTESTATARIO AS T_COGNOME_NOME_INTESTATARIO,"
                                + " TIPI_CERTIFICATO.DESCRIZIONE AS T_TIPO_CERTIFICATO,"
                                + " CERTIFICATI.DATA_EMISSIONE AS T_DATA_EMISSIONE";
            string sqlString2 = " FROM CERTIFICATI JOIN RICHIESTE ON "
                                + " CERTIFICATI.RICHIESTA_ID= RICHIESTE.ID"
                                + " JOIN TIPI_CERTIFICATO ON"
                                + " CERTIFICATI.TIPO_CERTIFICATO_ID = TIPI_CERTIFICATO.ID"
                                + " WHERE (RICHIESTE.CODICE_FISCALE_RICHIEDENTE='" + cf + "')";
            sqlString2 +=         " AND (CERTIFICATI.STATUS_ID = "
                                + (int)Status.C_GENERAZIONE_PDF_OK
                                + " OR (CERTIFICATI.STATUS_ID IN ("
                                + (int)Status.C_VERIFICA_EMETTIBILITA_OK
                                + ", " + (int)Status.C_EMISSIONE_OK
                                + ", " + (int)Status.C_RICHIESTA_PAGAMENTO
                                + ") AND RICHIESTE.CLIENT_ID = " + (int)clientID
                                + ")) and CERTIFICATI.CIU <> ' ' ";

            switch (tipoRitiro)
            {
                case 1:
                    sqlString = sqlString1
                        + sqlString2
                        + "order by T_DATA_EMISSIONE desc";
                    break;
                case 2:
                    extSqlString = ", ROW_NUMBER OVER (ORDER BY DATA_EMISSIONE DESC) AS RN";
                    sqlString = "SELECT * FROM ("
                                  + sqlString1
                                  + extSqlString
                                  + sqlString2
                                  + ") WHERE RN=1";
                    break;
                case 3:
                    extSqlString = " ROW_NUMBER OVER (PARTITION BY TIPO_CERTIFICATO_ID ORDER BY DATA_EMISSIONE DESC) AS RN";
                    sqlString = "SELECT * FROM ("
                                  + sqlString1
                                  + extSqlString
                                  + sqlString2
                                  + ") WHERE RN=1";
                    break;
                case 4:
                    extSqlString = " ROW_NUMBER OVER (PARTITION BY TIPO_CERTIFICATO_ID, T_CODICE_FISCALE_INTESTATARIO ORDER BY DATA_EMISSIONE DESC) AS RN";
                    sqlString = "SELECT * FROM ("
                                  + sqlString1
                                  + extSqlString
                                  + sqlString2
                                  + ") WHERE RN=1";
                    break;
            }

            ProfiloDownload.CertificatiDataTable tb = new ProfiloDownload.CertificatiDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = sqlString;
            try

            {   
              
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) detttagli: " + ex.Message,
                    "ERR_052",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadCertificati4Download",
                    "Recupero certificati da ritirare",
                    string.Empty,
                    "PassiveObjectCF: " + cf,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;

        }


        /// <summary>
        /// Restituisce un datatable con la lista delle motivazioni per l'esenzione dal bollo
        /// </summary>
        /// <returns><c>DataTable</c> lista certificati attivi</returns>
        public ProfiloMotivazione.MotivazioneDataTable LoadMotivazioni()
        {
            ProfiloMotivazione.MotivazioneDataTable tb = new ProfiloMotivazione.MotivazioneDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT * FROM MOTIVAZIONI";
            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_062",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadMotivazioni",
                    "Caricamento motivazioni attivi Data Layer",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProfiloTipoUso.TipoUsoDataTable LoadTipiUso()
        {
            ProfiloTipoUso.TipoUsoDataTable tb = new ProfiloTipoUso.TipoUsoDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT * FROM TIPI_USO";

            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " +ex.Message,
                    "ERR_072",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadTipiUso",
                    "Caricamento tipi d'uso attivi Data Layer",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProfiloClient.ClientsDataTable LoadClients()
        {
            ProfiloClient.ClientsDataTable tb = new ProfiloClient.ClientsDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT * FROM CLIENTS";

            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " +ex.Message,
                    "ERR_082",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadClients",
                    "Caricamento clients attivi Data Layer",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProfiloDiciturePagamenti.DiciturePagamentiDataTable LoadDiciturePagamenti()
        {
            ProfiloDiciturePagamenti.DiciturePagamentiDataTable tb = new ProfiloDiciturePagamenti.DiciturePagamentiDataTable();
            OracleDataAdapter ad = base.prepareSelectAdapter();
            ad.SelectCommand.CommandText = "SELECT * FROM TIPI_PAGAMENTO";

            try
            {
                ad.Fill(tb);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_092",
                    "Certi.Datalayer.OracleImpl.DAOListaSemplice",
                    "LoadDiciturePagamenti",
                    "Caricamento clients attivi Data Layer",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                ad.Dispose();
            }
            return tb;
        }
        #endregion
    }
}
