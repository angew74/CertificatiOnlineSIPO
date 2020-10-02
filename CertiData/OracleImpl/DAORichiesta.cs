using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using log4net;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.DataLayer.Contract;
using Com.Unisys.Data;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Collections;
using System.Configuration;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    /// <summary>
    /// Classe di accesso allo storage dati con la finalità di recuperare le informazioni su 
    /// un flusso richiesta certificati. Estende OracleDao e implementa l'interfaccia IDAORichiesta
    /// </summary>
    public class DAORichiesta : Com.Unisys.Data.Oracle10.OracleDao<OracleSessionManager, ISession>, IDAORichiesta
    {
        private static readonly ILog log = LogManager.GetLogger("DAORichiesta");

        #region IDAORichiesta Members
        /// <summary>
        /// Costruttore - restituisce il dao implementato
        /// </summary>
        /// <param name="DaoContext"><c>OracleSessionManager</c></param>
        public DAORichiesta(OracleSessionManager session) : base(session) { }

        #endregion


        /// <summary>
        /// Caricamento dei dati base di una richiesta (senza certificati)
        /// </summary>
        /// <param name="idFlusso"></param> 
        public ProfiloRichiesta LoadBaseRichiesta(int idFlusso)
        {
            ProfiloRichiesta pr = new ProfiloRichiesta();
            OracleDataAdapter oda = base.prepareSelectAdapter();
            oda.SelectCommand.CommandText = "select id, client_id, ufficio_id, status_id,"
                                          + "codice_fiscale_richiedente, codice_fiscale_intestatario,"
                                          + "cod_ind_intestatario, null, null from richieste where id=" + idFlusso;

            try
            {
                oda.Fill(pr.Richieste);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_112",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "LoadBaseRichiesta",
                    "Caricamento richiesta senza certificati Data Layer",
                    string.Empty,
                    "IDFlusso: " + idFlusso,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                oda.Dispose();
            }
            return pr;
        }

        /// <summary>
        ///  Caricamento dei dati di una richiesta (con eventuali certificati)
        /// </summary>
        /// <param name="idFlusso"></param>
        /// <returns></returns>
        public ProfiloRichiesta LoadRichiesta(int idFlusso)
        {
            ProfiloRichiesta pr = new ProfiloRichiesta();
            OracleDataAdapter oda = base.prepareSelectAdapter();

            try
            {
                oda.SelectCommand.CommandText = "select * from richieste where id=" + idFlusso; ;
                oda.Fill(pr.Richieste);
                oda.SelectCommand.CommandText = "SELECT CERTIFICATI.ID, CERTIFICATI.RICHIESTA_ID, CERTIFICATI.TIPO_CERTIFICATO_ID,"
                                                    + " CERTIFICATI.TIPO_USO_ID,"
                                                    + " CERTIFICATI.CIU, CERTIFICATI.STATUS_ID,"
                                                    + " CERTIFICATI.XML_CERTIFICATO, CERTIFICATI.CODICE_PAGAMENTO,"
                                                    + " CERTIFICATI.XML_PAGAMENTO, CERTIFICATI.DATA_EMISSIONE,"
                                                    + " CERTIFICATI.SOGGETTO_RITIRO, CERTIFICATI.ESENZIONE_ID,"
                                                    + " TIPI_CERTIFICATO.DESCRIZIONE AS T_TIPO_CERTIFICATO,"
                                                    + " TIPI_USO.DESCRIZIONE AS T_TIPO_USO,"
                                                    + " TIPI_CERTIFICATO.SERVIZIO_ID AS T_SERVIZIO_ID,"
                                                    + " SERVIZI.DESCRIZIONE AS T_SERVIZIO,"
                                                    + " TIPI_CERTIFICATO.BACKEND_ID AS T_BACKEND_ID,"
                                                    + " TIPI_CERTIFICATO.PUBLIC_ID AS T_PUBLIC_ID"
                                                //     + ", empty_blob() AS T_DOCUMENTO"                                                   
                                                    + " from CERTIFICATI JOIN TIPI_CERTIFICATO ON"
                                                    + " CERTIFICATI.TIPO_CERTIFICATO_ID = TIPI_CERTIFICATO.ID"
                                                    + " LEFT OUTER JOIN TIPI_USO ON"
                                                    + " CERTIFICATI.TIPO_USO_ID=TIPI_USO.ID"
                                                    + " JOIN SERVIZI ON"
                                                    + " TIPI_CERTIFICATO.SERVIZIO_ID=SERVIZI.ID"
                                                    + " where CERTIFICATI.RICHIESTA_ID=" + idFlusso;
                log.Debug("prima della select_" +idFlusso);
                log.Debug("Ecco la select: " + oda.SelectCommand.CommandText);
                oda.Fill(pr.Certificati);
                log.Debug("dopo select_" + idFlusso);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_122",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "LoadRichiesta",
                    "Caricamento richiesta Data Layer",
                    string.Empty,
                    "IDFlusso: " + idFlusso,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                oda.Dispose();
            }
            return pr;
        }

        /// <summary>
        ///  Caricamento dei dati di una richiesta (con eventuali certificati)
        /// </summary>
        /// <param name="idFlusso"></param>
        /// <returns></returns>
        public ProfiloRichiesta LoadRichiestaByTransazione(string idTransazione)
        {
            ProfiloRichiesta pr = new ProfiloRichiesta();
            OracleDataAdapter oda = base.prepareSelectAdapter();

            try
            {
                oda.SelectCommand.CommandText = "select * from richieste where transazione_id = '" + idTransazione + "'";
                oda.Fill(pr.Richieste);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_122",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "LoadRichiesta",
                    "Caricamento richiesta Data Layer",
                    string.Empty,
                    "IDTransazione: " + idTransazione,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                oda.Dispose();
            }
            return pr;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CIU"></param>
        /// <returns></returns>
        public ProfiloRichiesta LoadRichiestaByCIU(string CIU)
        {
            ProfiloRichiesta pr = new ProfiloRichiesta();
            OracleDataAdapter oda = base.prepareSelectAdapter();

            try
            {
                oda.SelectCommand.CommandText = "select * from richieste where id="
                                                    + "(select richiesta_id from certificati where ciu='" + CIU + "')";
                oda.Fill(pr.Richieste);
                oda.SelectCommand.CommandText = "SELECT CERTIFICATI.ID, CERTIFICATI.RICHIESTA_ID, CERTIFICATI.TIPO_CERTIFICATO_ID,"
                                                    + " CERTIFICATI.TIPO_USO_ID,"
                                                    + " CERTIFICATI.CIU, CERTIFICATI.STATUS_ID,"
                                                    + " CERTIFICATI.XML_CERTIFICATO, CERTIFICATI.CODICE_PAGAMENTO,"
                                                    + " CERTIFICATI.XML_PAGAMENTO, CERTIFICATI.DATA_EMISSIONE,"
                                                    + " CERTIFICATI.SOGGETTO_RITIRO, CERTIFICATI.ESENZIONE_ID,"
                                                    + " TIPI_CERTIFICATO.DESCRIZIONE AS T_TIPO_CERTIFICATO,"
                                                    + " TIPI_USO.DESCRIZIONE AS T_TIPO_USO,"
                                                    + " TIPI_CERTIFICATO.SERVIZIO_ID AS T_SERVIZIO_ID,"
                                                    + " SERVIZI.DESCRIZIONE AS T_SERVIZIO,"
                                                    + " TIPI_CERTIFICATO.BACKEND_ID AS T_BACKEND_ID,"
                                                    + " TIPI_CERTIFICATO.PUBLIC_ID AS T_PUBLIC_ID "
                                                   //  + " ,empty_blob() AS T_DOCUMENTO"
                                                    + " from CERTIFICATI JOIN TIPI_CERTIFICATO ON"
                                                    + " CERTIFICATI.TIPO_CERTIFICATO_ID = TIPI_CERTIFICATO.ID"
                                                    + " JOIN TIPI_USO ON"
                                                    + " CERTIFICATI.TIPO_USO_ID=TIPI_USO.ID"
                                                    + " JOIN SERVIZI ON"
                                                    + " TIPI_CERTIFICATO.SERVIZIO_ID=SERVIZI.ID"
                                                    + " where CERTIFICATI.CIU='" + CIU + "'";
                oda.Fill(pr.Certificati);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_111",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "LoadRichiestaByCIU",
                    "Caricamento richiesta Data Layer",
                    string.Empty,
                    "CIU: " + CIU,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                oda.Dispose();
            }
            return pr;
        }

        /// <summary>
        /// Inserimento di una nuova richiesta (inizializzazione flusso)
        /// </summary>
        /// <param name="pr">dataset con la nuova richiesta</param>
        public int InsertNuovaRichiesta(ProfiloRichiesta pr)
        {
            int id_richiesta = 0;
            ComponentRichieste c = new ComponentRichieste();
            c.setConnection(this.CurrentConnection);
            try
            {
                c.richiesteOracleDataAdapter1.Update(pr.Richieste);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_113",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "InsertNuovaRichiesta",
                    "Inserimento nuova richiesta Data Layer",
                    "ClientID: " + pr.Richieste[0].CLIENT_ID,
                    "PassiveObjectCF: " + pr.Richieste[0].CODICE_FISCALE_INTESTATARIO,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                c.Dispose();
            }

            id_richiesta = int.Parse(c.richiesteOracleDataAdapter1.InsertCommand.Parameters[":V_RICHIESTA_ID"].Value.ToString());
            if (id_richiesta < 0)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) ",
                    "ERR_114",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "InsertNuovaRichiesta",
                    "Inserimento nuova richiesta Data Layer",
                    "ClientID: " + pr.Richieste[0].CLIENT_ID,
                    "passiveObjectCF: " + pr.Richieste[0].CODICE_FISCALE_INTESTATARIO,
                    null);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            return id_richiesta;
        }

        /// <summary>
        /// Inserimento certificati di una richiesta inseriTa precedentemente
        /// </summary>
        /// <param name="richiesta"></param>
        /// <returns></returns>
        public void InsertCertificati(ProfiloRichiesta pr)
        {
            ComponentCertificati c = new ComponentCertificati();
            c.setConnection(this.CurrentConnection);

            try
            {
                c.CertificatiDataAdapter.Update(pr.Certificati);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_114",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "InsertCertificati",
                    "Inserimento dei certificati di una richiesta Data Layer",
                    "ClientID: " + pr.Richieste[0].CLIENT_ID,
                    "PassiveObjectCF: " + pr.Richieste[0].CODICE_FISCALE_INTESTATARIO,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                c.Dispose();
            }
        }

        public void UpdateCertificati(ProfiloRichiesta.CertificatiRow[] rows)
        {
            ComponentCertificati c = new ComponentCertificati();
            c.setConnection(this.CurrentConnection);
            try
            {
                c.CertificatiDataAdapter.Update(rows);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_115",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "UpdateCertificati",
                    "Aggiornamento dei certificati di una richiesta Data Layer",
                    "ClientID: " + rows[0].T_PUBLIC_ID,
                    "CIU: " + rows[0].CIU,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                c.Dispose();
            }
        }

        /// <summary>
        /// Aggiornamento di una richiesta già inserita in baase dati
        /// </summary>
        /// <param name="pr"></param> 
        public void UpdateRichiesta(ProfiloRichiesta pr)
        {
            if (pr.Certificati.GetChanges() != null)
            {
                ComponentCertificati c = new ComponentCertificati();
                c.setConnection(this.CurrentConnection);
                try
                {
                    c.CertificatiDataAdapter.Update(pr.Certificati);
                }
                catch (Exception ex)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                        "ERR_116",
                        "Certi.Datalayer.OracleImpl.DAORichiesta",
                        "UpdateRichiesta",
                        "Aggiornamento di una richiesta Data Layer",
                        "ClientID: " + pr.Richieste[0].CLIENT_ID,
                        "CIU: " + pr.Certificati[0].CIU,
                        ex.InnerException);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                    log.Error(error);
                    throw mex;
                }
                finally
                {
                    c.Dispose();
                }
            }

            ComponentRichieste r = new ComponentRichieste();
            r.setConnection(this.CurrentConnection);
            try
            {
                r.richiesteOracleDataAdapter1.Update(pr.Richieste);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                    "ERR_117",
                    "Certi.Datalayer.OracleImpl.DAORichiesta",
                    "UpdateRichiesta",
                    "Aggiornamento di una richiesta Data Layer",
                    "ClientID: " + pr.Richieste[0].CLIENT_ID,
                    "PassiveObjectCF: " + pr.Richieste[0].CODICE_FISCALE_INTESTATARIO,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                r.Dispose();
            }
        }


        public void UpdatePagamentoExt(int statusId, string xmlPagamento, decimal id, string ciu)
        {           
            string ConString = ConfigurationManager.AppSettings["IndexConnection"];
            log.Debug("ho letto la string di connessione");
            var queryString = string.Format(@"UPDATE CERTIFICATI SET STATUS_ID=:current_STATUS_ID, XML_PAGAMENTO=:current_XML_PAGAMENTO WHERE ID=:original_ID");
            OracleConnection con = null;
            try
            {
                using (con = new OracleConnection(ConString))
                {
                    log.Debug("sto per aprire la connessione");
                    con.Open();
                    OracleCommand cmd = con.CreateCommand();
                    log.Debug("ho creato il command");
                    cmd.CommandText = queryString;
                    cmd.Parameters.Add("current_STATUS_ID",statusId);               
                    cmd.Parameters.Add("current_XML_PAGAMENTO", xmlPagamento);
                    cmd.Parameters.Add("original_ID", id);
                    log.Debug("sto per aggiornare");
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated != 1)
                    {
                        con.Close();
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: righe non aggiornate ",
                       "ERR_118",
                       "Certi.Datalayer.OracleImpl.DAORichiesta",
                       "UpdatePagamento",
                       "Aggiornamento dati pagamento Data Layer",
                       string.Empty,
                       "CIU: " + ciu,
                       null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                        log.Error(error);
                        throw mex;
                    }
                }
                
            }
            catch (Exception ex)
            {
                con.Close();
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                      "ERR_118",
                      "Certi.Datalayer.OracleImpl.DAORichiesta",
                      "UpdatePagamento",
                      "Aggiornamento dati pagamento Data Layer",
                      string.Empty,
                      "CIU: " + ciu ,
                      ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            con.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb"></param>
        public void UpdatePagamento(ProfiloDownload.CertificatiDataTable tb)
        {
            log.Debug("sto per istanziare certificati component");
            ComponentCertificati c = new ComponentCertificati();
            c.setConnection(this.CurrentConnection);
            log.Debug("sto dentro update pagamento");
            if (tb.GetChanges() != null)
            {
                try
                {                   
                    OracleCommand upPaga = new OracleCommand();
                    upPaga.Connection = this.CurrentConnection;
                    upPaga.CommandType = CommandType.Text;
                    upPaga.CommandText = "UPDATE CERTIFICATI SET STATUS_ID=:current_STATUS_ID_param5, CODICE_PAGAMENTO=:current_CODICE_PAGAMENT_param7, XML_PAGAMENTO=:current_XML_PAGAMENTO_param8 WHERE ID=:original_ID_param11";
                    OracleParameter oc1 = new OracleParameter();
                    OracleParameter oc2 = new OracleParameter();
                    OracleParameter oc3 = new OracleParameter();
                    OracleParameter oc4 = new OracleParameter();

                    oc1.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
                    oc1.ParameterName = ":current_STATUS_ID_param5";
                    oc1.SourceColumn = "STATUS_ID";

                    oc2.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
                    oc2.ParameterName = ":current_CODICE_PAGAMENT_param7";
                    oc2.SourceColumn = "CODICE_PAGAMENTO";

                    oc3.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
                    oc3.ParameterName = ":current_XML_PAGAMENTO_param8";
                    oc3.SourceColumn = "XML_PAGAMENTO";

                    oc4.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
                    oc4.ParameterName = ":original_ID_param11";
                    oc4.SourceColumn = "ID";

                    upPaga.Parameters.Add(oc1);
                    upPaga.Parameters.Add(oc2);
                    upPaga.Parameters.Add(oc3);
                    upPaga.Parameters.Add(oc4);

                    c.CertificatiDataAdapter.UpdateCommand = upPaga;
                    c.CertificatiDataAdapter.Update(tb);
                    log.Debug("ho aggiornato il pagamento");

                }
                catch (Exception ex)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                        "ERR_118",
                        "Certi.Datalayer.OracleImpl.DAORichiesta",
                        "UpdatePagamento",
                        "Aggiornamento dati pagamento Data Layer",
                        string.Empty,
                        "CIU: " + tb[0].CIU,
                        ex.InnerException);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                    log.Error(error);
                    throw mex;
                }
                finally
                {
                    c.Dispose();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciu"></param>
        /// <param name="descrizione"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateCertificatoByCIU(string ciu, string descrizione, int status, string juv)
        {
            int ret = -1;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = this.CurrentConnection;
            cmd.CommandType = CommandType.Text;
            string sqlString = "";
            if (!string.IsNullOrEmpty(juv))
            { sqlString = "Update certificati set STATUS_ID = " + status + ", CODICE_PAGAMENTO='" + juv + "' where CIU ='" + ciu + "'"; }
            if (!String.IsNullOrEmpty(descrizione))
            {
                sqlString = "Update certificati set SOGGETTO_RITIRO = '" + descrizione + "', STATUS_ID = " +
                   status + " where CIU ='" + ciu + "'";
            }
            if (string.IsNullOrEmpty(descrizione) && string.IsNullOrEmpty(ciu))
            {
                sqlString = "Update certificati set STATUS_ID = " + status + " where CIU ='" + ciu + "'";
            }
            if (string.IsNullOrEmpty(descrizione) && !(string.IsNullOrEmpty(ciu)) && string.IsNullOrEmpty(juv))
            {
                sqlString = "Update certificati set STATUS_ID = " + status + " where CIU ='" + ciu + "'";
            }
            cmd.CommandText = sqlString;

            try
            {
                if (!(cmd.Connection.State == ConnectionState.Open)) cmd.Connection.Open();
                ret = cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                        "ERR_119",
                        "Certi.Datalayer.OracleImpl.DAORichiesta",
                        "UpdatePagamento",
                        "Aggiornamento dati pagamento Data Layer",
                        string.Empty,
                        "CIU: " + ciu,
                        ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                cmd.Dispose();
                cmd = null;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        public int GetStatusCertificato(string ciu)
        {
            int ret;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = this.CurrentConnection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select STATUS_ID from certificati where ciu = '" + ciu + "'";
            try
            {
                if (!(cmd.Connection.State == ConnectionState.Open)) cmd.Connection.Open();
                Object obj = cmd.ExecuteScalar();
                ret = int.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: "  + ex.Message,
                        "ERR_120",
                        "Certi.Datalayer.OracleImpl.DAORichiesta",
                        "GetStatusCertificato",
                        "Interrogazione status certificato Data Layer",
                        string.Empty,
                        "CIU: " + ciu,
                        ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                cmd.Dispose();
                cmd = null;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public bool UpdateStatusRichiesta(ProfiloRichiesta.RichiesteRow[] rows)
        {
            bool resp = false;
            ComponentRichieste r = new ComponentRichieste();
            r.setConnection(this.CurrentConnection);

            try
            {
                OracleCommand upStatus = new OracleCommand();
                upStatus.Connection = this.CurrentConnection;
                upStatus.CommandType = CommandType.Text;
                upStatus.CommandText = "UPDATE RICHIESTE SET STATUS_ID=:current_STATUS_ID_param3 WHERE ID=:original_ID_param6";
                OracleParameter oc1 = new OracleParameter();
                OracleParameter oc2 = new OracleParameter();


                oc1.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
                oc1.ParameterName = ":current_STATUS_ID_param3";
                oc1.SourceColumn = "STATUS_ID";

                oc2.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
                oc2.ParameterName = ":original_ID_param6";
                oc2.SourceColumn = "ID";

                upStatus.Parameters.Add(oc1);
                upStatus.Parameters.Add(oc2);

                r.richiesteOracleDataAdapter1.UpdateCommand = upStatus;
                r.richiesteOracleDataAdapter1.Update(rows);
                resp = true;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness) dettagli: " + ex.Message,
                        "ERR_123",
                        "Certi.Datalayer.OracleImpl.DAORichiesta",
                        "UpdateStatusRichiesta",
                        "Aggiornamento status richiesta Data Layer",
                        "ClientID: " + rows[0].CLIENT_ID,
                        "PassiveObjectCF: " + rows[0].CODICE_FISCALE_INTESTATARIO,
                        ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                log.Error(error);
                throw mex;
            }
            finally
            {
                r.Dispose();
            }
            return resp;
        }
        public byte[] ReadHandle(string id)
        {
            byte[] ret = null;
            string Sql="SELECT HANDLE FROM tab_index WHERE CIU='"+ id.Replace("'","''")+ "'";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = this.CurrentConnection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Sql;
            try
            {
                if (!(cmd.Connection.State == ConnectionState.Open)) cmd.Connection.Open();
                Object obj = cmd.ExecuteScalar();
                ret = (byte[])cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                ret = null;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                cmd.Dispose();
                cmd = null;
            }
            return ret;
        
        }

    }
}
