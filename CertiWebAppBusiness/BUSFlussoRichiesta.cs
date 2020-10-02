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

        /*
        /// <summary>
        /// Preparazione form di richiesta pagamento
        /// </summary>
        /// <param name="tipoArea"></param>
        /// <param name="currentContext"></param>
        /// <param name="url"></param>
        /// <param name="backPage"></param>
        /// <param name="idFlusso"></param>
        /// <param name="CIU"></param>
        /// <param name="tipoCertificato"></param>
        /// <param name="dtEmissione"></param>
        /// <param name="codFisRic"></param>
        /// <param name="nomeInt"></param>
        /// <param name="cogInt"></param>
        /// <param name="codFisInt"></param>
        /// <param name="inBollo"></param>
        /// <returns></returns>
        public string PreparePayment(string tipoArea, System.Web.HttpContext currentContext, string url, string backPage, string idFlusso,
            string CIU, string tipoCertificato, DateTime dtEmissione, string codFisRic, string nomeInt, string cogInt, string codFisInt, bool inBollo)
        {
            string xmlRequest = buildXmlRequestString(CIU, tipoCertificato, dtEmissione, nomeInt, cogInt, codFisInt, inBollo);
            string form = buildPayForm(url, codFisRic, backPage, tipoArea, xmlRequest);
            return form;
        }
    */


/*
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
            XmlNode rp = null;
            bool wsResp = false;

            try
            {
                int tipoRitiro = int.Parse(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["TipoRitiro"));
                certs = lst.GetElencoDownload(codiceFiscale, tipoRitiro, int.Parse(clientID));
                DataRow[] emptyRows = certs.Select("STATUS_ID=" + (int)Status.C_RICHIESTA_PAGAMENTO);
                if (emptyRows.Length > 0)
                {
                    string[] listaCIU = new string[emptyRows.Length];
                    for (int i = 0; i < emptyRows.Length; i++)
                    {
                        listaCIU[i] = (emptyRows[i] as ProfiloDownload.CertificatiRow).CIU;
                    }

                    ProxyPagamentiWS.Pagamento_Certificati ck = new ProxyPagamentiWS.Pagamento_Certificati();
                    ck.Url = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["UrlControlloPagamenti");
                    try
                    {

                        rp = ck.ListaTransazioni(listaCIU);
                        wsResp = true;
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
                    finally
                    {
                        if (ck != null)
                            ck.Dispose();
                    }
                    if (wsResp)
                    {
                        foreach (System.Xml.XmlNode p in rp)
                        {
                            string es = p.SelectSingleNode("//esito").InnerText;
                            string ci = p.SelectSingleNode("//idCertificato").InnerText;
                            switch (es)
                            {
                                case "OK":
                                    certs.FindByCIU(ci).CODICE_PAGAMENTO = p.SelectSingleNode("//datiTransazione/datiPagamento/idEmissione").InnerText;
                                    certs.FindByCIU(ci).XML_PAGAMENTO = p.SelectSingleNode("//datiTransazione").OuterXml;
                                    certs.FindByCIU(ci).STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_OK;
                                    break;
                                case "KO":
                                    certs.FindByCIU(ci).STATUS_ID = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                                    break;
                            }

                        }

                        for (int i = emptyRows.Length - 1; i >= 0; i--)
                        {
                            ProfiloDownload.CertificatiRow rr = (ProfiloDownload.CertificatiRow)emptyRows[i];
                            if (rr.STATUS_ID == (int)Status.C_RICHIESTA_PAGAMENTO)
                                certs.FindByCIU(rr.CIU).STATUS_ID = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                        }
                        DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamento(certs);
                        DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();

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
                " and STATUS_ID <> " + (int)Status.C_GENERAZIONE_PDF_OK+ " and STATUS_ID <> "+ (int)Status.C_RITIRATO,
                "T_DATA_EMISSIONE desc");
        }*/

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

        /*
        public BridgeProxy.datibridgeTYPE CreaDatiBridge(string enteCred,string tipoArea, string idSistemaT, string bckUrl)
        {
              BridgeProxy.datibridgeTYPE datiBridge = new BridgeProxy.datibridgeTYPE();
            datiBridge.entecreditore = enteCred;
            datiBridge.idsistemasorgente = tipoArea;
            datiBridge.idsistematarget = idSistemaT;
            datiBridge.backurl = bckUrl;
            return datiBridge;
        }*/
        /*
        public BridgeProxy.carrellopayloadTYPE CreaCarrello(int idx, string tipoDebito, Decimal importoTot, string macroStrut, string areaTema, string CIU, DateTime dtEmissione, string codDiritti, string tipoCerti,string tipoGruppo,string tipoBollo, bool inBollo, decimal totaleDebiti, string cognome, string nome , string codFisc)
        {

            BridgeProxy.carrellopayloadTYPE carrello = new BridgeProxy.carrellopayloadTYPE();
                carrello.debitidapagare = new BridgeProxy.debitodapagareTYPE[idx];

                for (int i = 0; i < idx; i++) 
                {
                    carrello.debitidapagare[i] = CreaDebitoDaPagare(tipoDebito, importoTot, macroStrut, areaTema,inBollo,codFisc,nome,cognome);
                    carrello.debitidapagare[i].coordinatedebito.altridati = CreaAltriDati(CIU,dtEmissione,codDiritti,tipoCerti,tipoGruppo,tipoBollo,inBollo);
                }
                
                return carrello;

        }
        
        private BridgeProxy.debitodapagareTYPE CreaDebitoDaPagare(string tipoDebito, Decimal importoTot, string macroStrut, string areaTema, bool idxden, string codFisc, string nome, string cognome)
        {
            BridgeProxy.debitodapagareTYPE debitoDaPagare = new Com.Unisys.CdR.Certi.WebApp.Business.BridgeProxy.debitodapagareTYPE();
            debitoDaPagare.coordinatedebito = new BridgeProxy.coorddebitoTYPE();
            debitoDaPagare.coordinatedebito.tipodebito = tipoDebito;
            debitoDaPagare.coordinatedebito.importototale = importoTot;
            debitoDaPagare.coordinatedebito.importototaleSpecified = true;
            debitoDaPagare.coordinatedebito.macrostrutturad = macroStrut;
            debitoDaPagare.coordinatedebito.areatematica = areaTema;
            debitoDaPagare.coordinatedebito.contribuente = CreaContribuente(idxden,codFisc,nome,cognome);
           
            return debitoDaPagare;
        }
        
        public BridgeProxy.contribuenteTYPE CreaContribuente(bool  idxden, string codFisc, string nome, string cognome)
        {

            int idx = 0;
            if (idxden)
            {
                idx = 3;
            }
            else
                idx = 2;

                BridgeProxy.contribuenteTYPE contribuente = new BridgeProxy.contribuenteTYPE();
                contribuente.tipo = BridgeProxy.naturagiuridicaTYPE.F;
                contribuente.codicefiscale = codFisc;
                contribuente.altridati = new BridgeProxy.namevaluepairTYPE[idx];
                contribuente.altridati[0] = new BridgeProxy.namevaluepairTYPE();
                contribuente.altridati[0].nome = "UC_NOME";
                contribuente.altridati[0].valore = new string[1];
                contribuente.altridati[0].valore[0] = nome;
                contribuente.altridati[1] = new BridgeProxy.namevaluepairTYPE();
                contribuente.altridati[1].nome = "UC_COGNOME";
                contribuente.altridati[1].valore = new string[1];
                contribuente.altridati[1].valore[0] = cognome;
                if (idxden)
                {
                    contribuente.altridati[2] = new BridgeProxy.namevaluepairTYPE();
                    contribuente.altridati[2].nome = "UC_DEN";
                }
                return contribuente;
        }

        private BridgeProxy.namevaluepairTYPE[] CreaAltriDati(string CIU, DateTime dtEmissione,string codDiritti,string tipoCerti,string tipoGruppo,string tipoBollo, bool inBollo) {
            BridgeProxy.namevaluepairTYPE[] altridati;
            if (inBollo)
            {
                altridati = new BridgeProxy.namevaluepairTYPE[6];
                altridati[0] = new BridgeProxy.namevaluepairTYPE();
                altridati[0].nome = "COD_CERTIFICATO";
                altridati[0].valore = new string[1];
                altridati[0].valore[0] = CIU;
                altridati[1] = new BridgeProxy.namevaluepairTYPE();
                altridati[1].nome = "DATA_EMISSIONE";
                DateTime[] dt = new DateTime[1];
                dt[0] = dtEmissione;
                altridati[1].valore = dt.Cast<object>().ToArray();
                altridati[2] = new BridgeProxy.namevaluepairTYPE();
                altridati[2].nome = "COD_DIRITTI";
                altridati[2].valore = new string[1];
                altridati[2].valore[0] = codDiritti;
                altridati[3] = new BridgeProxy.namevaluepairTYPE();
                altridati[3].nome = "TIPO_GRUPPO";
                altridati[3].valore = new string[1];
                altridati[3].valore[0] = tipoGruppo;
                altridati[4] = new BridgeProxy.namevaluepairTYPE();
                altridati[4].nome = "TIPO_BOLLO_APPLICATO";
                altridati[4].valore = new string[1];
                altridati[4].valore[0] =tipoBollo;
                altridati[5] = new BridgeProxy.namevaluepairTYPE();
                altridati[5].nome = "AD_NOME_CERTIFICATO";
                altridati[5].valore = new string[1];
                altridati[5].valore[0] = tipoCerti;
            }
            else
            {
                altridati = new BridgeProxy.namevaluepairTYPE[4];
                altridati[0] = new BridgeProxy.namevaluepairTYPE();
                altridati[0].nome = "COD_CERTIFICATO";
                altridati[0].valore = new string[1];
                altridati[0].valore[0] = CIU;
                altridati[1] = new BridgeProxy.namevaluepairTYPE();
                altridati[1].nome = "DATA_EMISSIONE";
                DateTime[] dt = new DateTime[1];
                dt[0] = dtEmissione;
                altridati[1].valore = dt.Cast<object>().ToArray();
                altridati[2] = new BridgeProxy.namevaluepairTYPE();
                altridati[2].nome = "COD_DIRITTI";
                altridati[2].valore = new string[1];
                altridati[2].valore[0] = codDiritti;
                altridati[3] = new BridgeProxy.namevaluepairTYPE();
                altridati[3].nome = "AD_NOME_CERTIFICATO";
                altridati[3].valore = new string[1];
                altridati[3].valore[0] = tipoCerti;
            }
            return altridati;
         }

        //aggiunta alessio 19/02/2014
        public bool PreparaPagamento(string tipoArea, System.Web.HttpContext currentContext, string url, string backPage, string idFlusso,
            string CIU, string tipoCertificato, DateTime dtEmissione, string codFisRic, string nomeInt, string cogInt, string codFisInt, bool inBollo, out string mess)
               
        {

            string tipoDebito = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["tipoDebito");
            decimal importoTot;
            decimal totaleDebiti;
            string macroStrut = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["macroStruttura");
            string areaT;
            string codDiritti;
            string tipoGruppo = string.Empty;
            string tipoBollo=string.Empty;


                BridgeProxy.FedFisWEBBridge fed = new BridgeProxy.FedFisWEBBridge();
                fed.Url = url;
                BridgeProxy.requestpreparabridgeTYPE req = new BridgeProxy.requestpreparabridgeTYPE();
                req.timestamp = DateTime.Now;
                BridgeProxy.datibridgeTYPE datiBridge = CreaDatiBridge(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["enteCreditore"), tipoArea, Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["SistemaTarget"), backPage);
                datiBridge.utente = new BridgeProxy.contribuenteTYPE();
                datiBridge.utente.tipo = BridgeProxy.naturagiuridicaTYPE.F;
                datiBridge.utente.codicefiscale = codFisRic;
                datiBridge.payload = new BridgeProxy.datibridgeTYPEPayload();

                if (inBollo)
                {
                    importoTot = Convert.ToDecimal(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["importoTotaleB"), CultureInfo.InvariantCulture);
                    areaT = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["areaTematicaBollo");
                    codDiritti = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["CodiceDirittiBollo");
                    tipoGruppo = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["tipoGruppo");
                    tipoBollo = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["CodiceBollo");
                    totaleDebiti=Convert.ToDecimal(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["totaleDebitiB"), CultureInfo.InvariantCulture);

                }
                else
                {
                    importoTot = Convert.ToDecimal(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["importoTotaleD"), CultureInfo.InvariantCulture);
                    areaT = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["areaTematicaDiritti");
                    codDiritti = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["CodiceDirittiSemplice");
                    totaleDebiti = Convert.ToDecimal(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["totaleDebitiD"), CultureInfo.InvariantCulture);

                }

                //!!!!!VERIFICARE!!!!! passo come parametro nome, cogn e codice fiscale del richiedente.
                BridgeProxy.carrellopayloadTYPE carrello =CreaCarrello(1,tipoDebito,importoTot,macroStrut,areaT,CIU,dtEmissione,codDiritti,tipoCertificato,tipoGruppo,tipoBollo,inBollo,totaleDebiti,cogInt,nomeInt,codFisInt);

                carrello.totaledebiti = totaleDebiti;
                datiBridge.payload.Item = carrello;
            
            req.datibridge = datiBridge;
            log.Debug("Sto chiamando prepare bridge_" + req.datibridge.utente.codicefiscale);
            if (!(string.IsNullOrEmpty(CIU)))
            { log.Debug("Ecco il ciu" + CIU); }    
            BridgeProxy.responsepreparabridgeTYPE resp = fed.preparaBridge(req);
            log.Debug("Ha risposto prepare bridge_" + req.datibridge.utente.codicefiscale);
            if (resp.esito)
            {
                log.Debug("ho il ticket_" + req.datibridge.utente);
                mess = resp.ticket;
            }
            else
                mess = resp.messaggio;
            return resp.esito;
        }*/

        //nuovo metodo di controllo dei pagamenti per il nuovo servizio Pago
        /*
        public ProfiloDownload.CertificatiRow[] ControllaPagamento(string codiceFiscale, string clientID, string ip)
        {
            ProfiloDownload.CertificatiDataTable certs;
            BUSListe lst = new BUSListe();

            try
            {
                int tipoRitiro = int.Parse(Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["TipoRitiro"));

                certs = lst.GetElencoDownload(codiceFiscale, tipoRitiro, int.Parse(clientID));
                DataRow[] emptyRows = certs.Select("STATUS_ID=" + (int)Status.C_RICHIESTA_PAGAMENTO);
                if (emptyRows.Length > 0)
                {
                    string[] listaCIU = new string[emptyRows.Length];
                    for (int i = 0; i < emptyRows.Length; i++)
                    {
                        listaCIU[i] = (emptyRows[i] as ProfiloDownload.CertificatiRow).CIU;
                    }
                    //modificare  qui
                    MWxSAProxy.FedFisMWxSA fedFis = new MWxSAProxy.FedFisMWxSA();
                    fedFis.Url = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["UrlControlloPagamenti");
                    MWxSAProxy.requestnotificapagamentiTYPE notificaPaga = new MWxSAProxy.requestnotificapagamentiTYPE();
                    MWxSAProxy.notificarichiestaTYPE notificaRic = new MWxSAProxy.notificarichiestaTYPE();
                    MWxSAProxy.responsenotificapagamentiTYPE respPago = new MWxSAProxy.responsenotificapagamentiTYPE();
                    notificaPaga.timestamp = DateTime.Now;
                    notificaPaga.enti = new MWxSAProxy.notificherichiesteenteTYPE[1];
                    notificaPaga.enti[0] = new MWxSAProxy.notificherichiesteenteTYPE();
                    notificaPaga.enti[0].entecreditore = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["enteCreditore");
                    notificaPaga.enti[0].notificherichieste = new MWxSAProxy.notificarichiestaTYPE[emptyRows.Length];
                    for (int i = 0; i < emptyRows.Length; i++) {
                        notificaPaga.enti[0].notificherichieste[i] = new MWxSAProxy.notificarichiestaTYPE();
                        notificaPaga.enti[0].notificherichieste[i].idsistemaoggettopagamento = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["idsistemaoggettopagamento");
                        notificaPaga.enti[0].notificherichieste[i].idlocaleoggettopagamento = listaCIU[i];
                    }

                    log.Debug("prima di controllo pagamenti");
                    log.Debug(fedFis.Url);
                    respPago = fedFis.chiediPagamenti(notificaPaga);
                    log.Debug("dopo controllo pagamenti");
                    if (respPago.esito)
                    {

                        MWxSAProxy.notificheenteTYPE[] arrayEnti = respPago.enti;
                        if (arrayEnti != null)
                        {
                            MWxSAProxy.notificaTYPE[] notifiche = arrayEnti[0].notifiche;

                            foreach (MWxSAProxy.notificaTYPE notifica in notifiche)
                            {
                                string ci = notifica.debitipagati[0].idoggettopagamento.idlocaleoggettopagamento;
                                certs.FindByCIU(ci).CODICE_PAGAMENTO = notifica.datipagamento.idricevutapagamento;
                                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(MWxSAProxy.notificaTYPE));
                                System.IO.Stream str = new System.IO.MemoryStream();
                                XmlTextWriter wr = new XmlTextWriter(str, new System.Text.UTF8Encoding(false));
                                ser.Serialize(wr, notifica);
                                str.Seek(0, System.IO.SeekOrigin.Begin);
                                byte[] buffer = new byte[str.Length];
                                str.Read(buffer, 0, buffer.Length);
                                string respNotifica = new UTF8Encoding(false).GetString(buffer);
                                certs.FindByCIU(ci).XML_PAGAMENTO = respNotifica;
                                certs.FindByCIU(ci).STATUS_ID = (int)Status.C_VERIFICA_EMETTIBILITA_OK;
                            }
                        }

                        string aggiornaPago = Com.Unisys.CdR.Certi.Utils.ConfigurationManager.AppSettings["aggiornaPago");

                        switch (aggiornaPago)
                        {
                            case "none":

                                for (int i = emptyRows.Length - 1; i >= 0; i--)
                                {
                                    ProfiloDownload.CertificatiRow rr = (ProfiloDownload.CertificatiRow)emptyRows[i];
                                    if (rr.STATUS_ID == (int)Status.C_RICHIESTA_PAGAMENTO)

                                        certs.FindByCIU(rr.CIU).STATUS_ID = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                                }
                                Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamento(certs);
                                Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();

                                break;
                            case "all":

                                break;

                            default:

                                for (int i = emptyRows.Length - 1; i >= 0; i--)
                                {
                                    ProfiloDownload.CertificatiRow rr = (ProfiloDownload.CertificatiRow)emptyRows[i];
                                    TimeSpan diffH;
                                    diffH = DateTime.Now.Subtract(rr.T_DATA_EMISSIONE);

                                    //&&  diffH == convert.toint16(aggiornaPago)
                                    if (rr.STATUS_ID == (int)Status.C_RICHIESTA_PAGAMENTO && diffH.TotalMinutes >= Convert.ToDouble(aggiornaPago))

                                        certs.FindByCIU(rr.CIU).STATUS_ID = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                                }
                                Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamento(certs);
                                Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Dispose();
                                break;

                        }

                    }
                }
            }
            catch (ManagedException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + ex.Message,
                            "ERR_222",
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
            return (ProfiloDownload.CertificatiRow[])certs.Select("STATUS_ID <>" + (int)Status.C_RICHIESTA_PAGAMENTO_KO,
                "T_DATA_EMISSIONE desc");
        }
        */
        



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
