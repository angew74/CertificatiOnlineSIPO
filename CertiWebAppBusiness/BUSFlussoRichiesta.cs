using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.Services.Protocols;
using System.Xml;
using log4net;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Caching;
using Microsoft.Web.Services2.Security.Tokens;
using Unisys.CdR.Servizi;
using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS;
using System.Configuration;
using System.Linq;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using Com.Unisys.CdR.Certi.WebApp.Business;
using Com.Unisys.Data;

namespace Com.Unisys.CdR.Certi.WebApp.Business
{
    
    public class Helper
    {
        public struct CertificatoRules
        {
            private bool _senzaDiritti;
            private bool _semplice;
            private bool _bollo;

            public CertificatoRules(bool senzaDir, bool sempl, bool bollo)
            {
                _senzaDiritti = senzaDir;
                _semplice = sempl;
                _bollo = bollo;
            }

            public bool IsGratuitoEnabled
            {
                get { return (_senzaDiritti && !_semplice && !_bollo); }
            }

            public bool IsSempliceEnabled
            {
                get { return _semplice; }
            }

            public bool IsBolloEnabled
            {
                get { return _bollo; }
            }
        }

        public static CertificatoRules GetCertificatoRules(ProfiloCertificato.CertificatoDataTable repCertif, string pidCert, string mode)
        {
            string filterSenzaDir = "PUBLIC_ID='" + pidCert + "' AND TIPO_USO_ID = 3";
            string filterSempl = "PUBLIC_ID='" + pidCert + "' AND TIPO_USO_ID = 1";
            string filterBollo = "PUBLIC_ID='" + pidCert + "' AND TIPO_USO_ID = 2";

            bool isSenzaDir = repCertif.Select(filterSenzaDir).Length > 0;
            bool isSempl = false;
            bool isBollo = false;

            if (mode != "0")
            {
                isSempl = repCertif.Select(filterSempl).Length > 0;
                isBollo = repCertif.Select(filterBollo).Length > 0;
            }
            return new CertificatoRules(isSenzaDir, isSempl, isBollo);
        }

    }
    /// <summary>
    /// Classe di business per la gestione delle richieste dati sui certificati
    /// </summary>
    public class BUSFlussoRichiesta
    {
        private static readonly ILog log = LogManager.GetLogger("BUSFlussoRichiesta");
        private ProxyWS.CertiService ws;

        public static string FixedClientId="";
        /// <summary>
        /// Costruttore - inizializza l'oggetto web service
        /// </summary>
        public BUSFlussoRichiesta()
        {
            /* completare */
            ws = new ProxyWS.CertiService();
            ws.Url = ConfigurationManager.AppSettings["CertiServiceUrl"];
            /* WSE security   */
            string key = "comunediroma2007";
            
            string uname = Encryption.Decrypt(ConfigurationManager.AppSettings["PortalAccount"], key);
            string upwd = Encryption.Decrypt(ConfigurationManager.AppSettings["PortalPassword"], key);
            UsernameToken myToken = new UsernameToken(uname, CryptoUty.PlainToSHA1(upwd), PasswordOption.SendPlainText);
            ws.RequestSoapContext.Security.Tokens.Add(myToken);
                      
        }

        #region Region chiamate WS

        /// <summary>
        /// Metodo per la chiamata al servizio web che effettua la 
        /// ricerca in base dati anagrafe del codice fiscale del richiedente.
        /// </summary>
        /// <param name="codiceFiscale" 
        /// <returns><c>String</c>Codice individuale richiedente</returns>        
        public String CallRicercaRichiedente(string codiceFiscale, string ip)
        {
            string resp = String.Empty;

            try
            {
                resp = ws.ricercaPersona(int.Parse(ConfigurationManager.AppSettings["ClientID"]), codiceFiscale);
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) "
                    + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                    ex.Detail.FirstChild.InnerText,
                    "Certi.WebApp.Business.BUSFlussoRichiesta",
                    "CallRicercaRichiedente",
                    "Chiamata WS per la ricerca del richiedente da codice fiscale",
                    "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                    "ActiveObjectCF: " + codiceFiscale,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }


        public ProxyWS.ComponenteFamigliaType[] GetComponenteFamiglias(string codiceFiscale)
        {
            ComponenteFamigliaType[] componenteFamiglias = null;
            componenteFamiglias = BusGestioneRicercheSIPO.FindComponenti(codiceFiscale);
            return componenteFamiglias;

        }

        /// <summary>
        /// Ricerca componenti della famiglia del richiedente
        /// </summary>
        /// <param name="codiceFiscale"></param>
        /// <returns></returns>
        public ProxyWS.ComponenteFamigliaType[] CallRicercaComponenti(string clientID, string codiceFiscale,
            string ipRichiedente)
        {
            ProxyWS.ComponenteFamigliaType[] resp = null;
            try
            {
                resp = ws.ricercaComponentiFamiglia(int.Parse(clientID), codiceFiscale);
                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "RCO";
                info.loggingAppCode = "CWA";
                info.flussoID = "";
                info.clientID = clientID;
                info.activeObjectCF = codiceFiscale;
                info.activeObjectIP = ipRichiedente;
                info.passiveObjectCF = "";
                log.Info(info);
            }

            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) "
                    + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                    ex.Detail.FirstChild.InnerText,
                    "Certi.WebApp.Business.BUSFlussoRichiesta",
                    "CallRicercaComponenti",
                    "Chiamata WS per la ricerca dei componenti da clientID e codice fiscale richiedente",
                    "ClientID: " + clientID,
                    "ActiveObjectCF: " + codiceFiscale,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return resp;

        }


        /// <summary>
        /// Invio richiesta di inizializzazione flusso richiesta
        /// </summary>
        /// <param name="cfRichiedente"></param>
        /// <param name="cfIntestatario"></param>
        /// <returns></returns>
        
        

        public string ExecuteInizializza(string idClient, string cfRichiedente, string cfIntestatario, string ipRichiedente)
        {
            string idFlusso = String.Empty;
            bool intestatarioTrovato = false;
            try
            {
                ProxyWS.TransactionRequestType request = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.TransactionRequestType();
                request.idTransazione = string.Empty;
                request.codiceFiscaleRichiedente = cfRichiedente;
                request.codiceFiscaleIntestatario = cfIntestatario;
                if(string.IsNullOrEmpty(FixedClientId))
                { 
                    ProfiloClient.ClientsDataTable clients =
                    CacheManager<ProfiloClient.ClientsDataTable>.get(CacheKeys.CLIENTS, VincoloType.NONE);
                    request.sistema = (clients.Select("ID = '" + idClient + "'")[0] as ProfiloClient.ClientsRow).Public_ID;
                }
                else
                {
                    request.sistema = FixedClientId;
                }

                request.idPod = string.Empty;
                ProxyWS.CredenzialiType credenziali;
                intestatarioTrovato = ws.richiestaCredenziali(request, out credenziali);

                //idFlusso = ws.richiestaCredenziali(int.Parse(idClient), String.Empty, cfRichiedente, cfIntestatario, out intestatarioTrovato);

                if (!intestatarioTrovato)
                    idFlusso = String.Empty;
                else
                    idFlusso = credenziali.idFlusso;

                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "INI";
                info.loggingAppCode = "CWA";
                info.flussoID = idFlusso;
                info.clientID = idClient;
                info.activeObjectCF = cfRichiedente;
                info.activeObjectIP = ipRichiedente;
                info.passiveObjectCF = cfIntestatario;
                log.Info(info);
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) "
                    + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                    ex.Detail.FirstChild.InnerText,
                    "Certi.WebApp.Business.BUSFlussoRichiesta",
                    "ExecuteInizializza",
                    "Inserimento nuovo flusso",
                    "ClientID: " + idClient,
                    "PassiveObjectCF: " + cfIntestatario,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return idFlusso;
        }

        /// <summary>
        /// Invio richiesta di verifica emettibilità certificato richiesto
        /// </summary>
        /// <param name="idFlusso"></param>
        /// <param name="idCertificato"></param>
        /// <returns></returns>
        public ProxyWS.InfoCertificatoType[] ExecuteVerifica(string idClient, string cfRichiedente, string ipRichiedente,
            string cfIntestatario, string idFlusso, string idPublicCertificato, string idUso, string motivoEsenzione,string cognomeIntestatario,string nomeIntestatario)
        {
            ProxyWS.InfoCertificatoType[] resp = null;
            ProxyWS.InfoCertificatoType[] rifs = new ProxyWS.InfoCertificatoType[1];
            ProxyWS.InfoCertificatoType rif = new ProxyWS.InfoCertificatoType();
            rif.idNomeCertificato = idPublicCertificato;
            rif.IdUso = idUso;
           // motivoEsenzione = string.Empty;
            rif.idMotivoEsenzione = motivoEsenzione;
            rifs[0] = rif;

            ProxyWS.CredenzialiType credenziali = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.CredenzialiType();
            credenziali.idFlusso = idFlusso;
            credenziali.transactionData = new ProxyWS.TransactionRequestType();
            credenziali.transactionData.sistema = idClient;
            credenziali.transactionData.codiceFiscaleRichiedente = cfRichiedente;
            credenziali.transactionData.codiceFiscaleIntestatario = cfIntestatario;
            credenziali.transactionData.idPod = string.Empty;
            credenziali.transactionData.idTransazione = string.Empty;
            credenziali.transactionData.cognomeIntestatario = cognomeIntestatario;
            credenziali.transactionData.nomeIntestatario = nomeIntestatario;

            try
            {
                // N.R. 09/2020 DA PROVARE IL NUOVO SERVIZIO
                resp = ws.verificaEmettibilita(credenziali, rifs);

                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "VEM";
                info.loggingAppCode = "CWA";
                info.flussoID = idFlusso;
                info.clientID = idClient;
                info.activeObjectCF = cfRichiedente;
                info.activeObjectIP = ipRichiedente;
                info.passiveObjectCF = cfIntestatario;
                info.freeTextDetails = "ID Public Certificato: " + idPublicCertificato;
                log.Info(info);
            }
            catch (SoapException ex)
            {
                ManagedException mex;
                if (ex.Code == SoapException.ClientFaultCode)
                {
                    mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness)"
                        + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                        ex.Detail.FirstChild.InnerText,
                        "Certi.WebApp.Business.BUSFlussoRichiesta",
                        "ExecuteVerifica",
                        "Verifica emettibilità certificato",
                        "ClientID: " + idClient,
                        "PassiveObjectCF: " + cfIntestatario,
                        ex.InnerException);
                }
                else
                {
                    mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness)"
                        + " di verifica dell'emettibilità del certificato con id: " + rif.idNomeCertificato,
                        ex.Message,
                        "Certi.WebApp.Business.BUSFlussoRichiesta",
                        "ExecuteVerifica",
                        "Verifica emettibilità certificato",
                        "ClientID: " + idClient,
                        "PassiveObjectCF: " + cfIntestatario,
                        ex.InnerException);
                }

                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

            return resp;
        }

        //metodo fatto per il motore di emissione batch
        public CertificatoType ExecuteVerificaAndRichiestaSingola(string idClient, string cfRichiedente, string ipRichiedente,
            string cfIntestatario, string idFlusso, string idPublicCertificato, string idUso, string motivoEsenzione)
        {
            ProxyWS.InfoCertificatoType[] resp = null;
            ProxyWS.InfoCertificatoType[] rifs = new ProxyWS.InfoCertificatoType[1];
            ProxyWS.InfoCertificatoType rif = new ProxyWS.InfoCertificatoType();
            rif.idNomeCertificato = idPublicCertificato;
            rif.IdUso = idUso;
            rif.idMotivoEsenzione = motivoEsenzione;
            rifs[0] = rif;

            ProxyWS.CredenzialiType credenziali = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.CredenzialiType();
            credenziali.idFlusso = idFlusso;
            credenziali.transactionData = new ProxyWS.TransactionRequestType();
            credenziali.transactionData.sistema = idClient;
            credenziali.transactionData.codiceFiscaleRichiedente = cfRichiedente;
            credenziali.transactionData.codiceFiscaleIntestatario = cfIntestatario;
            credenziali.transactionData.idPod = string.Empty;
            credenziali.transactionData.idTransazione = string.Empty;

            try
            {
                resp = ws.verificaEmettibilita(credenziali, rifs);
                if (resp[0].emettibile == true)
                {
                    CertificatoType[] cert = ws.richiestaCertificati(credenziali, resp);
                    if (cert[0] != null && cert[0].bin !=null && cert[0].bin.Length>0)
                    {
                        cert[0].emettibile = true;
                        cert[0].emettibileSpecified = true;
                        return cert[0];
                    }
                        
                    else
                    {
                        CertificatoType cc = new CertificatoType();
                        cc.emettibile = true;
                        cc.emettibileSpecified = true;
                        return cc;

                    }
                }
                else
                {
                    CertificatoType cc = new CertificatoType();
                    cc.emettibile = false;
                    cc.emettibileSpecified = true;
                    return cc;
                };
            }
            catch (SoapException ex)
            {
                ManagedException mex;
                if (ex.Code == SoapException.ClientFaultCode)
                {
                    mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness)"
                        + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                        ex.Detail.FirstChild.InnerText,
                        "Certi.WebApp.Business.BUSFlussoRichiesta",
                        "ExecuteVerifica",
                        "Verifica emettibilità certificato",
                        "ClientID: " + idClient,
                        "PassiveObjectCF: " + cfIntestatario,
                        ex.InnerException);
                }
                else
                {
                    mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness)"
                        + " di verifica dell'emettibilità del certificato con id: " + rif.idNomeCertificato,
                        ex.Message,
                        "Certi.WebApp.Business.BUSFlussoRichiesta",
                        "ExecuteVerifica",
                        "Verifica emettibilità certificato",
                        "ClientID: " + idClient,
                        "PassiveObjectCF: " + cfIntestatario,
                        ex.InnerException);
                }

                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

        }

        public bool ExecuteAfterPaymentRichiestaPA(string codFisRich, string CIU, string ip, string partevariabile)
        {
            bool resp = false;

            ProxyWS.CredenzialiType credenziali = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.CredenzialiType();
            ProxyWS.InfoCertificatoType[] input = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.InfoCertificatoType[1];
            ProfiloRichiesta pr = new ProfiloRichiesta();
            pr = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.LoadRichiestaByCIU(CIU);
            DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            credenziali.idFlusso = pr.Richieste[0].ID.ToString();
            credenziali.transactionData = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.TransactionRequestType();
            credenziali.transactionData.codiceFiscaleIntestatario = pr.Richieste[0].CODICE_FISCALE_INTESTATARIO;
            credenziali.transactionData.codiceFiscaleRichiedente = pr.Richieste[0].CODICE_FISCALE_RICHIEDENTE;
            credenziali.transactionData.idPod = pr.Richieste[0].UFFICIO_ID;
            credenziali.transactionData.idTransazione = pr.Richieste[0].TRANSAZIONE_ID;
            credenziali.transactionData.sistema = (CacheManager<ProfiloClient.ClientsDataTable>.get(
                CacheKeys.CLIENTS, VincoloType.NONE).Select(
                "ID = '" + pr.Richieste[0].CLIENT_ID + "'") as ProfiloClient.ClientsRow[])[0].Public_ID;

            input[0] = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.InfoCertificatoType();
            input[0].idMotivoEsenzione = pr.Certificati[0].ESENZIONE_ID;
            input[0].idNomeCertificato = (CacheManager<ProfiloCertificato.CertificatoDataTable>.get(
                CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE).Select("CERTID = '" +
                pr.Certificati[0].TIPO_CERTIFICATO_ID + "'") as ProfiloCertificato.CertificatoRow[])[0].PUBLIC_ID;
            input[0].IdUso = pr.Certificati[0].TIPO_USO_ID.ToString();             
            input[0].dicituraVariabile = partevariabile;
            ProxyWS.CertificatoType[] certificato;

            try
            {
                certificato = ws.richiestaCertificati(credenziali, input);
                if (certificato != null)
                    resp = true;

                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "REM";
                info.loggingAppCode = "CWA";
                info.flussoID = String.Empty;
                info.clientID = ConfigurationManager.AppSettings["ClientID"];
                info.activeObjectCF = codFisRich;
                info.activeObjectIP = ip;
                info.passiveObjectCF = String.Empty;
                info.freeTextDetails = "CIU Certificato:" + CIU;
                log.Info(info);
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) "
                    + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText)+ " " + ex.Message,
                    ex.Detail.FirstChild.InnerText,
                    "Certi.WebApp.Business.BUSFlussoRichiesta",
                    "ExecuteAfterPaymentRichiesta",
                    "Emissione certificato",
                    "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                    "ActiveObjectCF: " + ((ProfiloUtente)SessionManager<ProfiloUtente>.get(
                    SessionKeys.RICHIEDENTE_CERTIFICATI)).CodiceFiscale,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }
      

        /// <summary>
        /// Invio richiesta preparazione documento pdf al servizio web
        /// </summary>
        /// <param name="idFlusso"></param>
        /// <param name="idCertificato"></param>
        /// <param name="tipoUso"></param>
        /// <returns></returns>
        public bool ExecuteAfterPaymentRichiesta(string codFisRich, string CIU, string ip)
        {
            bool resp = false;

            ProxyWS.CredenzialiType credenziali = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.CredenzialiType();
            ProxyWS.InfoCertificatoType[] input = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.InfoCertificatoType[1];

            ProfiloRichiesta pr = new ProfiloRichiesta();
            log.Debug(CIU);            
            pr = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.LoadRichiestaByCIU(CIU);
            log.Debug(pr.Certificati[0].ID);   
            DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
            credenziali.idFlusso = pr.Richieste[0].ID.ToString();
            log.Debug(pr.Richieste);
            credenziali.transactionData = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.TransactionRequestType();
            credenziali.transactionData.codiceFiscaleIntestatario = pr.Richieste[0].CODICE_FISCALE_INTESTATARIO;
            credenziali.transactionData.codiceFiscaleRichiedente = pr.Richieste[0].CODICE_FISCALE_RICHIEDENTE;
            credenziali.transactionData.idPod = pr.Richieste[0].UFFICIO_ID;
            credenziali.transactionData.idTransazione = pr.Richieste[0].TRANSAZIONE_ID;
            credenziali.transactionData.sistema = (CacheManager<ProfiloClient.ClientsDataTable>.get(
                CacheKeys.CLIENTS, VincoloType.NONE).Select(
                "ID = '" + pr.Richieste[0].CLIENT_ID + "'") as ProfiloClient.ClientsRow[])[0].Public_ID;

            input[0] = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.InfoCertificatoType();
            input[0].idMotivoEsenzione = pr.Certificati[0].ESENZIONE_ID;
            input[0].idNomeCertificato = (CacheManager<ProfiloCertificato.CertificatoDataTable>.get(
                CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE).Select("CERTID = '" +
                pr.Certificati[0].TIPO_CERTIFICATO_ID + "'") as ProfiloCertificato.CertificatoRow[])[0].PUBLIC_ID;
            input[0].IdUso = pr.Certificati[0].TIPO_USO_ID.ToString();             
            ProxyWS.CertificatoType[] certificato;
            try
            {

                // N.R. MODIFICATO IL SERVIZIO DA TESTA 09/2020
                certificato = ws.richiestaCertificati(credenziali, input);
                if (certificato != null)
                    resp = true;

                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.logCode = "REM";
                info.loggingAppCode = "CWA";
                info.flussoID = String.Empty;
                info.clientID = ConfigurationManager.AppSettings["ClientID"];  
                info.activeObjectCF = codFisRich;
                info.activeObjectIP = ip;
                info.passiveObjectCF = String.Empty;
                info.freeTextDetails = "CIU Certificato:" + CIU;
                log.Info(info);
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) "
                    + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText)+ " dettagli: " + ex.Message ,
                    ex.Detail.FirstChild.InnerText,
                    "Certi.WebApp.Business.BUSFlussoRichiesta",
                    "ExecuteAfterPaymentRichiesta",
                    "Emissione certificato",
                    "ClientID: " + ConfigurationManager.AppSettings["ClientID"],
                    "ActiveObjectCF: " + ((ProfiloUtente)SessionManager<ProfiloUtente>.get(
                    SessionKeys.RICHIEDENTE_CERTIFICATI)).CodiceFiscale,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }

        /// <summary>
        /// Recupero documento pdf con ciu dato dallo storage
        /// passiamo l'indirizzo ip nel caso in cui l'utente arriva dall'area pubblica
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns>byte[][]</returns>
        public byte[][] GetPDF(string codFisRich, string ciu, string ip)
        {
            byte[][] resp = null;
            string client = ConfigurationManager.AppSettings["ClientID"];

            try
            {
                // N.R. 09/2020 MODIFICATO IL SERVIZIO DA TESTARE 
                resp = ws.recuperaDocumento(int.Parse(client), codFisRich, ciu);
                Com.Unisys.Logging.Certi.CertiLogInfo info = new Com.Unisys.Logging.Certi.CertiLogInfo();
                info.freeTextDetails = "CIU: " + ciu;
                info.logCode = "RIT";
                info.loggingAppCode = "CWA";
                info.flussoID = String.Empty;
                info.clientID = client;
                info.activeObjectCF = codFisRich;
                info.activeObjectIP = ip;
                info.passiveObjectCF = String.Empty;
                log.Info(info);
            }
            catch (SoapException ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) "
                    + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                   ex.Detail.FirstChild.InnerText,
                   "Certi.WebApp.Business.BUSFlussoRichiesta",
                   "GetPDF",
                   "Recupero certificato",
                   "ClientID: " + client,
                   "ActiveObjectCF: " + codFisRich,
                   ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return resp;
        }

        #endregion
           




        /// <summary>
        /// Recupero certificato con il CIU dato
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        public ProfiloDownload.CertificatiDataTable GetCertificatoByCIU(string ciu)
        {
            ProfiloDownload.CertificatiDataTable certs;
            BUSListe lst = new BUSListe();
            string client = ConfigurationManager.AppSettings["ClientID"];
            try
            {
                certs = lst.GetCertificatoDownload(ciu);
            }
            catch (ManagedException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_015",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "GetCertificatoByCIU",
                            "Caricamento dati certificato",
                            "ClientID: " + client,
                            "CIU: " + ciu,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return certs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idFlusso"></param>
        /// <returns></returns>
        public ProfiloRichiesta GetRichiestabyId(string idFlusso)
        {
            try
            {
                ProfiloRichiesta profilo = DataLayer.Dao.getDaoFactory(StoreType.ORACLE).Richiesta.LoadRichiesta(int.Parse(idFlusso));
                DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
                return profilo;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_032",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "GetRichiestabyId",
                            "Profilo richiesta da idFlusso",
                            string.Empty,
                            "Richiesta: " + idFlusso,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }

        /// <summary>
        /// Aggiornamento status e descrizione del soggetto che ha ritirato il certificato
        /// </summary>
        /// <param name="id"></param>
        /// <param name="descrizione"></param>
        /// <returns></returns>
        public int SignRitirato(string ciu, string descrizione)
        {
            string client = ConfigurationManager.AppSettings["ClientID"];
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).Richiesta.UpdateCertificatoByCIU(ciu, descrizione, (int)Status.C_RITIRATO,"");
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_014",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "SignRitirato",
                            "Aggiornamento certificato ritirato",
                            "ClientID: " + client,
                            "CIU: " + ciu,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

        }

        public bool ConfermaStampa(string idClient, string cfRichiedente, string cfIntestatario, string ipRichiedente, string idFlusso, string[] certificati)
        {
         
            bool vero = false;
            ProxyWS.CredenzialiType credenziali = new Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.CredenzialiType();
            credenziali.idFlusso = idFlusso;
            credenziali.transactionData = new ProxyWS.TransactionRequestType();
            credenziali.transactionData.sistema = idClient;
            credenziali.transactionData.codiceFiscaleRichiedente = cfRichiedente;
            credenziali.transactionData.codiceFiscaleIntestatario = cfIntestatario;
            credenziali.transactionData.idPod = string.Empty;
            credenziali.transactionData.idTransazione = string.Empty;
            try
            {
               vero =  ws.confermaStampa(credenziali,certificati);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_044",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "ConfermaStampa",
                            "Cambiamento status per conferma stampa",
                            string.Empty,
                            "Richiesta: " + credenziali.transactionData.codiceFiscaleIntestatario,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

            return vero;

        }
        


        /// <summary>
        /// Aggiornamento status certificato con richiesta pagamento
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        public int SignRichiestaPagamento(string ciu, string juv)
        {
            string client = ConfigurationManager.AppSettings["ClientID"];
            try
            {
                return DataLayer.Dao.getDaoFactory(StoreType.ORACLE).Richiesta.UpdateCertificatoByCIU(ciu, "", (int)Status.C_RICHIESTA_PAGAMENTO,juv);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) dettagli: " + ex.Message,
                            "ERR_033",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "SignRichiestaPagamento",
                            "Aggiornamento certificato con richiesta pagamento",
                            "ClientID: " + client,
                            "CIU: " + ciu,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

        }


        /// <summary>
        /// Chiamata al sevizio web per verificare lo stato di pagamento e vengono filtrati i certificati 
        /// già prodotti solo per il caso di pagina in cui devo visualizzare i certificati già prodotti
        /// </summary>
        /// <param name="codiceFiscale"></param>
        /// <returns></returns>
        public ProfiloDownload.CertificatiRow[] ControlPaymentRitiro(string codiceFiscale, string clientID, string ip)
        {
            ProfiloDownload.CertificatiDataTable certs;
            BUSListe lst = new BUSListe();
            try
            {
                int tipoRitiro = int.Parse(ConfigurationManager.AppSettings["TipoRitiro"]);
                certs = lst.GetElencoDownload(codiceFiscale, tipoRitiro, int.Parse(clientID));
                DataRow[] emptyRows = certs.Select("STATUS_ID=" + (int)Status.C_RICHIESTA_PAGAMENTO);
                if (emptyRows.Length > 0)
                {                   
                    try
                    {
                        for (int i = emptyRows.Length - 1; i >= 0; i--)
                        {
                            ProfiloDownload.CertificatiRow rr = (ProfiloDownload.CertificatiRow)emptyRows[i];
                            if (rr.STATUS_ID == (int)Status.C_RICHIESTA_PAGAMENTO)
                                certs.FindByCIU(rr.CIU).STATUS_ID = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                        }
                        DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamento(certs);
                        DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();                       
                       
                    }
                    catch (SoapException ex)
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli: " + ex.Message
                            + Utility.ExceptionUtils.GetMessageFromSOAPException(ex.Detail.FirstChild.InnerText),
                            ex.Detail.FirstChild.InnerText,
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "ControlPayment",
                            "Controllo pagamenti - Lista transazioni attive",
                            "ClientID: " + clientID,
                            "ActiveObjectCF: " + codiceFiscale,
                            ex.InnerException);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                        log.Error(error);
                        throw mex;
                    }                                       
                }
            }
            catch (ManagedException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli: " + ex.Message,
                            "ERR_022",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "ControlPayment",
                            "Controllo pagamenti",
                            "ClientID: " + clientID,
                            "ActiveObjectCF: " + codiceFiscale,

                             ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return (ProfiloDownload.CertificatiRow[])certs.Select("STATUS_ID <>" + (int)Status.C_RICHIESTA_PAGAMENTO_KO +
                " and STATUS_ID <> " + (int)Status.C_GENERAZIONE_PDF_OK + " and STATUS_ID <> " + (int)Status.C_RITIRATO,
                "T_DATA_EMISSIONE desc");
        }

        



        #region Region metodi privati

        /// <summary>
        /// Preparazione input verifica emettibilità in formato atteso dal WS
        /// </summary>
        /// <param name="certificati"></param>
        /// <returns></returns>
        private ProxyWS.InfoCertificatoType[] SetVerificaEmettibilitaInput(ProfiloRichiesta.CertificatiDataTable certificati)
        {
            int cont = certificati.Rows.Count;
            ProxyWS.InfoCertificatoType[] rifs = new ProxyWS.InfoCertificatoType[cont];
            for (int i = 0; i < cont; i++)
            {
                ProxyWS.InfoCertificatoType rif = new ProxyWS.InfoCertificatoType();
                rif.idNomeCertificato = certificati[i].T_PUBLIC_ID;
                rif.IdUso = certificati[i].TIPO_USO_ID.ToString();
                rif.idMotivoEsenzione = certificati[i].ESENZIONE_ID;
                rifs[i] = rif;
            }
            return rifs;

        }

        /*
        /// <summary>
        /// Preparazione input richiesta emissione in formato atteso da WS
        /// </summary>
        /// <param name="richiesta"></param>
        /// <param name="certificatiEmettibili"></param>
        /// <returns></returns>
        private ProxyWS.Certificato[] SetRichiestaCertificatiInput(ProfiloRichiesta richiesta,
            ProxyWS.ArrayOfVerificaEmettibilitaResponseCertificatoCertificato[] certificatiEmettibili)
        {
            int cont = certificatiEmettibili.Length;
            ProxyWS.Certificato[] certs = new ProxyWS.Certificato[cont];
            for (int i = 0; i < cont; i++)
            {
                ProxyWS.Certificato cert = new ProxyWS.Certificato();
                cert.idNomeCertificato = certificatiEmettibili[i].idNomeCertificato;
                string tipoUso = ((ProfiloRichiesta.CertificatiRow)(richiesta.Certificati.Select(
                    "T_PUBLIC_ID='"
                    + certificatiEmettibili[i].idNomeCertificato.ToString()
                    + "'")[0])).TIPO_USO_ID.ToString();
                cert.IdUso = (ProxyWS.IDUsoType)Enum.Parse(typeof(ProxyWS.IDUsoType), tipoUso);
                certs[i] = cert;
            }
            return certs;
        }
        */
        /*
        /// <summary>
        /// Preparazione form di chiamata post a Roma Pagamenti 
        /// </summary>
        /// <param name="cf"></param>
        /// <param name="backurl"></param>
        /// <param name="tipo"></param>
        /// <param name="pagamento"></param>
        /// <returns></returns>
        private String buildPayForm(string url, string cf, string backurl, string tipoArea, string pagamento)
        {
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<html><header></header><body>");
            strForm.Append("<form id=\"_xclick\" name=\"_xclick\" target=\"_self\" action=\"{0}\" method=\"post\">");
            strForm.Append("<input type=\"hidden\" name=\"cf\" value=\"{1}\">");
            strForm.Append("<input type=\"hidden\" name=\"backUrl\" value=\"{2}\">");
            strForm.Append("<input type=\"hidden\" name=\"tipo\" value=\"{3}\">");
            strForm.Append("<input type=\"hidden\" name=\"pagamento\" value=\"{4}\">");
            strForm.Append("</form>");
            strForm.Append("</body></html>");
            strForm.Append(_GetPayPalPostJS("_xclick"));
            return String.Format(strForm.ToString(), url, cf, backurl, tipoArea, pagamento);
        }

        /// <summary>
        /// Javascript di submit form pagamenti
        /// </summary>
        /// <param name="strFormId"></param>
        /// <returns></returns>
        private String _GetPayPalPostJS(String strFormId)
        {
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var ctlForm = document.forms.namedItem('{0}');");
            strScript.Append("ctlForm.submit();");
            strScript.Append("</script>");
            return String.Format(strScript.ToString(), strFormId);
        }

        /// <summary>
        /// Preparazione struttura dati pagamento
        /// </summary>
        /// <param name="idCertificato"></param>
        /// <param name="nomeCertificato"></param>
        /// <param name="dataEmissione"></param>
        /// <param name="nomeIntestatario"></param>
        /// <param name="cognomeIntestatario"></param>
        /// <param name="codiceFiscaleIntestatario"></param>
        /// <param name="inBollo"></param>
        /// <returns></returns>
        private string buildXmlRequestString(string idCertificato, string nomeCertificato, System.DateTime dataEmissione, string nomeIntestatario, string cognomeIntestatario, string codiceFiscaleIntestatario, bool inBollo)
        {
            string COD_BOLLO = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["CodiceBollo");
            string COD_DIRITTI;
            if (inBollo)
                COD_DIRITTI = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["CodiceDirittiBollo");
            else COD_DIRITTI = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["CodiceDirittiSemplice");

            StringBuilder s = new StringBuilder();
            s.Append("<?xml version='1.0' encoding='UTF-8'?>");
            s.Append("<requestPagamentoCertificato xmlns='http://roma.pol.eng.it/Pagamento_Certificati/http' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>");
            s.Append("<idCertificato>").Append(idCertificato).Append("</idCertificato>");
            s.Append("<nomeCertificato>").Append(nomeCertificato).Append("</nomeCertificato>");
            s.Append("<dataEmissione>").Append(dataEmissione.ToString("yyyy-MM-dd")).Append("</dataEmissione>");
            s.Append("<nomeIntestatario>").Append(nomeIntestatario).Append("</nomeIntestatario>");
            s.Append("<cognomeIntestatario>").Append(cognomeIntestatario).Append("</cognomeIntestatario>");
            s.Append("<codiceFiscaleIntestatario>").Append(codiceFiscaleIntestatario).Append("</codiceFiscaleIntestatario>");
            s.Append("<codiceDiritti>").Append(COD_DIRITTI).Append("</codiceDiritti>");
            if (inBollo) { s.Append("<codiceBollo>").Append(COD_BOLLO).Append("</codiceBollo>"); }
            else { s.Append("<codiceBollo />"); }
            s.Append("</requestPagamentoCertificato>");
            return s.ToString();
        }*/

        #endregion

    }
}
