using System;
using System.Collections;
using System.Xml;
using log4net;
using Com.Unisys.Data;
using Com.Unisys.Logging;
using Com.Unisys.CdR.DataObjects.CertificatiAnagrafici;
using Com.Unisys.CdR.DataObjects.Common.RicercheAnagrafiche;
using Com.Unisys.CdR.Certi.WS.Dati;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.Objects.SIPO;
using System.Configuration;
using Com.Unisys.CdR.Certi.WS.Business.sipo;
using Newtonsoft.Json;
using Com.Unisys.CdR.Certi.WS.Business.bus;
using System.Collections.Generic;
using System.Linq;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BUSManager));

        #region Ricerche client web



        /// <summary>
        /// Verifica la presenza di un codice fiscale nell'archivio dell'Anagrafe CdR
        /// </summary>
        /// <param name="idClient"> Id client chiamante<see cref=System.String/><remarks>Serve solo per effettuare i log</remarks> </param>
        /// <param name="codiceFiscale"><see cref=System.String/></param>
        /// <returns>Codice individuale dela persona richiesta<see cref=System.String/></returns>
        public string VerificaCodiceFiscale(int idClient, string codiceFiscale)
        {
            string cod = String.Empty;
            ResponseRichiestaToken r = new ResponseRichiestaToken();
            try
            {
             
                string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];
                string AuthRequest = "BASIC " + SerializationHelper.ToBase64Encode(utenzadominio);
                string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
                SIPORequestJson sipoRequestToken = new SIPORequestJson(RichiestaToken, "RichiestaToken", AuthRequest, "", codiceFiscale);
                r = sipoRequestToken.CallingRichiestaToken(codiceFiscale);
                string accesstoken = r.access_token;
                RicercaPosAnag ricercaPosAnag = new RicercaPosAnag();
                ricercaPosAnag.codiceFiscale = codiceFiscale;
                ricercaPosAnag.hostname = ConfigurationManager.AppSettings["hostname"];
                ricercaPosAnag.cfUser = ConfigurationManager.AppSettings["cfuser"];
                string ricerca = JsonConvert.SerializeObject(ricercaPosAnag);
                string AccessTokenRicerca = "bearer " + accesstoken;              
                SIPORequestJson sipoRequest = new SIPORequestJson(ConfigurationManager.AppSettings["ServiceRicercaPosAnag"], "RicercaPosAnag", AccessTokenRicerca, ricerca, codiceFiscale);
                List<MyArray> myArrays = sipoRequest.CallingRicercaPosizione(codiceFiscale);
                if (myArrays.Count == 0)
                {
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog();
                    error.loggingAppCode = "CWS";
                    error.logCode = "ERR_100";
                    error.freeTextDetails = "La ricerca non ha dato risultati. ClientID: " + idClient.ToString() +
                                            " PassiveObjectCF: " + codiceFiscale + " Errore: " 
                                           ;
                    log.Error(error);
                }
                else
                {
                    cod = myArrays[0].idSoggetto.ToString();
                }

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore chiamata BIS dettagli: " + ex.Message,
                    "ERR_144",
                    "Certi.WS.Business.BUSManager",
                    "VerificaCodiceFiscale",
                    "Ricerca persona in backend",
                    "ClientID: " + idClient.ToString(),
                    "PassiveObjectCF: " + codiceFiscale,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;

            }
            return cod;
        }

     
        /// <summary>
        /// Metodo business di ricerca dei componenti della famiglia della persona con codice fiscale dato
        /// </summary>
        /// <param name="codiceFiscale"></param>
        /// <returns>codice individuale</returns>

        /// <summary>
        /// Metodo business di ricerca dei componenti della famiglia della persona con codice fiscale dato
        /// </summary>
        /// <param name="idClient"> Id client chiamante<see cref=System.String/><remarks>Serve solo per effettuare i log</remarks> </param>
        /// <param name="codiceFiscale"><see cref=System.String/></param>
        /// <returns>Elenco componenti della famiglia<see cref=Com.Unisys.CdR.Certi.WS.Dati.ComponenteFamigliaType[]/></returns>
        public ComponenteFamigliaType[] InviaRicercaComponentiFamiglia(int idClient, string codiceFiscale)
        {
            ComponenteFamigliaType[] componentiFamiglia = null;
            BUSVeriEmettibilita br = new BUSVeriEmettibilita();
            try
            {

                 componentiFamiglia =  br.FindComponenti(codiceFiscale);
                if (componentiFamiglia != null)
                {                    
                    Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                    info.logCode = "RCO";
                    info.loggingAppCode = "CWS";
                    info.flussoID = String.Empty;
                    info.clientID = idClient.ToString();
                    info.activeObjectCF = codiceFiscale;
                    info.activeObjectIP = String.Empty;
                    info.passiveObjectCF = codiceFiscale;
                    log.Info(info);
                }
                else
                {
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog();
                    error.loggingAppCode = "CWS";
                    error.logCode = "ERR";
                    error.freeTextDetails = "La ricerca non ha dato risultati. ClientID: " + idClient.ToString() +
                                            " ActiveObjectCF: " + codiceFiscale +
                                            " PassiveObjectCF: " + codiceFiscale + " Errore: ";
                                           
                    log.Error(error);
                }
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore chiamata BIS dettagli: " + ex.Message,
                    "ERR_146",
                    "Certi.WS.Business.BUSManager",
                    "InviaRicercaComponentiFamiglia",
                    "Ricerca componenti della famiglia",
                    "ClientID: " + idClient.ToString(),
                    "PassiveObjectCF: " + codiceFiscale,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return componentiFamiglia;
        }

        // N.R. MODIFICATO IL SERVIZIO DA TESTARE 09/2020
        /// <summary>
        /// Metodo business per il recupero dallo storage di un documento pdf con identificativo dato (ciu) 
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        public byte[][] FindPdf(int idClient, string codiceFiscaleRichiedente, string ciu)
        {
            byte[][] documento = null;
            try
            {
                BUSStorage bs = new BUSStorage();
                int st = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.GetStatusCertificato(ciu);
                documento = bs.LoadCertificato(ciu);
                if (documento[0][0] == 0 && documento[1].Length > 0)
                {
                    DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
                    if (st == (int)Status.C_EMISSIONE_OK)
                    {
                        DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdateCertificatoByCIU(ciu, "", (int)Status.C_GENERAZIONE_PDF_OK, "");
                        DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
                    }
                }
                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "PDF";
                info.loggingAppCode = "CWS";
                info.flussoID = String.Empty;
                info.clientID = idClient.ToString();
                info.activeObjectCF = codiceFiscaleRichiedente;
                info.activeObjectIP = String.Empty;
                info.passiveObjectCF = String.Empty;
                log.Info(info);
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel recupero del certificato con identificativo " + ciu + " dettagli: " + ex.Message,
                    "ERR_147",
                    "Certi.WS.Business.BUSManager",
                    "FindPdf",
                    "Recupero certificato richiesto",
                    null,
                    "CIU: " + ciu,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return documento;
        }

        # endregion

        # region Metodi pubblici

        /// <summary>
        /// Verifica la presenza di un cittadino, intestatario dei certificati, nell'archivio dell'Anagrafe CdR
        /// </summary>
        /// <param name="transactionRequest">Credenziali del client chiamante<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.TransactionRequestType/></param>
        /// <param name="credenziali">IdFlusso (CdR) + credenziali del client chiamante<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.CredenzialiType/></param>
        /// <param name="anagrafica">Dati anagrafici dell'intestatario del certificato<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.AnagraficaType/></param>
        /// <returns>Intestatario trovato o no<see cref="System.Boolean"/></returns>
        public bool InviaRichiestaCredenziali(TransactionRequestType transactionRequest, out CredenzialiType credenziali)
        {
            int idClient = 0;
            int idFlusso = 0;
            int status = (int)Status.R_INIZIALIZZAZIONE_KO;
            string codind = String.Empty;
            bool resp = false;
            credenziali = new CredenzialiType();
            log.Info("Sono qui 3");
            try
            {
                ProfiloClient.ClientsRow[] clientRows = CacheManager<ProfiloClient.ClientsDataTable>.
                    get(CacheKeys.CLIENTS_WS, VincoloType.NONE).Select("Public_ID ='" + transactionRequest.sistema + "'") as ProfiloClient.ClientsRow[];
                if (clientRows == null || clientRows.Length == 0)
                {
                    ManagedException mex = new ManagedException("Errore nella ricerca del client",
                        "ERR_008",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCredenziali",
                        "Verifica la presenza di un cittadino, intestatario dei certificati, nell'archivio dell'Anagrafe CdR",
                        string.Empty,
                        string.Empty,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                idClient = (clientRows[0] as ProfiloClient.ClientsRow).ID;
                BusProfiloRichiesta bp = new BusProfiloRichiesta();
                log.Debug("ho letto client id");
                /* controllo per univocità idTransazione -- forse da rimettere in futuro
                if (transactionRequest.sistema == "PGOV")
                {
                    if(string.IsNullOrEmpty(transactionRequest.idTransazione))
                    {
                        ManagedException mex = new ManagedException("Id transazione assente",
                            "ERR_001",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCredenziali",
                            "Verifica la presenza di un cittadino, intestatario dei certificati, nell'archivio dell'Anagrafe CdR",
                            transactionRequest.sistema,
                            "ActiveObjectCF: " + transactionRequest.codiceFiscaleRichiedente,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }

                    ProfiloRichiesta oldRichiesta = bp.GetRichiestaByTransazione(transactionRequest.idTransazione);
                    if (oldRichiesta.Richieste.Rows.Count != 0)
                    {
                        ManagedException mex = new ManagedException("Id transazione duplicato",
                            "ERR_001",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCredenziali",
                            "Verifica la presenza di un cittadino, intestatario dei certificati, nell'archivio dell'Anagrafe CdR",
                            transactionRequest.sistema,
                            "ActiveObjectCF: " + transactionRequest.codiceFiscaleRichiedente,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                }*/
                if (transactionRequest.codiceFiscaleIntestatario.Length == 16)
                {
                    log.Debug("prima di verifica codice fiscale 1");
                    codind = this.VerificaCodiceFiscale(idClient, transactionRequest.codiceFiscaleIntestatario);
                    log.Debug("dopo verifica codice fiscale 1");
                }
                else
                {
                    log.Debug("prima di verifica codice fiscale 1");
                    codind = transactionRequest.codiceFiscaleIntestatario;
                    log.Debug("dopo verifica codice fiscale 2");
                }
                resp = !String.IsNullOrEmpty(codind);
                if (resp) status = (int)Status.R_INIZIALIZZAZIONE_OK;
                log.Debug("sto per inserire una nuova richiesta");
                idFlusso = bp.AddNuovaRichiesta(idClient, transactionRequest.idPod, status, transactionRequest.codiceFiscaleRichiedente, transactionRequest.codiceFiscaleIntestatario, codind, transactionRequest.idTransazione);
                log.Debug("ho inserito una nuova richiesta " + idFlusso.ToString());
                credenziali.transactionData = transactionRequest;
                credenziali.idFlusso = idFlusso.ToString();

                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "INI";
                info.loggingAppCode = "CWS";
                info.flussoID = idFlusso.ToString();
                info.clientID = transactionRequest.sistema;
                info.activeObjectCF = transactionRequest.codiceFiscaleRichiedente;
                info.activeObjectIP = String.Empty;
                info.passiveObjectCF = transactionRequest.codiceFiscaleIntestatario;
                log.Info(info);
            }
            catch (ManagedException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness)",
                    "ERR_008",
                    "Certi.WS.Business.BUSManager",
                    "InviaNuovaRichiesta",
                    "Inizializzazione nuova richiesta",
                      ex.InnerException.Message + " " + ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite,
                    "PassiveObjectCF: " + transactionRequest.codiceFiscaleIntestatario,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            log.Debug("sto per tornare " + idFlusso.ToString());
            return resp;
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenziali"></param>
        /// <param name="certificati"></param>
        /// <returns></returns>
        public InfoCertificatoType[] InviaVerificaEmettibilita(CredenzialiType credenziali, InfoCertificatoType[] certificati)
        {
            ProfiloRichiestaStub rs = null;
            BusProfiloRichiesta pr = null;
            BUSVeriEmettibilita bve = null;
            BUSXmlGenerator bx = null;
            XmlDocument doc = new XmlDocument();
            InfoCertificatoType[] resp = null;
            bool esitoVerifica = false;
            int idFlusso = 0;
            int idClient = 0;

            try
            {
                idFlusso = int.Parse(credenziali.idFlusso);
                pr = new BusProfiloRichiesta();
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));
                if (!VerificaFormaleCredenziali(credenziali, rs, out idClient))
                {
                    ManagedException mex = new ManagedException("Errore credenziali",
                        "ERR_008",
                        "Certi.WS.Business.BUSManager",
                        "InviaVerificaEmettibilita",
                        "Invio certificati emettibili",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                if (!VerificaStatusRichiesta(StepRichiesta.VERIFICA_EMETTIBILITA, (Status)Enum.Parse(typeof(Status), rs.StatusID.ToString())))
                {
                    ManagedException mex = new ManagedException("Richiesta con status sbagliato",
                        "ERR_010",
                        "Certi.WS.Business.BUSManager",
                        "InviaVerificaEmettibilita",
                        "Invio certificati emettibili",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                // verifiche formali
                ProfiloRichiesta.CertificatiDataTable tabForm = VerificaFormaleEmettibilita(idClient, certificati);
                rs.setTabCertificati(tabForm);
                // inserimento in DB certificati richiesti
                rs.StatusID = (int)Status.R_VERIFICA_EMETTIBILITA;
                pr.RenewRichiesta(rs);


                // verifica emettibilità in backend
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));
                bve = new BUSVeriEmettibilita();
                // bve.VerificaEmettibilita(rs.CodiceIndividualeIntestatario, (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select("STATUS_ID=" + (int)Status.C_VERIFICA_EMETTIBILITA));
                string esenzioneId = "";
                string idCertificato = ((ProfiloRichiesta.CertificatiRow)tabForm.Rows[0]).TIPO_CERTIFICATO_ID.ToString();
                string tipoUso = ((ProfiloRichiesta.CertificatiRow)tabForm.Rows[0]).TIPO_USO_ID.ToString();
                if (tipoUso == "3")
                {
                    esenzioneId = ((ProfiloRichiesta.CertificatiRow)tabForm.Rows[0]).ESENZIONE_ID.ToString();
                }
                bve.VerificaEmettibilitaSIPO(rs.CodiceIndividualeIntestatario, (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select("STATUS_ID=" + (int)Status.C_VERIFICA_EMETTIBILITA), credenziali.transactionData, idCertificato, tipoUso, esenzioneId);
                esitoVerifica = (bve.VeriResponse.Messaggi.Select("Codice=0").Length > 0);

                switch (esitoVerifica)
                {
                    case false:
                        rs.StatusID = (int)Status.R_VERIFICA_EMETTIBILITA_KO;
                        foreach (ProfiloRichiesta.CertificatiRow cr in rs.Dataset.Certificati.Rows)
                        { cr.STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_KO; }
                        pr.RenewRichiesta(rs);
                        break;
                    case true:
                        if (bve.VeriResponse.Verifica.Rows.Count == 0)
                        {
                            ManagedException mex = new ManagedException("Nessun certificato emettibile",
                                "ERR_010",
                                "Certi.WS.Business.BUSManager",
                                "InviaVerificaEmettibilita",
                                "Invio certificati emettibili",
                                string.Empty,
                                "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                        DateTime dtEmissione = DateTime.Now;
                        foreach (VeriEmettibilitaCertificatiResponse.MessaggiRow row in bve.VeriResponse.Messaggi.Rows)
                        {
                            Messaggio mes = new Messaggio(row);
                            string backendID = mes.Variabili["Certificato"][0];
                            string filtro = "T_BACKEND_ID='" + backendID + "'";
                            ProfiloRichiesta.CertificatiRow[] rows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select(filtro);
                            for (int i0 = 0; i0 < rows.Length; i0++)
                            {
                                for (int i1 = i0 + 1; i1 < rows.Length; i1++)
                                {
                                    if (rows[i1].TIPO_USO_ID == rows[i0].TIPO_USO_ID)
                                    {
                                        ManagedException mex = new ManagedException("Richiesta duplicata",
                                            "ERR_006",
                                            "Certi.WS.Business.BUSManager",
                                            "InviaVerificaEmettibilita",
                                            "Verifica di emittibilità dei certificati",
                                            string.Empty,
                                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                            null);
                                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWSB", mex);
                                        log.Error(error);
                                        throw mex;
                                    }
                                }

                                ProfiloRichiesta.CertificatiRow r = rows[i0];
                                int messageCode = int.Parse(mes.Codice);
                                if (messageCode == 0)
                                {
                                    bx = new BUSXmlGenerator();
                                    doc = bx.MakeXml(r, bve.VeriResponse.Verifica[0].RawXml, dtEmissione, rs.ClientID, rs.UfficioID);
                                    r.STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_OK;
                                    r.DATA_EMISSIONE = dtEmissione;
                                    //aggiornamento dati anagrafici richiedente
                                    rs.CognomeIntestatario = doc.SelectSingleNode(@"//generalita/cognome").InnerText;
                                    rs.NomeIntestatario = doc.SelectSingleNode(@"//generalita/nome").InnerText;
                                    bool ciuConstraint = true;
                                    int cont = 0;
                                    do
                                    {
                                        try
                                        {
                                            XmlDocument docFinal = bx.GetCIU(doc);
                                            r.CIU = docFinal.SelectSingleNode(@"//indice/ciu").InnerText;
                                            r.XML_CERTIFICATO = docFinal.OuterXml;
                                            //pr.RenewCertificati(rows);
                                            pr.RenewRichiesta(rs);
                                            ciuConstraint = false;
                                        }
                                        catch (ManagedException ex)
                                        {
                                            if (ex.InnerExceptionMessage.Equals("ORA-00001: unique constraint (CERTIOL_IDX.CERTIFICATI_CIU_UK) violated"))
                                            {
                                                r.CIU = String.Empty;
                                                r.XML_CERTIFICATO = String.Empty;
                                                cont++;
                                            }
                                            else
                                            {
                                                throw ex;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ManagedException mex = new ManagedException("Errore generazione CIU dettagli: " + ex.Message,
                                                "ERR_148",
                                                "Certi.WS.Business.BUSManager",
                                                "InviaVerificaEmettibilita",
                                                "Invio della verifica di emettibilità del certificato",
                                                string.Empty,
                                                "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                                ex.InnerException);
                                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                                            log.Error(error);
                                            throw mex;
                                        }
                                    }
                                    while (ciuConstraint && cont < 5);
                                }
                                else
                                {
                                    r.STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_KO;
                                    pr.RenewCertificati(rows);
                                }
                            }
                        }
                        rs.StatusID = (rs.Dataset.Certificati.Select("STATUS_ID=" + (int)Status.C_VERIFICA_EMETTIBILITA_OK).Length > 0) ?
                            (int)Status.R_VERIFICA_EMETTIBILITA_OK : (int)Status.R_VERIFICA_EMETTIBILITA_KO;
                        pr.RenewStatusRichiesta((ProfiloRichiesta.RichiesteRow[])rs.Dataset.Richieste.Select());
                        break;
                }             
                ProfiloRichiesta.CertificatiRow[] respRows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select();
                resp = new InfoCertificatoType[respRows.Length];
                for (int i = 0; i < respRows.Length; i++)
                {
                    resp[i] = new InfoCertificatoType();
                    resp[i].idNomeCertificato = respRows[i].T_PUBLIC_ID;
                    resp[i].IdUso = respRows[i].TIPO_USO_ID.ToString();
                    resp[i].idMotivoEsenzione = respRows[i].ESENZIONE_ID;
                    resp[i].emettibile = (respRows[i].STATUS_ID == (int)Status.C_VERIFICA_EMETTIBILITA_OK) ? true : false;
                    resp[i].emettibileSpecified = true;
                    resp[i].dicituraVariabile = resp[i].dicituraVariabile;
                }


                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "VEM";
                info.loggingAppCode = "CWS";
                info.flussoID = idFlusso.ToString();
                info.clientID = credenziali.transactionData.sistema;
                info.activeObjectCF = rs.CodiceFiscaleRichiedente;
                info.activeObjectIP = String.Empty;
                info.passiveObjectCF = rs.CodiceFiscaleIntestatario;
                log.Info(info);
            }
            catch (ManagedException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore invio della verifica di emettibilità",
                                            "ERR_022",
                                            "Certi.WS.Business.BUSManager",
                                            "InviaVerificaEmettibilita",
                                            "Invio della verifica di emettibilità del certificato",
                                            string.Empty,
                                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }*/

        public InfoCertificatoType[] InviaVerificaEmettibilitaSIPO(CredenzialiType credenziali, InfoCertificatoType[] certificati)
        {
            ProfiloRichiestaStub rs = null;
            BusProfiloRichiesta pr = null;
            BUSVeriEmettibilita bve = null; 
            InfoCertificatoType[] resp = null;
            int idFlusso = 0;
            int idClient = 0;

            try
            {
                idFlusso = int.Parse(credenziali.idFlusso);
                pr = new BusProfiloRichiesta();
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));
                if (!VerificaFormaleCredenziali(credenziali, rs, out idClient))
                {
                    ManagedException mex = new ManagedException("Errore credenziali",
                        "ERR_008",
                        "Certi.WS.Business.BUSManager",
                        "InviaVerificaEmettibilita",
                        "Invio certificati emettibili",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                if (!VerificaStatusRichiesta(StepRichiesta.VERIFICA_EMETTIBILITA, (Status)Enum.Parse(typeof(Status), rs.StatusID.ToString())))
                {
                    ManagedException mex = new ManagedException("Richiesta con status sbagliato",
                        "ERR_010",
                        "Certi.WS.Business.BUSManager",
                        "InviaVerificaEmettibilita",
                        "Invio certificati emettibili",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                // verifiche formali
                ProfiloRichiesta.CertificatiDataTable tabForm = VerificaFormaleEmettibilita(idClient, certificati);
                rs.setTabCertificati(tabForm);
                // inserimento in DB certificati richiesti
                rs.StatusID = (int)Status.R_VERIFICA_EMETTIBILITA;
                pr.RenewRichiesta(rs);
                // verifica emettibilità in backend
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));
                bve = new BUSVeriEmettibilita();
                string esenzioneId = "";
                string idCertificato = ((ProfiloRichiesta.CertificatiRow)tabForm.Rows[0]).TIPO_CERTIFICATO_ID.ToString();
                string tipoUso = ((ProfiloRichiesta.CertificatiRow)tabForm.Rows[0]).TIPO_USO_ID.ToString();
                if (tipoUso == "3")
                {
                    esenzioneId = ((ProfiloRichiesta.CertificatiRow)tabForm.Rows[0]).ESENZIONE_ID.ToString();
                }
                VerificaEmettibilita verificaEmettibilita = bve.VerificaEmettibilitaSIPO(rs.CodiceIndividualeIntestatario, (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select("STATUS_ID=" + (int)Status.C_VERIFICA_EMETTIBILITA), credenziali.transactionData, idCertificato, tipoUso, esenzioneId);

                switch (verificaEmettibilita.IsEmettibile)
                {
                    case false:
                        rs.StatusID = (int)Status.R_VERIFICA_EMETTIBILITA_KO;
                        foreach (ProfiloRichiesta.CertificatiRow cr in rs.Dataset.Certificati.Rows)
                        { cr.STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_KO; }
                        pr.RenewRichiesta(rs);
                        break;
                    case true:
                        if (verificaEmettibilita.Certificato is null)
                        {
                            ManagedException mex = new ManagedException("Nessun certificato emettibile",
                                "ERR_010",
                                "Certi.WS.Business.BUSManager",
                                "InviaVerificaEmettibilita",
                                "Invio certificati emettibili",
                                string.Empty,
                                "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                        DateTime dtEmissione = DateTime.Now;
                        string filtro = "TIPO_CERTIFICATO_ID='" + idCertificato + "'";
                        ProfiloRichiesta.CertificatiRow[] rows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select(filtro);
                        for (int i0 = 0; i0 < rows.Length; i0++)
                        {
                            ProfiloRichiesta.CertificatiRow r = rows[i0];                           
                            r.STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_OK;
                            r.DATA_EMISSIONE = dtEmissione;                            
                            //aggiornamento dati anagrafici richiedente
                             rs.CognomeIntestatario = credenziali.transactionData.cognomeIntestatario;
                             rs.NomeIntestatario = credenziali.transactionData.nomeIntestatario;
                            bool ciuConstraint = true;
                            int cont = 0;
                            do
                            {
                                try
                                {
                                    
                                    r.CIU = verificaEmettibilita.CIU;   
                                    pr.RenewRichiesta(rs);
                                    ciuConstraint = false;
                                }
                                catch (ManagedException ex)
                                {
                                    if (ex.InnerExceptionMessage.Equals("ORA-00001: unique constraint (CERTIOL_IDX.CERTIFICATI_CIU_UK) violated"))
                                    {
                                        r.CIU = String.Empty;
                                        r.XML_CERTIFICATO = String.Empty;
                                        cont++;
                                    }
                                    else
                                    {
                                        throw ex;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ManagedException mex = new ManagedException("Errore generazione CIU dettagli: " + ex.Message,
                                        "ERR_148",
                                        "Certi.WS.Business.BUSManager",
                                        "InviaVerificaEmettibilita",
                                        "Invio della verifica di emettibilità del certificato",
                                        string.Empty,
                                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                        ex.InnerException);
                                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                                    log.Error(error);
                                    throw mex;
                                }
                            }
                            while (ciuConstraint && cont < 5);

                        }

                        rs.StatusID = (rs.Dataset.Certificati.Select("STATUS_ID=" + (int)Status.C_VERIFICA_EMETTIBILITA_OK).Length > 0) ?
                            (int)Status.R_VERIFICA_EMETTIBILITA_OK : (int)Status.R_VERIFICA_EMETTIBILITA_KO;
                        pr.RenewStatusRichiesta((ProfiloRichiesta.RichiesteRow[])rs.Dataset.Richieste.Select());
                        break;
                }

                /* risposta */
                ProfiloRichiesta.CertificatiRow[] respRows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select();
                resp = new InfoCertificatoType[respRows.Length];
                for (int i = 0; i < respRows.Length; i++)
                {
                    resp[i] = new InfoCertificatoType();
                    resp[i].idNomeCertificato = respRows[i].T_PUBLIC_ID;
                    resp[i].IdUso = respRows[i].TIPO_USO_ID.ToString();
                    resp[i].idMotivoEsenzione = respRows[i].ESENZIONE_ID;
                    resp[i].emettibile = (respRows[i].STATUS_ID == (int)Status.C_VERIFICA_EMETTIBILITA_OK) ? true : false;
                    resp[i].emettibileSpecified = true;
                    resp[i].dicituraVariabile = resp[i].dicituraVariabile;
                }


                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "VEM";
                info.loggingAppCode = "CWS";
                info.flussoID = idFlusso.ToString();
                info.clientID = credenziali.transactionData.sistema;
                info.activeObjectCF = rs.CodiceFiscaleRichiedente;
                info.activeObjectIP = String.Empty;
                info.passiveObjectCF = rs.CodiceFiscaleIntestatario;
                log.Info(info);
            }
            catch (ManagedException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore invio della verifica di emettibilità",
                                            "ERR_022",
                                            "Certi.WS.Business.BUSManager",
                                            "InviaVerificaEmettibilita",
                                            "Invio della verifica di emettibilità del certificato",
                                            string.Empty,
                                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenziali"></param>
        /// <param name="certificati"></param>
        /// <returns></returns>
        public CertificatoType[] InviaRichiestaCertificati(CredenzialiType credenziali, InfoCertificatoType[] certificati)
        {
            ProfiloRichiestaStub rs = null;
            BusProfiloRichiesta pr = null;
            BUSPdfGeneratorSIPO bp = null;
            BUSFirma bf = null;
            BUSStorage bs = null;
            CertificatoType[] resp = null;
            int idFlusso = 0;
            int idClient = 0;

            try
            {
                log.Debug("debbo leggere id flusso");
                idFlusso = int.Parse(credenziali.idFlusso);
                log.Debug("ho letto id flusso");
                pr = new BusProfiloRichiesta();
                log.Debug("creo profilo richiesta");
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));
                log.Debug(" ho creato profilo richiesta");
                if (!VerificaFormaleCredenziali(credenziali, rs, out idClient))
                {
                    ManagedException mex = new ManagedException("Credenziali errate",
                        "ERR_008",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "Invio della richiesta dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                if (!VerificaStatusRichiesta(StepRichiesta.RICHIESTA_CERTIFICATI, (Status)Enum.Parse(typeof(Status), rs.StatusID.ToString())))
                {
                    ManagedException mex = new ManagedException("Errore verifica status richiesta",
                        "ERR_010",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "Invio della richiesta dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                foreach (InfoCertificatoType cert in certificati)
                {
                    // che succede se l'array dei certificati contiene più volte lo stesso tipo di certificat???
                    // controllare in entrata!!!
                    ProfiloRichiesta.CertificatiRow[] rows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select("T_PUBLIC_ID='" + cert.idNomeCertificato + "'");               
                    if (rows.Length != 1)
                    {
                        ManagedException mex = new ManagedException("Presenti più certificati",
                            "ERR_030",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    ProfiloRichiesta.CertificatiRow r = rows[0];
                    ProfiloRichiesta.RichiesteRow rowRichiesta = (ProfiloRichiesta.RichiesteRow)rs.Dataset.Richieste.Select("ID=" + rows[0].RICHIESTA_ID + "").First();
                    if (r.STATUS_ID != (int)Status.C_VERIFICA_EMETTIBILITA_OK)
                    {
                        ManagedException mex = new ManagedException("Certificato non emettibile",
                            "ERR_007",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    if (r.TIPO_USO_ID != int.Parse(cert.IdUso))
                    {
                        ManagedException mex = new ManagedException("Incompatibilità dei dati della richiesta: tipo uso",
                            "ERR_011",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    if (r.ESENZIONE_ID != cert.idMotivoEsenzione)
                    {
                        if ((idClient == 2 && int.Parse(cert.IdUso) == 3 && cert.idMotivoEsenzione == null) || (idClient != 2 && int.Parse(cert.IdUso) == 3 && cert.idMotivoEsenzione == string.Empty))
                        {

                        }
                        else
                        {
                            ManagedException mex = new ManagedException("Incompatibilità dei dati della richiesta: motivo esenzione",
                                "ERR_040",
                                "Certi.WS.Business.BUSManager",
                                "InviaRichiestaCertificati",
                                "Invio della richiesta dei certificati",
                                string.Empty,
                                "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                    }

                    bp = new BUSPdfGeneratorSIPO();                   
                    bp.MakePdf( credenziali.idFlusso,r, rowRichiesta, cert);
                    if (ConfigurationManager.AppSettings["GetTimbro"] == "1")
                    {
                        bf = new BUSFirma();
                        //  r.T_DOCUMENTO = bf.SetFirmaDOUSipo(r.T_DOCUMENTO);
                        r.T_DOCUMENTO = bf.SetFirmaDouFendSipo(r.T_DOCUMENTO);
                    }

                    bs = new BUSStorage(r.CIU, rs.CodiceFiscaleIntestatario, r.DATA_EMISSIONE,
                        "pdf", r.T_DOCUMENTO, int.Parse(r.STATUS_ID.ToString()), int.Parse(r.ID.ToString()));
                    bool es = bs.SaveCertificato();
                    if (!es)
                    {
                        ManagedException mex = new ManagedException("Errore durante il salvataggio del documento pdf con CIU: " + r.CIU,
                            "ERR_002",
                            "Certi.WS.Business",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + rs.CodiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    r.STATUS_ID = (int)Status.C_EMISSIONE_OK;
                }

                rs.StatusID = (int)Status.R_EMISSIONE_OK;
                /* aggiorno in BD */
                try
                {
                    pr.RenewRichiesta(rs);
                }
                catch (ManagedException ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                catch (Exception ex)
                {
                    ManagedException mex = new ManagedException("Errore aggiornamento richiesta emissione",
                        "ERR_002",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "invia la richiesta di emissione dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        ex.InnerException);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                /* risposta */
                ProfiloRichiesta.CertificatiRow[] respRows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select();
                ArrayList respTmp = new ArrayList();
                for (int i = 0; i < respRows.Length; i++)
                {
                    if (respRows[i].STATUS_ID == (int)Status.C_EMISSIONE_OK)
                    {
                        CertificatoType certificato = new CertificatoType();
                        certificato.idNomeCertificato = respRows[i].T_PUBLIC_ID;
                        certificato.IdUso = respRows[i].T_TIPO_USO;
                        certificato.idMotivoEsenzione = respRows[i].ESENZIONE_ID;
                        certificato.emettibile = true;
                        certificato.bin = respRows[i].T_DOCUMENTO;
                        certificato.IdDocumento = respRows[i].CIU;
                        respTmp.Add(certificato);
                    }
                }

                resp = (CertificatoType[])respTmp.ToArray(typeof(CertificatoType));


                //resp = new CertificatoType[respRows.Length];
                //for (int i = 0; i < respRows.Length; i++)
                //{
                //    resp[i] = new CertificatoType();
                //    resp[i].idNomeCertificato = respRows[i].T_PUBLIC_ID;
                //    resp[i].IdUso = respRows[i].T_TIPO_USO;
                //    resp[i].idMotivoEsenzione = respRows[i].ESENZIONE_ID;
                //    resp[i].emettibile = (respRows[i].STATUS_ID == (int)Status.C_EMISSIONE_OK) ? true : false;
                //    resp[i].bin = respRows[i].T_DOCUMENTO;
                //    resp[i].IdDocumento = respRows[i].CIU;
                //}



                /* invio la richiesta di contabilizzazione al backend */

                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "REM";
                info.loggingAppCode = "CWS";
                info.flussoID = rs.Id.ToString();
                info.clientID = credenziali.transactionData.sistema;
                info.activeObjectCF = rs.CodiceFiscaleRichiedente;
                info.activeObjectIP = String.Empty;
                info.passiveObjectCF = rs.CodiceFiscaleIntestatario;
                log.Info(info);
            }
            catch (ManagedException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore invio richiesta emissione",
                        "ERR_032",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "invia la richiesta di emissione dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                log.Debug(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw mex;
            }
            return resp;
        }


        // N.R. NUOVO METODO CERTIFICATI 09/2020
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenziali"></param>
        /// <param name="certificati"></param>
        /// <returns></returns>
        public CertificatoType[] InviaRichiestaCertificatiSIPO(CredenzialiType credenziali, InfoCertificatoType[] certificati)
        {
            ProfiloRichiestaStub rs = null;
            BusProfiloRichiesta pr = null;
            BUSPdfGeneratorSIPO bp = null;
            BUSFirma bf = null;
            BUSStorage bs = null;
            CertificatoType[] resp = null;
            int idFlusso = 0;
            int idClient = 0;

            try
            {
                log.Debug("debbo leggere id flusso");
                idFlusso = int.Parse(credenziali.idFlusso);
                log.Debug("ho letto id flusso");
                pr = new BusProfiloRichiesta();
                log.Debug("creo profilo richiesta");
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));
                log.Debug(" ho creato profilo richiesta");
                if (!VerificaFormaleCredenziali(credenziali, rs, out idClient))
                {
                    ManagedException mex = new ManagedException("Credenziali errate",
                        "ERR_008",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "Invio della richiesta dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                if (!VerificaStatusRichiesta(StepRichiesta.RICHIESTA_CERTIFICATI, (Status)Enum.Parse(typeof(Status), rs.StatusID.ToString())))
                {
                    ManagedException mex = new ManagedException("Errore verifica status richiesta",
                        "ERR_010",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "Invio della richiesta dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                foreach (InfoCertificatoType cert in certificati)
                {
                    // che succede se l'array dei certificati contiene più volte lo stesso tipo di certificat???
                    // controllare in entrata!!!
                    ProfiloRichiesta.CertificatiRow[] rows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select("T_PUBLIC_ID='" + cert.idNomeCertificato + "'");
                    ProfiloRichiesta.RichiesteRow rowRichiesta = ((ProfiloRichiesta.RichiesteRow)rs.Dataset.Richieste[0]);

                    if (rows.Length != 1)
                    {
                        ManagedException mex = new ManagedException("Presenti più certificati",
                            "ERR_030",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    ProfiloRichiesta.CertificatiRow r = rows[0];
                    if (r.STATUS_ID != (int)Status.C_VERIFICA_EMETTIBILITA_OK)
                    {
                        ManagedException mex = new ManagedException("Certificato non emettibile",
                            "ERR_007",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    if (r.TIPO_USO_ID != int.Parse(cert.IdUso))
                    {
                        ManagedException mex = new ManagedException("Incompatibilità dei dati della richiesta: tipo uso",
                            "ERR_011",
                            "Certi.WS.Business.BUSManager",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    if (r.ESENZIONE_ID != cert.idMotivoEsenzione)
                    {
                        if ((idClient == 2 && int.Parse(cert.IdUso) == 3 && cert.idMotivoEsenzione == null) || (idClient != 2 && int.Parse(cert.IdUso) == 3 && cert.idMotivoEsenzione == string.Empty))
                        {

                        }
                        else
                        {
                            ManagedException mex = new ManagedException("Incompatibilità dei dati della richiesta: motivo esenzione",
                                "ERR_040",
                                "Certi.WS.Business.BUSManager",
                                "InviaRichiestaCertificati",
                                "Invio della richiesta dei certificati",
                                string.Empty,
                                "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                                null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                    }

                    // DA TESTARE N.R. 09/2020 
                    bp = new BUSPdfGeneratorSIPO();
                    bp.MakePdf(credenziali.idFlusso,r,rowRichiesta, cert);              
                    if (ConfigurationManager.AppSettings["GetTimbro"] == "1")
                    {
                        bf = new BUSFirma();
                        //  r.T_DOCUMENTO = bf.SetFirmaDOUSipo(r.T_DOCUMENTO);
                        r.T_DOCUMENTO = bf.SetFirmaDouFendSipo(r.T_DOCUMENTO);
                    }

                    bs = new BUSStorage(r.CIU, rs.CodiceFiscaleIntestatario, r.DATA_EMISSIONE,
                        "pdf", r.T_DOCUMENTO, int.Parse(r.STATUS_ID.ToString()), int.Parse(r.ID.ToString()));
                    bool es = bs.SaveCertificato();
                    if (!es)
                    {
                        ManagedException mex = new ManagedException("Errore durante il salvataggio del documento pdf con CIU: " + r.CIU,
                            "ERR_002",
                            "Certi.WS.Business",
                            "InviaRichiestaCertificati",
                            "Invio della richiesta dei certificati",
                            string.Empty,
                            "PassiveObjectCF: " + rs.CodiceFiscaleIntestatario,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    r.STATUS_ID = (int)Status.C_EMISSIONE_OK;
                }

                rs.StatusID = (int)Status.R_EMISSIONE_OK;
                /* aggiorno in BD */
                try
                {
                    pr.RenewRichiesta(rs);
                }
                catch (ManagedException ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                catch (Exception ex)
                {
                    ManagedException mex = new ManagedException("Errore aggiornamento richiesta emissione",
                        "ERR_002",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "invia la richiesta di emissione dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        ex.InnerException);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                /* risposta */
                ProfiloRichiesta.CertificatiRow[] respRows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select();
                ArrayList respTmp = new ArrayList();
                for (int i = 0; i < respRows.Length; i++)
                {
                    if (respRows[i].STATUS_ID == (int)Status.C_EMISSIONE_OK)
                    {
                        CertificatoType certificato = new CertificatoType();
                        certificato.idNomeCertificato = respRows[i].T_PUBLIC_ID;
                        certificato.IdUso = respRows[i].T_TIPO_USO;
                        certificato.idMotivoEsenzione = respRows[i].ESENZIONE_ID;
                        certificato.emettibile = true;
                        certificato.bin = respRows[i].T_DOCUMENTO;
                        certificato.IdDocumento = respRows[i].CIU;
                        respTmp.Add(certificato);
                    }
                }

                resp = (CertificatoType[])respTmp.ToArray(typeof(CertificatoType));
                /* invio la richiesta di contabilizzazione al backend */
                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "REM";
                info.loggingAppCode = "CWS";
                info.flussoID = rs.Id.ToString();
                info.clientID = credenziali.transactionData.sistema;
                info.activeObjectCF = rs.CodiceFiscaleRichiedente;
                info.activeObjectIP = String.Empty;
                info.passiveObjectCF = rs.CodiceFiscaleIntestatario;
                log.Info(info);
            }
            catch (ManagedException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore invio richiesta emissione",
                        "ERR_032",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "invia la richiesta di emissione dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                log.Debug(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw mex;
            }
            return resp;
        }


        // N.R. DA TESTARE NON MODIFICATO
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenziali"></param>
        /// <param name="certificatoEmesso"></param>
        /// <returns></returns>
        public bool InviaConfermaStampa(CredenzialiType credenziali, String[] certificatoEmesso)
        {
            ProfiloRichiestaStub rs = null;
            BusProfiloRichiesta pr = null;
            bool resp = false;
            int idFlusso = 0;
            int idClient = 0;

            try
            {
                idFlusso = int.Parse(credenziali.idFlusso);
                pr = new BusProfiloRichiesta();
                rs = new ProfiloRichiestaStub(pr.GetRichiesta(idFlusso));

                if (!VerificaFormaleCredenziali(credenziali, rs, out idClient))
                {
                    ManagedException mex = new ManagedException("Credenziali errate",
                        "ERR_008",
                        "Certi.WS.Business.BUSManager",
                        "InviaRichiestaCertificati",
                        "Invio della richiesta dei certificati",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                if (!VerificaStatusRichiesta(StepRichiesta.CONFERMA_STAMPA, (Status)Enum.Parse(typeof(Status), rs.StatusID.ToString())))
                {
                    ManagedException mex = new ManagedException("Errore richiesta conferma stampa",
                        "ERR_010",
                        "Certi.WS.Business.BUSManager",
                        "InviaConfermaStampa",
                        "Invia conferma di stamap pdf",
                        string.Empty,
                        "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }

                rs.StatusID = (int)Status.R_CONFERMA_STAMPA;
                foreach (string docID in certificatoEmesso)
                {
                    ProfiloRichiesta.CertificatiRow[] rows = (ProfiloRichiesta.CertificatiRow[])rs.Dataset.Certificati.Select("CIU='" + docID + "' and RICHIESTA_ID = " + idFlusso);
                    if (rows.Length != 1)
                    {
                        ManagedException mex = new ManagedException("Presenti più di un documento nella richiesta con CIU " + docID,
                            "ERR_010",
                            "Certi.WS.Business.BUSManager",
                            "InviaConfermaStampa",
                            "Invia conferma di stamap pdf",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                             null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    ProfiloRichiesta.CertificatiRow r = rows[0];
                    r.STATUS_ID = (int)Status.C_GENERAZIONE_PDF_OK;
                }

                //rs.StatusID = (rs.Dataset.Certificati.Rows.Count == certificatoEmesso.Length) ? (int)Status.R_CONFERMA_STAMPA_COMPLETA : (int)Status.R_CONFERMA_STAMPA_PARZIALE;
                rs.StatusID = (rs.Dataset.Certificati.Select("STATUS_ID = " + (int)Status.C_EMISSIONE_OK).Length == 0) ? (int)Status.R_CONFERMA_STAMPA_COMPLETA : (int)Status.R_CONFERMA_STAMPA_PARZIALE;
                pr.RenewRichiesta(rs);
                resp = true;
            }
            catch (ManagedException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore invio congferma stampa dettagli: " + ex.Message,
                            "ERR_148",
                            "Certi.WS.Business.BUSManager",
                            "InviaConfermaStampa",
                            "Invia conferma di stamap pdf",
                            string.Empty,
                            "PassiveObjectCF: " + credenziali.transactionData.codiceFiscaleIntestatario,
                             ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }
        #endregion

        #region Metodi privati
        /// <summary>
        /// Metodo di controllo della compatibilità della richiesta con lo status del flusso
        /// </summary>
        /// <param name="step">identificativo step flusso richiesta</param> 
        /// <returns>Restituisce true-false</returns>
        private bool VerificaStatusRichiesta(StepRichiesta step, Status status)
        {
            bool resp = false;
            switch (step)
            {
                case StepRichiesta.INIZIALIZZAZIONE:
                    resp = true;
                    break;

                case StepRichiesta.VERIFICA_EMETTIBILITA:
                    switch (status)
                    {
                        case Status.R_INIZIALIZZAZIONE_OK:
                        case Status.R_VERIFICA_EMETTIBILITA_OK:
                        case Status.R_VERIFICA_EMETTIBILITA_KO:
                        case Status.R_VERIFICA_EMETTIBILITA:
                            resp = true;
                            break;

                        default:
                            resp = false;
                            break;
                    }
                    break;

                case StepRichiesta.RICHIESTA_CERTIFICATI:
                    switch (status)
                    {
                        case Status.R_VERIFICA_EMETTIBILITA_OK:
                            resp = true;
                            break;

                        default:
                            resp = false;
                            break;
                    }
                    break;

                case StepRichiesta.CONFERMA_STAMPA:
                    switch (status)
                    {
                        case Status.R_EMISSIONE_OK:
                        case Status.R_CONFERMA_STAMPA_PARZIALE:
                            resp = true;
                            break;

                        default:
                            resp = false;
                            break;
                    }
                    break;
            }

            return resp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        private ProfiloRichiesta.CertificatiDataTable VerificaFormaleEmettibilita(int idClient, InfoCertificatoType[] arr)
        {
            // che succede se l'array dei certificati contiene più volte lo stesso tipo di certificat???
            // controllare in entrata!!!

            ProfiloRichiesta.CertificatiDataTable tab = new ProfiloRichiesta.CertificatiDataTable();
            try
            {
                ProfiloCertificato.CertificatoDataTable tbCert = CacheManager<ProfiloCertificato.CertificatoDataTable>.get(CacheKeys.CERTIFICATI_ATTIVI_WS, VincoloType.NONE);
                ProfiloMotivazione.MotivazioneDataTable tbMot = CacheManager<ProfiloMotivazione.MotivazioneDataTable>.get(CacheKeys.ESENZIONI_ATTIVI_WS, VincoloType.NONE);
                ProfiloTipoUso.TipoUsoDataTable tbTip = CacheManager<ProfiloTipoUso.TipoUsoDataTable>.get(CacheKeys.TIPI_USO_WS, VincoloType.NONE);
                for (int i = 0; i < arr.Length; i++)
                {
                    // controlli codifiche
                    ProfiloCertificato.CertificatoRow[] certs = (ProfiloCertificato.CertificatoRow[])tbCert.Select("PUBLIC_ID='" + arr[i].idNomeCertificato + "' and  CLIENT_ID='" + idClient + "'");
                    if (certs.Length == 0)
                    {
                        ManagedException mex = new ManagedException("Errore nella ricerca dei certificati",
                            "ERR_005",
                            "Certi.WS.Business.BUSManager",
                            "VerificaFormaleEmettibilita",
                            "Verifica della correttezza formale dei dati",
                            "ClientID: " + idClient,
                            string.Empty,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    ProfiloTipoUso.TipoUsoRow[] tips = (ProfiloTipoUso.TipoUsoRow[])tbTip.Select("ID='" + arr[i].IdUso + "'");
                    if (tips.Length == 0)
                    {
                        ManagedException mex = new ManagedException("Errore nella ricerca del tipo di uso",
                            "ERR_005",
                            "Certi.WS.Business.BUSManager",
                            "VerificaFormaleEmettibilita",
                            "Verifica della correttezza formale dei dati",
                            "ClientID: " + idClient,
                            string.Empty,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    if (!String.IsNullOrEmpty(arr[i].idMotivoEsenzione))
                    {
                        ProfiloMotivazione.MotivazioneRow[] esenzioni = (ProfiloMotivazione.MotivazioneRow[])tbMot.Select("ID='" + arr[i].idMotivoEsenzione + "'");
                        if (esenzioni.Length == 0)
                        {
                            ManagedException mex = new ManagedException("Errore nella ricerca delle esenzioni",
                                "ERR_005",
                                "Certi.WS.Business.BUSManager",
                                "VerificaFormaleEmettibilita",
                                "Verifica della correttezza formale dei dati",
                                "ClientID: " + idClient,
                                string.Empty,
                                null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                    }

                    // controllo emettibilità tipo uso richiesto
                    ProfiloCertificato.CertificatoRow[] emett = (ProfiloCertificato.CertificatoRow[])tbCert.Select("PUBLIC_ID='" + arr[i].idNomeCertificato + "' and TIPO_USO_ID='" + arr[i].IdUso + "' and  CLIENT_ID='" + idClient + "'");
                    int status = (int)Status.C_VERIFICA_EMETTIBILITA; ;
                    if (emett.Length == 0) status = (int)Status.C_VERIFICA_EMETTIBILITA_KO;
                    else
                    {
                        if (emett[0].ATTIVO == 0)
                        {
                            ManagedException mex = new ManagedException("Errore: certificato non attivo",
                               "ERR_005",
                               "Certi.WS.Business.BUSManager",
                               "VerificaFormaleEmettibilita",
                               "Verifica della correttezza formale dei dati",
                               "ClientID: " + idClient,
                               string.Empty,
                               null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                        if (tips[0].Descrizione == "CARTA SEMPLICE" && String.IsNullOrEmpty(arr[i].idMotivoEsenzione))
                        {
                            ManagedException mex = new ManagedException("Errore nei parametri tipo d'uso - motivazione del certificato",
                                "ERR_003",
                                "Certi.WS.Business.BUSManager",
                                "VerificaFormaleEmettibilita",
                                "Verifica della correttezza formale dei dati",
                                "ClientID: " + idClient,
                                string.Empty,
                                null);
                            Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                            log.Error(error);
                            throw mex;
                        }
                        if (tips[0].Descrizione != "CARTA SEMPLICE" && !String.IsNullOrEmpty(arr[i].idMotivoEsenzione))
                        {
                            if (arr[i].idNomeCertificato == "C0001" && arr[i].idNomeCertificato == "C0002" && arr[i].idNomeCertificato == "C0003")
                            {
                                ManagedException mex = new ManagedException("Errore nei parametri tipo d'uso - motivazione del certificato",
                                       "ERR_004",
                                       "Certi.WS.Business.BUSManager",
                                       "VerificaFormaleEmettibilita",
                                       "Verifica della correttezza formale dei dati",
                                       "ClientID: " + idClient,
                                       string.Empty,
                                       null);
                                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                                log.Error(error);
                                throw mex;
                            }
                        }
                    }

                    // caricamento certificato in datatable
                    ProfiloRichiesta.CertificatiRow r = tab.NewCertificatiRow();
                    r.ID = -(i + 1);
                    r.RICHIESTA_ID = -1;
                    r.TIPO_CERTIFICATO_ID = certs[0].CERTID;
                    r.T_BACKEND_ID = certs[0].BACKEND_ID;
                    r.TIPO_USO_ID = int.Parse(arr[i].IdUso);
                    r.STATUS_ID = status;
                    r.ESENZIONE_ID = arr[i].idMotivoEsenzione;
                    tab.AddCertificatiRow(r);
                }
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nei parametri tipo d'uso - motivazione del certificato dettagli: " + ex.Message,
                                "ERR_148",
                                "Certi.WS.Business.BUSManager",
                                "VerificaFormaleEmettibilita",
                                "Verifica della correttezza formale dei dati",
                                "ClientID: " + idClient.ToString(),
                                string.Empty,
                                ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return tab;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private ComponenteFamigliaType[] TableToArrayRicercaComponenti(RicercaRalpoResponse.PersonaElencoDataTable tb)
        {
            ComponenteFamigliaType[] resp = new ComponenteFamigliaType[tb.Rows.Count];

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                ComponenteFamigliaType componente = new ComponenteFamigliaType();
                componente.codiceIndividuale = ((RicercaRalpoResponse.PersonaElencoRow)tb.Rows[i]).CodiceIndiv;
                componente.codiceFiscale = ((RicercaRalpoResponse.PersonaElencoRow)tb.Rows[i]).CodiceFis;
                componente.cognome = ((RicercaRalpoResponse.PersonaElencoRow)tb.Rows[i]).Cognome;
                componente.nome = ((RicercaRalpoResponse.PersonaElencoRow)tb.Rows[i]).NomeIndiv;
                resp[i] = componente;
            }
            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string getMessageFromCache(string m)
        {
            ListaDecodificaMessaggi el = CacheManager<ListaDecodificaMessaggi>.get(
                                CacheKeys.LISTA_DECODIFICA_MESSAGGI, VincoloType.FILESYSTEM);
            if (el.Errore.FindBycod(m) == null)
                return string.Format("livello,{0} codice,{1} (DESCRIZIONE ASSENTE!!!)", "X", m);
            else return el.Errore.FindBycod(m).desc;
        }

        private bool VerificaFormaleCredenziali(CredenzialiType credenziali, ProfiloRichiestaStub richiesta, out int idClient)
        {
            bool resp = false;
            log.Debug(" sono dentro verifica formale credenziali");
            try
            {
                ProfiloClient.ClientsDataTable tbClient = CacheManager<ProfiloClient.ClientsDataTable>.get(CacheKeys.CLIENTS_WS, VincoloType.NONE);
                ProfiloClient.ClientsRow[] clientRows = tbClient.Select("Public_ID ='" + credenziali.transactionData.sistema + "'") as ProfiloClient.ClientsRow[];
                if (clientRows.Length == 0)
                {
                    ManagedException mex = new ManagedException("Errore nella ricerca del client",
                        "ERR_005",
                        "Certi.WS.Business.BUSManager",
                        "VerificaFormaleEmettibilita",
                        "Verifica della correttezza formale dei dati",
                        string.Empty,
                        string.Empty,
                        null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;
                }
                idClient = (clientRows[0] as ProfiloClient.ClientsRow).ID;

                if (credenziali.idFlusso == richiesta.Id.ToString() &&
                    credenziali.transactionData.codiceFiscaleIntestatario == richiesta.CodiceFiscaleIntestatario &&
                    credenziali.transactionData.codiceFiscaleRichiedente == richiesta.CodiceFiscaleRichiedente &&
                    credenziali.transactionData.idPod == richiesta.UfficioID &&
                    credenziali.transactionData.idTransazione == richiesta.TransazioneID &&
                    idClient == richiesta.ClientID)
                    resp = true;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nella ricerca del client: dettagli " + ex.Message,
                        "ERR_055",
                        "Certi.WS.Business.BUSManager",
                        "VerificaFormaleEmettibilita",
                        "Verifica della correttezza formale dei dati",
                        string.Empty,
                        string.Empty,
                        ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return resp;

        }

        #endregion
    }
}
