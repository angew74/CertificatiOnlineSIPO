using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.DataObjects.CertificatiAnagrafici;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.WS.Dati;
using log4net;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Configuration;
using Com.Unisys.CdR.Certi.WS.Business.sipo;
using Com.Unisys.CdR.Certi.Objects.SIPO;
using Newtonsoft.Json;
using Com.Unisys.CdR.Certi.WS.Business.bus;
using System.IO;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSVeriEmettibilita
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(BUSVeriEmettibilita));
        VeriEmettibilitaCertificatiResponse _veriResponse = new VeriEmettibilitaCertificatiResponse();

        #region Class Properties


        public VeriEmettibilitaCertificatiResponse VeriResponse
        {
            get { return _veriResponse; }
            set { _veriResponse = value; }
        }

        #endregion

        /// <summary>
        /// Costruttore
        /// </summary>
        public BUSVeriEmettibilita()
        {
        }



        public ComponenteFamigliaType[] FindComponenti(string codFiscale)
        {
            ComponenteFamigliaType[] componenteFamiglias = null;
            ResponseRichiestaToken r = new ResponseRichiestaToken();
            string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];
            string AuthRequest = "BASIC " + SerializationHelper.ToBase64Encode(utenzadominio);
            string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
            SIPORequestJson sipoRequestToken = new SIPORequestJson(RichiestaToken, "RichiestaToken", AuthRequest, "", codFiscale);
            r = sipoRequestToken.CallingRichiestaToken(codFiscale);
            string accesstoken = r.access_token;
            RicercaPosAnag ricercaPosAnag = new RicercaPosAnag();
            ricercaPosAnag.codiceFiscale = codFiscale;
            ricercaPosAnag.hostname = ConfigurationManager.AppSettings["hostname"];
            ricercaPosAnag.cfUser = ConfigurationManager.AppSettings["cfuser"];
            ricercaPosAnag.flagOrdineProfessionale = "N";
            string ricerca = JsonConvert.SerializeObject(ricercaPosAnag);
            string AccessTokenRicerca = "bearer " + accesstoken;
            string serviceRicerca = ConfigurationManager.AppSettings["ServiceRicercaPosAnag"];
            SIPORequestJson sipoRequest = new SIPORequestJson(serviceRicerca, "RicercaPosAnag", AccessTokenRicerca, ricerca, codFiscale);
            List<MyArray> myArrays = sipoRequest.CallingRicercaPosizione(codFiscale);
            _log.Debug("sono uscito da ricerca posizione ");
            List<ComponenteFamigliaType> l = new List<ComponenteFamigliaType>();         
            if (myArrays != null && myArrays[0].confStatusSoggetto != null && myArrays[0].confStatusSoggetto.idStatusSoggetto == 0)
            {
                _log.Debug("myarray non nulla");
                for (int i = 0; i < myArrays.Count; i++)
                {
                    _log.Debug("dentro ciclo for");
                    if (myArrays[i].famigliaConvivenza != null)
                    {
                        _log.Debug("famiglia diverso da null");
                        ComponenteFamigliaType componenteFamigliaType = new ComponenteFamigliaType();
                        componenteFamigliaType.codiceFiscale = myArrays != null ? myArrays[i].codiceFiscale : "";
                        componenteFamigliaType.codiceIndividuale =myArrays != null ? myArrays[i].idSoggetto.ToString() : "";
                        componenteFamigliaType.nome = myArrays != null ?  myArrays[i].nome : "";
                        componenteFamigliaType.cognome = myArrays != null ? myArrays[i].cognome : "";
                        componenteFamigliaType.rapportoParentela = myArrays != null && myArrays[i].confCodiceLegameFamigliaConv != null ? myArrays[i].confCodiceLegameFamigliaConv.descrizione : "";
                        _log.Debug("aggiungo persona " + i);
                        l.Add(componenteFamigliaType);
                    }
                }
            }
            if (l.Count > 0)
            {
                componenteFamiglias = l.ToArray();
            }
            return componenteFamiglias;
        }

        public VerificaEmettibilita VerificaEmettibilitaSIPO(string idIntestatario, ProfiloRichiesta.CertificatiRow[] certificatiRichiesti,TransactionRequestType transactionRequestType,string certificatoId,string tipoUsoId, string esenzioneId)
        {
            ResponseRichiestaToken r = new ResponseRichiestaToken();
            VerificaEmettibilita verifica = new VerificaEmettibilita();
            string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];
            string AuthRequest = "BASIC " + SerializationHelper.ToBase64Encode(utenzadominio);
            string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
            SIPORequestJson sipoRequestToken = new SIPORequestJson(RichiestaToken, "RichiestaToken", AuthRequest, "", certificatoId);
            r = sipoRequestToken.CallingRichiestaToken(idIntestatario);
            string accesstoken = r.access_token;
            RecuperaCertificato recuperaCertificato = new RecuperaCertificato();
            recuperaCertificato.idIntestatario = idIntestatario;
            if(transactionRequestType.codiceFiscaleIntestatario != transactionRequestType.codiceFiscaleRichiedente)
            {
                recuperaCertificato.tipoRichiedente = "1";
                recuperaCertificato.codiceFiscaleRichiedente = transactionRequestType.codiceFiscaleRichiedente;
            }
            else { recuperaCertificato.tipoRichiedente = "2"; }
            if (certificatoId == "1" || certificatoId == "2" || certificatoId == "3")
            { recuperaCertificato.flgAnagSC = "S"; }
            else { recuperaCertificato.flgAnagSC = "A"; }
            recuperaCertificato.flgAnteprima = "N";
            if(tipoUsoId == "2")
            { recuperaCertificato.flgSempliceBollata = "B"; }
            else { recuperaCertificato.flgSempliceBollata = "S"; }
            recuperaCertificato.idCertificatiAnpr = SIPOHelper.GetCertificatoSIPO(certificatoId);
            if(tipoUsoId == "3" || (tipoUsoId == "1" && recuperaCertificato.flgAnagSC != "S"))
            { recuperaCertificato.idEsenzioneAnpr = SIPOHelper.GetEsenzioneSIPO(esenzioneId); }            
            recuperaCertificato.hostname = ConfigurationManager.AppSettings["hostname"];
            recuperaCertificato.cfUser = ConfigurationManager.AppSettings["cfuser"];
            string ricerca = JsonConvert.SerializeObject(recuperaCertificato);
            File.WriteAllText(@"c:\components\jsonrequest.txt", ricerca);          
            string AccessTokenRicerca = "bearer " + accesstoken;          
            string serviceRicerca = ConfigurationManager.AppSettings["ServiceRecuperaCertificato"];
            SIPORequestJson sipoRequest = new SIPORequestJson(serviceRicerca, "RecuperaCertificato", AccessTokenRicerca, ricerca, idIntestatario);
            var response = sipoRequest.CallingRecuperaCertificato(idIntestatario);
            verifica.IsEmettibile = response.esito;
            _log.Debug(response.esito);
            if (response.esito == false)
            { _log.Debug(transactionRequestType.codiceFiscaleIntestatario + "_" + response.risposta); }
            verifica.Certificato = response.certificato;
            verifica.Risposta = response.risposta;
            verifica.CIU = response.idCertificatoAggiunto;
            return verifica;
        }
    }
}
