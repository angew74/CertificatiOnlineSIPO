using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.Objects.SIPO;
using Com.Unisys.CdR.Certi.WS.Business.sipo;
using Com.Unisys.CdR.Certi.WS.Dati;
using Com.Unisys.Logging;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business.bus
{
    public class BUSPdfGeneratorSIPO
    {

        static readonly ILog log = LogManager.GetLogger(typeof(BUSPdfGeneratorSIPO));
        public bool MakePdf(string idFlusso, ProfiloRichiesta.CertificatiRow certificatiRow, ProfiloRichiesta.RichiesteRow richiesteRow, InfoCertificatoType cert)
        {
            ResponseRichiestaToken r = new ResponseRichiestaToken();            
            string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];      
            try
            {
                string AuthRequest = "BASIC " + SerializationHelper.ToBase64Encode(utenzadominio);
                string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
                SIPORequestJson sipoRequestToken = new SIPORequestJson(RichiestaToken, "RichiestaToken", AuthRequest, "", idFlusso);
                r = sipoRequestToken.CallingRichiestaToken(idFlusso);
                string accesstoken = r.access_token;
                RecuperaCertificato recuperaCertificato = new RecuperaCertificato();               
                recuperaCertificato.idIntestatario = richiesteRow.COD_IND_INTESTATARIO;
                if (richiesteRow.CODICE_FISCALE_INTESTATARIO != richiesteRow.CODICE_FISCALE_RICHIEDENTE)
                {
                    recuperaCertificato.tipoRichiedente = "1";
                    recuperaCertificato.codiceFiscaleRichiedente = richiesteRow.CODICE_FISCALE_RICHIEDENTE;
                }
                else { recuperaCertificato.tipoRichiedente = "2"; }
                if (cert.idNomeCertificato == "1" || cert.idNomeCertificato == "2" || cert.idNomeCertificato == "3")
                { recuperaCertificato.flgAnagSC = "S"; }
                else { recuperaCertificato.flgAnagSC = "A"; }
                recuperaCertificato.flgAnteprima = "N";
                if (cert.IdUso == "2")
                { recuperaCertificato.flgSempliceBollata = "B"; }
                else { recuperaCertificato.flgSempliceBollata = "S"; }
                recuperaCertificato.idCertificatiAnpr = SIPOHelper.GetCertificatoSIPO(cert.idNomeCertificato);
                if (cert.IdUso == "3" || cert.IdUso == "1")
                { recuperaCertificato.idEsenzioneAnpr = SIPOHelper.GetEsenzioneSIPO(cert.idMotivoEsenzione); }
                recuperaCertificato.hostname = ConfigurationManager.AppSettings["hostname"];
                recuperaCertificato.cfUser = ConfigurationManager.AppSettings["cfuser"];
                recuperaCertificato.idCertificato = certificatiRow.CIU.ToString();
                string ricerca = JsonConvert.SerializeObject(recuperaCertificato);
                string AccessTokenRicerca = "bearer " + accesstoken;
                ResponseRicercaPosAnag responseRicercaPosAnag = new ResponseRicercaPosAnag();
                SIPORequestJson sipoRequest = new SIPORequestJson(ConfigurationManager.AppSettings["ServiceRecuperaCertificato"], "RecuperaCertificato", AccessTokenRicerca, ricerca, idFlusso);
                var response = sipoRequest.CallingRecuperaCertificato(idFlusso);
                certificatiRow.T_DOCUMENTO = response.certificato;
            }
            catch(Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWSBusiness)"
                  + " di generazione certificato con ID = " +certificatiRow.ID + " " + ex.Message,
                  "ERR_149",
                  "Certi.WS.Business.BUSManager",
                  "MakePdf",
                  "Generazione PDF",
                  string.Empty,
                  "CIU: " + certificatiRow.CIU,
                   ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return true;
        }
    }
}
