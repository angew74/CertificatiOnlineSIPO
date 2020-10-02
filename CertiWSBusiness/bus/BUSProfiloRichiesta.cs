using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using log4net;
using Com.Unisys.Logging;
using Com.Unisys.Data;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.WS.Dati;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class ProfiloRichiestaStub
    {
        private static readonly ILog log = LogManager.GetLogger("ProfiloRichiestaStub");

        private ProfiloRichiesta pr;
        private ProfiloRichiesta.RichiesteRow rrow;
        private int id = -1;

        public ProfiloRichiesta Dataset
        {
            get { return pr; }
        }

        /// <summary>
        /// Costruttore di default
        /// </summary>
        public ProfiloRichiestaStub()
        {
            pr = new ProfiloRichiesta();
        }

        public ProfiloRichiestaStub(ProfiloRichiesta pr)
        {
            this.pr = pr;
            this.id = (int)((ProfiloRichiesta.RichiesteRow)pr.Richieste.Rows[0]).ID;
            rrow = pr.Richieste.FindByID(id);
        }


        ///<summary>
        /// ID della richiesta  
        /// deve essere impostato al fine di consentire l'update e il read dei dati di uno specifico record mediante
        /// le proprietà di questa classe
        ///</summary>
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                rrow = pr.Richieste.FindByID(id);
            }
        }

        ///<summary>
        ///ID del client
        ///</summary>
        public virtual int ClientID
        {
            get { return (int)rrow.CLIENT_ID; }
            set { rrow.CLIENT_ID = value; }
        }

        ///<summary>
        ///ID del client
        ///</summary>
        public virtual string TransazioneID
        {
            get { return (string)rrow.TRANSAZIONE_ID; }
            set { rrow.TRANSAZIONE_ID = value; }
        }

        ///<summary>
        ///ID dell'ufficio
        ///</summary>
        public virtual string UfficioID
        {
            get { return rrow.UFFICIO_ID; }
            set { rrow.UFFICIO_ID = value; }
        }

        ///<summary>
        ///ID status richiesta
        ///</summary>
        public virtual int StatusID
        {
            get { return (int)rrow.STATUS_ID; }
            set { rrow.STATUS_ID = value; }
        }

        ///<summary>
        ///Codice fiscale del richiedente
        ///</summary>
        public virtual string CodiceFiscaleRichiedente
        {
            get { return rrow.CODICE_FISCALE_RICHIEDENTE; }
            set { rrow.CODICE_FISCALE_RICHIEDENTE = value; }
        }

        ///<summary>
        ///Codice fiscale dell'intestatario
        ///</summary>
        public virtual string CodiceFiscaleIntestatario
        {
            get { return rrow.CODICE_FISCALE_INTESTATARIO; }
            set { rrow.CODICE_FISCALE_INTESTATARIO = value; }
        }

        ///<summary>
        ///Codice individuale dell'intestatario
        ///</summary>
        public virtual string CodiceIndividualeIntestatario
        {
            get { return rrow.COD_IND_INTESTATARIO; }
            set { rrow.COD_IND_INTESTATARIO = value; }
        }

        ///<summary>
        ///Cognome intestatario
        ///</summary>
        public virtual string CognomeIntestatario
        {
            get { return rrow.COGNOME_INTESTATARIO; }
            set { rrow.COGNOME_INTESTATARIO = value; }
        }

        ///<summary>
        ///Nome intestatario
        ///</summary>
        public virtual string NomeIntestatario
        {
            get { return rrow.NOME_INTESTATARIO; }
            set { rrow.NOME_INTESTATARIO = value; }
        }

        /// <summary>
        /// Proprietà pubblica che riespone i recordset in forma di lista di riga...da usare solo nel caso in cui i dati servono per essere visualizzati
        /// sotto forma di lista
        /// </summary>
        public virtual System.Data.DataRowCollection ListaRichieste
        {
            get { return pr.Richieste.Rows; }
        }

        /// <summary>
        /// Data table che espone nel formato richiesto dal layer di presentazione i certificati relativi ad 
        /// una richiesta
        /// </summary>
        /// <param name="id_richiesta"></param> 
        /// <returns><c>DataTable</c></returns>
        public DataTable getTabCertificati(int id_richiesta)
        {
            ProfiloRichiesta.CertificatiDataTable tab = default(ProfiloRichiesta.CertificatiDataTable);
            tab = (ProfiloRichiesta.CertificatiDataTable)pr.Certificati.Copy();
            return tab;
        }

        /// <summary>
        /// Metodo di aggiornamento certificati da verificare la loro emettibilità
        /// </summary>
        /// <param name="arr"></param>
        public void setTabCertificati(ProfiloRichiesta.CertificatiDataTable tb)
        {
            // cancello i certificati richiesti precedentemente
            int lines = pr.Certificati.Rows.Count;
            for (int i = lines - 1; i >= 0; i--)
            {
                ProfiloRichiesta.CertificatiRow r = pr.Certificati[i];
                //if (!String.IsNullOrEmpty(r.CIU))
                if(r.STATUS_ID >= (int)Status.C_EMISSIONE)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness)"
                        + " di generazione certificato con ID = " + r.ID,
                        "ERR_006",
                        "Certi.WS.Business.ProfiloRichiestaStub",
                        "setTabCertificati",
                        "Metodo di aggiornamento certificati da verificare la loro emettibilità",
                        string.Empty,
                        "CIU: " + r.CIU,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                else
                {
                    pr.Certificati.Rows[i].Delete();
                }
            }

            foreach (ProfiloRichiesta.CertificatiRow r in tb.Rows)
            {
                ProfiloRichiesta.CertificatiRow newrow = pr.Certificati.NewCertificatiRow();
                newrow.ID = r.ID;
                newrow.RICHIESTA_ID = this.id;
                newrow.TIPO_CERTIFICATO_ID = r.TIPO_CERTIFICATO_ID;
                newrow.TIPO_USO_ID = r.TIPO_USO_ID;
                newrow.ESENZIONE_ID = r.ESENZIONE_ID;
                newrow.STATUS_ID = r.STATUS_ID;
                pr.Certificati.Rows.Add(newrow);
            }
        }
    }

    public class BusProfiloRichiesta
    {

        private static readonly ILog log = LogManager.GetLogger("BUSProfiloRichiesta");


        /// <summary>
        /// Metodo che si interfaccia con il data layer per i dettagli della richiesta 
        /// </summary>
        /// <param name="idRichiesta"></param>
        /// <returns>Richiesta (oggetto ProfiloRichiesta)</returns>
        public ProfiloRichiesta GetRichiesta(int idRichiesta)
        {
            ProfiloRichiesta pr = null;
            try
            {
                pr = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.LoadRichiesta(idRichiesta);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel caricamento della richiesta. dettagli: " + ex.Message,
                    "ERR_150",
                    "Certi.WS.Business.BUSProfiloRichiesta",
                    "GetRichiesta",
                    "Caricamento richiesta",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return pr;
        }

        /// <summary>
        /// Metodo che si interfaccia con il data layer per i dettagli della richiesta con idtransazione dato
        /// </summary>
        /// <param name="idTransazione"></param>
        /// <returns></returns>
        public ProfiloRichiesta GetRichiestaByTransazione(string idTransazione)
        {
            ProfiloRichiesta pr = null;
            try
            {
                pr = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.LoadRichiestaByTransazione(idTransazione);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel caricamento della richiesta. dettagli: " + ex.Message,
                    "ERR_151",
                    "Certi.WS.Business.BUSProfiloRichiesta",
                    "GetRichiesta",
                    "Caricamento richiesta",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return pr;
        }

        /// <summary>
        /// Metodo che si interfaccia con il data layer per i dettagli della richiesta con ciu dato 
        /// </summary>
        /// <param name="CIU"></param>
        /// <returns>Richiesta (oggetto ProfiloRichiesta)</returns>
        public ProfiloRichiesta GetRichiestaByCIU(string CIU)
        {
            ProfiloRichiesta pr = null;
            try
            {
                pr = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.LoadRichiestaByCIU(CIU);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel caricamento della richiesta. dettagli: " +ex.Message,
                    "ERR_152",
                    "Certi.WS.Business.BUSProfiloRichiesta",
                    "GetRichiestaByCIU",
                    "Caricamento richiesta",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return pr;
        }

        /// <summary>
        /// Metodo che si interfaccia con il data layer per l'inserimento di una nuova richiesta 
        /// </summary>
        /// <param name="idClient"></param> 
        /// <param name="idPod"></param>
        /// <param name="status"></param> 
        /// <param name="codiceFiscaleRichiedente"></param> 
        /// <param name="codiceFiscaleIntestatario"></param> 
        /// <param name="codiceIndividualeIntestatario"></param> 
        /// <returns>ID richiesta</returns> 
        public int AddNuovaRichiesta(int idClient, string idPod, int status, string codiceFiscaleRichiedente, string codiceFiscaleIntestatario, string codiceIndividualeIntestatario, string idTransazione)
        {
            int id_richiesta = -1;
            ProfiloRichiesta pr = new ProfiloRichiesta();
            ProfiloRichiesta.RichiesteRow r = pr.Richieste.NewRichiesteRow();
            r.ID = id_richiesta;
            r.CLIENT_ID = idClient;
            r.UFFICIO_ID = idPod;
            r.STATUS_ID = status;
            r.CODICE_FISCALE_RICHIEDENTE = codiceFiscaleRichiedente;
            r.CODICE_FISCALE_INTESTATARIO = codiceFiscaleIntestatario;
            r.COD_IND_INTESTATARIO = codiceIndividualeIntestatario;
            r.TRANSAZIONE_ID = idTransazione;
            pr.Richieste.AddRichiesteRow(r);
            try
            {
                id_richiesta = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.InsertNuovaRichiesta(pr);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nell'inserimento di una nuova richiesta. dettagli: " + ex.Message,
                    "ERR_159",
                    "Certi.WS.Business.BUSProfiloRichiesta",
                    "AddNuovaRichiesta",
                    "Caricamento richiesta",
                    "ClientID: " + idClient,
                    "PassiveObjectCF: " + codiceFiscaleIntestatario,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return id_richiesta;
        }

        /// <summary>
        /// Metodo che si interfaccia con il data layer per l'aggiornamento della richiesta 
        /// </summary>
        /// <param name="prs">Dati richiesta da aggiornare</param>
        public void RenewRichiesta(ProfiloRichiestaStub prs)
        {
            try
            {
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdateRichiesta(prs.Dataset);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business di aggiornamento di una richiesta dettagli: " + ex.Message,
                    "ERR_160",
                    "Certi.WS.Business.BusProfiloRichiesta",
                    "RenewRichiesta",
                    "Aggiornamento richiesta",
                    "ClientID: " + prs.ClientID,
                    "PassiveObjectCF: " + prs.CodiceFiscaleIntestatario,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
        }

        public void RenewCertificati(ProfiloRichiesta.CertificatiRow[] rows)
        {
            try
            {
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdateCertificati(rows);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore aggiornamento certificati dettagli: " + ex.Message,
                    "ERR_161",
                    "Certi.WS.Business.BusProfiloRichiesta",
                    "RenewCertificati",
                    "Aggiornamento certificati",
                    string.Empty,
                    "CIU: " + rows[0].CIU,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSW", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prs"></param>
        public bool RenewStatusRichiesta(ProfiloRichiesta.RichiesteRow[] rows)
        {
            bool resp = false;
            try
            {
                resp = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdateStatusRichiesta(rows);
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore aggiornamento richiesta dettagli: " + ex.Message,
                    "ERR_163",
                    "Certi.WS.Business.BUSManager",
                    "RenewStatusRichiesta",
                    "Aggiorna lo stato della richiesta",
                    "ClientID: " + rows[0].CLIENT_ID,
                    "PassiveObjectCF: " + rows[0].CODICE_FISCALE_INTESTATARIO,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }
    }
}
