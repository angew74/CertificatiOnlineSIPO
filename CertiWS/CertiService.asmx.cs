using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using Com.Unisys.CdR.Certi.WS.Business;
using Com.Unisys.CdR.Certi.WS.Dati;
using Com.Unisys.Logging;
using System.Web.Services.Description;
using log4net;


namespace Com.Unisys.CdR.Certi.WS
{
    /// <summary>
    /// Summary description for CertiService
    /// </summary>
    [WebService(Namespace = "http://www.comune.roma.it/certificati")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1), SoapDocumentService(SoapBindingUse.Literal, SoapParameterStyle.Wrapped, RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    public class CertiService : System.Web.Services.WebService, Com.Unisys.Certi.WS.ICertificatiBinding
    {
        private static readonly ILog log = LogManager.GetLogger("CertiService");
        private BUSManager bman = new BUSManager();

        #region ICertificatiBinding Members

        /// <summary>
        /// Verifica la presenza di un codice fiscale dato in DB Anagrafe
        /// </summary>
        /// <param name="idClient">Identificativo dell'applicazione client chiamante<see cref=System.Int32/></param>
        /// <param name="codiceFiscale"><see cref=System.String/></param>
        /// <returns>Codice individuale del cittadino richiesto<see cref=System.String/></returns>
        public string ricercaPersona(int idClient, string codiceFiscale)
        {
            try
            {
                return bman.VerificaCodiceFiscale(idClient, codiceFiscale);
            }
            catch (ManagedException ex)
            {
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
            }
            catch (Exception ex)
            {
                throw new SoapException("ERR_008",
                    ExceptionUty.SetSOAPFault("ERR_008"),
                    null,
                    ex);
            }
            finally
            {
                this.Dispose();
            }
        }


        /// <summary>
        /// Ricerca dei componenti della famiglia del codice fiscale dato in input da dismettere
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="codiceFiscaleRichiedente"></param>
        /// <returns>Elenco componenti della famiglia</returns>
        public ComponenteFamigliaType[] ricercaComponentiFamiglia(int idClient, string codiceFiscaleRichiedente)
        {
            try
            {
                return bman.InviaRicercaComponentiFamiglia(idClient, codiceFiscaleRichiedente);
            }
            catch (ManagedException ex)
            {
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
            }
            catch (Exception ex)
            {
                throw new SoapException("ERR_009",
                    ExceptionUty.SetSOAPFault("ERR_009"),
                    null,
                    ex);
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Recupero di un documento emesso
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        public byte[][] recuperaDocumento(int idClient, string codiceFiscaleRichiedente, string ciu)
        {
            try
            {
                return bman.FindPdf(idClient, codiceFiscaleRichiedente, ciu);
            }
            catch (ManagedException ex)
            {
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
            }
            catch (Exception ex)
            {
                throw new SoapException("ERR_003",
                    ExceptionUty.SetSOAPFault("ERR_003"),
                    null,
                    ex);
            }
            finally
            {
                this.Dispose();
            }
        }


        /// <summary>
        /// Verifica la presenza di un cittadino, intestatario dei certificati, nell'archivio dell'Anagrafe CdR
        /// </summary>
        /// <param name="transactionRequest">Credenziali del client chiamante<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.TransactionRequestType/></param>
        /// <param name="credenziali">IdFlusso (CdR) + credenziali del client chiamante<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.CredenzialiType/></param>
        /// <returns>Intestatario trovato o no<see cref="System.boolean"/></returns>
        public CredenzialiType richiestaCredenziali(TransactionRequestType transactionRequest, out bool intestatarioTrovato)
        {
            try
            {
                CredenzialiType credenziali;
                intestatarioTrovato = bman.InviaRichiestaCredenziali(transactionRequest, out credenziali);
                log.Debug("sono nel ritorno del web method" + credenziali.idFlusso);
                return credenziali;
            }
            catch (ManagedException ex)
            {
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
            }
            catch (Exception ex)
            {
                throw new SoapException("ERR_004",
                    ExceptionUty.SetSOAPFault(string.Concat("ERR_004",ex.Message)),
                    null,
                    ExceptionUty.SetDetails(string.Concat("ERR_004", ex.Message)));
            }
            finally
            {
                this.Dispose();
            }
        }
     

        /// <summary>
        /// Verifica l'emettibilità dei certificati richiesti in base alle credenziali passate
        /// </summary>
        /// <param name="credenziali">Credenziali identificativi del flusso richiesta<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.CredenzialiType/></param>
        /// <param name="certificati">Array dei certificati richiesti<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.InfoCertificatoType/></param>
        /// <returns>Elenco dei certificati emettibili<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.InfoCertificatoType/><remarks>la risposta è un sottoinsieme dell'elenco di input</remarks></returns>
        public InfoCertificatoType[] verificaEmettibilita(CredenzialiType credenziali, InfoCertificatoType[] certificati)
        {
            try
            {
                return bman.InviaVerificaEmettibilitaSIPO(credenziali, certificati);
            }
            catch (ManagedException ex)
            {
                log.Debug(ex);
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
                
            }
            catch (Exception exm)
            {
                log.Debug(exm);
                throw new SoapException("ERR_005",
                    ExceptionUty.SetSOAPFault("ERR_005"),
                    null,
                    ExceptionUty.SetDetails("ERR_005"));
            }
            finally
            {
                this.Dispose();
            }
        }

        // N.R. MODIFICATO 09/2020 DA TESTARE

        /// <summary>
        /// Richiesta di emissione certificati
        /// </summary>
        /// <param name="credenziali">Credenziali identificativi del flusso richiesta<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.CredenzialiType/></param>
        /// <param name="certificati">Array dei certificati richiesti<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.InfoCertificatoType/></param>
        /// <returns>Elenco dei certificati emessi<see ref=Com.Unisys.CdR.Certi.WSOmnia.Dati.CertificatoType/>
        /// <remarks>Nel caso in cui un certificato non sia più emettibile rispetto alla verifica precedente, 
        /// non si restituisce nessun certificato, ma si genera un eccezione applicativa</remarks>
        /// </returns>
        public CertificatoType[] richiestaCertificati(CredenzialiType credenziali, InfoCertificatoType[] certificati)
        {
            try
            {
                log.Debug("arrivato in richiestaCertificati");
                return bman.InviaRichiestaCertificati(credenziali, certificati);
            }
            catch (ManagedException ex)
            {
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
            }
            catch (Exception ex)
            {
                log.Error("errore metodo richiestaCertificati dettagli: " + ex.Message);
                throw new SoapException("ERR_006",
                    ExceptionUty.SetSOAPFault("ERR_006"),
                    null,
                    ex);
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Conferma di stampma dei certificati emessi
        /// </summary>
        /// <param name="credenziali">Credenziali identificativi del flusso richiesta<see cref=Com.Unisys.CdR.Certi.WSOmnia.Dati.CredenzialiType/></param>
        /// <param name="certificati">Array dei certificati stampati<see cref=System.String/></param>
        /// <returns>Conferma ricezione messaggio di stampa<see cref=System.Boolean/></returns>
        public bool confermaStampa(CredenzialiType credenziali, string[] certificati)
        {
            try
            {
                return bman.InviaConfermaStampa(credenziali, certificati);
            }
            catch (ManagedException ex)
            {
                throw new SoapException(ex.CodiceEccezione,
                    ExceptionUty.SetSOAPFault(ex.CodiceEccezione),
                    null,
                    ExceptionUty.SetDetails(ex.CodiceEccezione));
            }
            catch (Exception ex)
            {
                throw new SoapException("ERR_007",
                    ExceptionUty.SetSOAPFault("ERR_007"),
                    null,
                    ex);
            }
            finally
            {
                this.Dispose();
            }
        }

        #endregion
    }
}
