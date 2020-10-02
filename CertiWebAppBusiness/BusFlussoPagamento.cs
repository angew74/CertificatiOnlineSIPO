using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.Objects.Pagamenti;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.WebApp.Business.Utility;
using Com.Unisys.Data;
using Com.Unisys.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Com.Unisys.CdR.Certi.WebApp.Business
{
    public class BusFlussoPagamento
    {
        private static readonly ILog log = LogManager.GetLogger("BusFlussoPagamento");

        private strutturaType CreaStruttura()
        {
            strutturaType struttura = new strutturaType();
            struttura.area = ConfigurationManager.AppSettings["area"];
            struttura.cdr = ConfigurationManager.AppSettings["cdr"];
            struttura.settore = ConfigurationManager.AppSettings["settore"];
            log.Debug("creato struttura");
            return struttura;
        }

        private soggettoType CreaSoggeto(string progressivo, ProfiloUtente p)
        {
            soggettoType soggetto = new soggettoType();
            anagraficaType persona = new anagraficaType();
            log.Debug("sto in crea soggetto");
            persona.cf = p.CodiceFiscale;          
             persona.codice = "1234567";            
            persona.progressivo = int.Parse(progressivo);
            if (string.IsNullOrEmpty(p.Cognome))
            { persona.cognome = "Cognome"; }
            else { persona.cognome = p.Cognome; }
            if (string.IsNullOrEmpty(p.Nome))
            { persona.nome = "Nome"; }
            else { persona.nome = p.Nome; }
            if (string.IsNullOrEmpty(p.Sesso))
            { persona.sesso = "M"; }
            else { persona.sesso = p.Sesso; }           
            persona.soggetto = "O";
            persona.persona = "F";
            if (string.IsNullOrEmpty(p.ComuneNascita))
            { persona.localita = "ROMA"; }
            else { persona.localita = p.ComuneNascita; }
            if (string.IsNullOrEmpty(p.ProvinciaNascita))
            { persona.provincia = "RM"; }
            else { persona.provincia = p.ProvinciaNascita; }
            List<anagraficaType> l = new List<anagraficaType>();
            l.Add(persona);
            soggetto.persona = l.ToArray();
            List<recapitoType> recapiti = new List<recapitoType>();
            recapitoType recapito = new recapitoType();
            if (string.IsNullOrEmpty(p.Cap))
            { recapito.cap = "00000"; }
            else { recapito.cap = p.Cap; }
            if (string.IsNullOrEmpty(p.ComuneResidenza))
            { recapito.comune = "SCONOSCIUTO"; }
            else { recapito.comune = p.ComuneResidenza; }
            recapito.tipo = "2";
            if (string.IsNullOrEmpty(p.Indirizzo))
            { recapito.descrizione = "RECAPITO SCONOSCIUTO"; }
            else { recapito.descrizione = p.Indirizzo + " " + p.Civico; }           
            if (string.IsNullOrEmpty(p.ProvinciaResidenza))
            { recapito.provincia = "NN"; }
            else { recapito.provincia = p.ProvinciaResidenza; }
            log.Debug("letto residenza");
            recapiti.Add(recapito);
            soggetto.recapiti = recapiti.ToArray();
            return soggetto;
        }

        private voceType[] CreaVoce()
        {
            List<voceType> voces = new List<voceType>();
            voceType voce = new voceType();
            voceType voce1 = new voceType();
            voceType voce2 = new voceType();
            int rata = int.Parse(ConfigurationManager.AppSettings["rataCNC"]);
            decimal importocnc = decimal.Parse(ConfigurationManager.AppSettings["importoCNC"], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            decimal importocnc1 = decimal.Parse(ConfigurationManager.AppSettings["importoCNC1"], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            decimal importocnc2 = decimal.Parse(ConfigurationManager.AppSettings["importoCNC2"], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            voce.codiceCNC = ConfigurationManager.AppSettings["codiceCNC"];
            voce.anno = System.DateTime.Now.Year;
            voce.importo = importocnc;
            voce.numerorata = rata;
            voce.datascadenza = System.DateTime.Now;
            voce.numerorataSpecified = true;
            voce.datascadenzaSpecified = true;
            voce1.codiceCNC = ConfigurationManager.AppSettings["codiceCNC1"]; ;
            voce1.anno = System.DateTime.Now.Year;
            voce1.importo = importocnc1;
            voce1.numerorata = rata;
            voce1.numerorataSpecified = true;
            voce1.datascadenzaSpecified = true;
            voce1.datascadenza = System.DateTime.Now;
            voce2.codiceCNC = ConfigurationManager.AppSettings["codiceCNC2"]; ;
            voce2.anno = System.DateTime.Now.Year;
            voce2.importo = importocnc2;
            voce2.numerorata = rata;
            voce2.numerorataSpecified = true;
            voce2.datascadenzaSpecified = true;
            voce2.datascadenza = System.DateTime.Now;
            voces.Add(voce);
            voces.Add(voce1);
            voces.Add(voce2);
            log.Debug("creato voci");
            return voces.ToArray();
        }

        private dettaglioType[] CreaDettagli(string idflusso)
        {
            List<dettaglioType> dettagli = new List<dettaglioType>();
            dettaglioType dettaglio = new dettaglioType();
            dettaglioType dettaglio1 = new dettaglioType();
            dettaglioType dettaglio2 = new dettaglioType();
            dettaglioType dettaglio3 = new dettaglioType();
            dettaglioType dettaglio4 = new dettaglioType();
            dettaglioType dettaglio5 = new dettaglioType();
            string progOggetto = ConfigurationManager.AppSettings["progOggetto"];
            int progOggetto1 = int.Parse(progOggetto) + 1;
            int progOggetto2 = int.Parse(progOggetto) + 2;
            int progOggetto3 = int.Parse(progOggetto) + 3;
            int progOggetto4 = int.Parse(progOggetto) + 4;
            int progOggetto5 = int.Parse(progOggetto) + 5;
            // primo dettaglio
            dettaglio.progPosizione = idflusso;
            dettaglio.progOggetto = progOggetto;
            dettaglio.datainizio = System.DateTime.Now;
            dettaglio.datafine = System.DateTime.Now;
            dettaglio.tipooggetto = ConfigurationManager.AppSettings["tipoOggetto0"];
            dettaglio.tipoPosizione = ConfigurationManager.AppSettings["tipoPosizione0"];
            // secondo dettaglio 
            dettaglio1.progPosizione = idflusso;
            dettaglio1.progOggetto = progOggetto1.ToString();
            dettaglio1.datainizio = System.DateTime.Now;
            dettaglio1.datafine = System.DateTime.Now;
            dettaglio1.tipooggetto = ConfigurationManager.AppSettings["tipoOggetto1"];
            dettaglio1.descrizioneoggetto = ConfigurationManager.AppSettings["descrizioneOggetto1"];
            // terzo dettaglio
            dettaglio2.progPosizione = idflusso;
            dettaglio2.progOggetto = progOggetto2.ToString();
            dettaglio2.datainizio = System.DateTime.Now;
            dettaglio2.datafine = System.DateTime.Now;
            dettaglio2.tipooggetto = ConfigurationManager.AppSettings["tipoOggetto2"];
            dettaglio2.descrizioneoggetto = ConfigurationManager.AppSettings["descrizioneOggetto2"];
            // quarto dettaglio
            dettaglio3.progPosizione = idflusso;
            dettaglio3.progOggetto = progOggetto3.ToString();
            dettaglio3.datainizio = System.DateTime.Now;
            dettaglio3.datafine = System.DateTime.Now;
            dettaglio3.tipooggetto = ConfigurationManager.AppSettings["tipoOggetto3"];
            dettaglio3.descrizioneoggetto = ConfigurationManager.AppSettings["descrizioneOggetto3"];
            // quinto dettaglio riepilogo
            dettaglio4.progPosizione = idflusso;
            dettaglio4.progOggetto = progOggetto4.ToString();
            dettaglio4.datainizio = System.DateTime.Now;
            dettaglio4.datafine = System.DateTime.Now;
            dettaglio4.tipooggetto = ConfigurationManager.AppSettings["tipoOggetto4"];
            dettaglio4.descrizioneoggetto = ConfigurationManager.AppSettings["descrizioneOggetto4"];
            dettaglio5.progPosizione = idflusso;
            dettaglio5.progOggetto = progOggetto5.ToString();
            dettaglio5.datainizio = System.DateTime.Now;
            dettaglio5.datafine = System.DateTime.Now;
            dettaglio5.tipooggetto = ConfigurationManager.AppSettings["tipoOggetto5"];
            dettaglio5.descrizioneoggetto = ConfigurationManager.AppSettings["descrizioneOggetto5"];
            dettagli.Add(dettaglio);
            dettagli.Add(dettaglio1);
            dettagli.Add(dettaglio2);
            dettagli.Add(dettaglio3);
            dettagli.Add(dettaglio4);
            dettagli.Add(dettaglio5);
            log.Debug("creato dettaglio");
            return dettagli.ToArray();
        }

        private crediti PreparaPagamentoRest(strutturaType struttura, dettaglioType[] dettagli, voceType[] voci, soggettoType soggetto, out string mess)

        {
            mess = "";
            crediti Crediti = new crediti();
            creditoType credito = new creditoType();
            posizioneType posizione = new posizioneType();
            List<soggettoType> l = new List<soggettoType>();
            l.Add(soggetto);
            posizione.soggetto = l.ToArray();
            posizione.dettaglio = dettagli;
            posizione.voce = voci;            
            List<posizioneType> posizioni = new List<posizioneType>();
            posizioni.Add(posizione);           
            credito.struttura = struttura;            
            credito.posizioni = posizioni.ToArray();
            List<creditoType> creditos = new List<creditoType>();
            creditos.Add(credito);
            Crediti.credito = creditos.ToArray();
            log.Debug("finito crediti");
            return Crediti;
        }

        private posizionePagamentoType creaPosizionePagamento(string juv)
        {
            posizionePagamentoType posizionePagamento = new posizionePagamentoType();
            posizionePagamento.quintocampoSIR = juv;
            return posizionePagamento;
        }

        private dettaglioIntermediarioPagamentoType creaDettaglioIntermediarioPagamento(string backurl, ProfiloUtente profilo)
        {
            dettaglioIntermediarioPagamentoType dettaglio = new dettaglioIntermediarioPagamentoType();
            dettaglio.backURL = backurl;
            dettaglio.codiceOperazione = ConfigurationManager.AppSettings["codiceOperazione"];
            dettaglio.codiceSistema = ConfigurationManager.AppSettings["codiceSistema"];
            anagraficaDetPosType anagraficaDetPosType = new anagraficaDetPosType();
            anagraficaDetPosType.tipoPersona = ConfigurationManager.AppSettings["tipoPersonaPagamento"];
            //  anagraficaDetPosType.tipoPersona = ConfigurationManager.AppSettings["tipoPersona"];
            anagraficaDetPosType.tipoSoggetto = ConfigurationManager.AppSettings["tipoSoggetto"];            
            anagraficaDetPosType.codiceAnagrafico = profilo.CodiceIndividuale;
            anagraficaDetPosType.nome = profilo.Nome;
            anagraficaDetPosType.cognome = profilo.Cognome;
            anagraficaDetPosType.codiceFiscale = profilo.CodiceFiscale;
            anagraficaDetPosType.recapito = new recapitoDetPosType()
            {
                provincia = profilo.ProvinciaResidenza,
                cap = profilo.Cap,
                comune = profilo.ComuneResidenza,
                indirizzo = profilo.Indirizzo + " " + profilo.Civico
            };
            dettaglio.utenteIdentificato = anagraficaDetPosType;
            log.Debug("creato dettaglio");
            return dettaglio;
        }


        public bool invioPosizioneCreditoria(string codicefiscale, string progressivo, string idflusso, string backurl,string ciu, out string juv, out string mess)
        {
            bool response = true;
            //  VERIRIC_RESP resp = BusGestioneRicerche.FindRichiedenteByCodiceFiscale(codicefiscale, "1", "1", "Nessuno", "", "", "");
            ProfiloUtente profiloUtente = new ProfiloUtente();
            if (SessionManager<ProfiloUtente>.exist(SessionKeys.RICHIEDENTE_CERTIFICATI))
            {
                profiloUtente = SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI);
                log.Debug("letto richiedente");
            }            
            else
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli: Profilo richiedente non corretto",
                            "ERR_229",
                            "Certi.WebApp.Business.BusFlussoPagamento",
                            "invioPosizioneCreditoria",
                            "Creazione posizione creditoria",
                            "Id flusso: " + idflusso,
                            "ActiveObjectCF: " + codicefiscale,
                            null);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            crediti crediti = new crediti();
            strutturaType struttura = CreaStruttura();
            soggettoType soggetto = CreaSoggeto(progressivo, profiloUtente);
            voceType[] voci = CreaVoce();
            dettaglioType[] dettaglios = CreaDettagli(idflusso);
            crediti = PreparaPagamentoRest(struttura, dettaglios, voci, soggetto, out mess);
            string xml = SerializationUtils.SerializeToXmlString<crediti>(crediti, false);
            metadata m = new metadata();
            m.TRACKER_BIZID_PROG_POSIZIONE = ciu;
            CreateClientRequest request = new CreateClientRequest(ConfigurationManager.AppSettings["addressRest"], ConfigurationManager.AppSettings["invioPosizioneCreditoria"], m, xml);
            juv = request.Calling("invioposizione");
            mess = invocaPreparaPagamentoRest(juv, ciu, backurl, profiloUtente);
            return response;
        }
        private string invocaPreparaPagamentoRest(string juv, string ciu, string backurl, ProfiloUtente profilo)
        {
            carrelloPosizioniPagamentoType carrelloPosizioni = new carrelloPosizioniPagamentoType();
            dettaglioIntermediarioPagamentoType dettaglio = creaDettaglioIntermediarioPagamento(backurl, profilo);
            posizionePagamentoType posizionePagamento = creaPosizionePagamento(juv);
            carrelloPosizioni.dettaglioIntermediarioPagamento = dettaglio;
            List<posizionePagamentoType> l = new List<posizionePagamentoType>();
            l.Add(posizionePagamento);
            carrelloPosizioni.posizioniPagamento = l.ToArray();
            string xml = SerializationUtils.SerializeToXmlString<carrelloPosizioniPagamentoType>(carrelloPosizioni, false);
            metadata m = new metadata();
            m.TRACKER_BIZID_IUV = juv;
            m.TRACKER_BIZID_PROG_POSIZIONE = ciu;
            CreateClientRequest request = new CreateClientRequest(ConfigurationManager.AppSettings["addressRest"], ConfigurationManager.AppSettings["preparaPagamentoService"], m, xml);
            string response = request.Calling("predisponi");
            return response;
        }

        public ProfiloDownload.CertificatiRow[] ControllaPagamento(string codiceFiscale, string clientID, string juv)
        {
            ProfiloDownload.CertificatiDataTable certs;            
            BUSListe lst = new BUSListe();
            int tipoRitiro = int.Parse(ConfigurationManager.AppSettings["TipoRitiro"]);
            try
            {            
                certs = lst.GetElencoDownload(codiceFiscale, tipoRitiro, int.Parse(clientID));                
                DataRow[] emptyRows = certs.Select("STATUS_ID=" + (int)Status.C_RICHIESTA_PAGAMENTO);
                DataRow[] alterRowS = certs.Select("STATUS_ID=" + (int)Status.C_VERIFICA_EMETTIBILITA_OK + " AND TIPO_USO_ID=2 AND CODICE_PAGAMENTO IS NULL");
                if (emptyRows.Length > 0)
                {
                    string[] listaCIU = new string[emptyRows.Length];
                    log.Debug("sto per ciclare i ciu");
                    for (int i = 0; i < emptyRows.Length; i++)
                    {
                        string ciu = (emptyRows[i] as ProfiloDownload.CertificatiRow).CIU;
                        juv = (emptyRows[i] as ProfiloDownload.CertificatiRow).CODICE_PAGAMENTO;
                        DateTime dataemissione = (emptyRows[i] as ProfiloDownload.CertificatiRow).T_DATA_EMISSIONE;
                        DateTime datalimite = dataemissione.AddMinutes(int.Parse(ConfigurationManager.AppSettings["refreshpage"]));
                        metadata m = new metadata();
                        log.Debug("ciu " + ciu);
                        m.TRACKER_BIZID_PROG_POSIZIONE = ciu;
                        m.TRACKER_BIZID_IUV = juv;
                        CreateClientRequest request = new CreateClientRequest(ConfigurationManager.AppSettings["addressRest"], ConfigurationManager.AppSettings["richiestaPagamentiPosizione"], m, ConfigurationManager.AppSettings["tipoIdentificativo"], juv);
                        string response = request.Calling("controlla");
                        if (!string.IsNullOrEmpty(response))
                        {
                            int statusid = (int)Status.C_VERIFICA_EMETTIBILITA_OK;
                            decimal id = (emptyRows[i] as ProfiloDownload.CertificatiRow).ID;
                            log.Debug("sto per aggiornare i pagamenti ");
                            Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamentoExt(statusid, response, id, ciu);
                        }
                        else if (System.DateTime.Now > datalimite)
                        {
                            ProfiloDownload.CertificatiRow rr = (ProfiloDownload.CertificatiRow)emptyRows[i]; listaCIU[i] = (emptyRows[i] as ProfiloDownload.CertificatiRow).CIU;
                            if (rr.STATUS_ID == (int)Status.C_RICHIESTA_PAGAMENTO)
                            {
                                int statusid = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                                decimal id = (emptyRows[i] as ProfiloDownload.CertificatiRow).ID;
                                log.Debug("sto per mettere ko il pagamento in sospeso");
                                Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamentoExt(statusid, response, id, ciu);
                            }
                        }
                    }
                }
                if(alterRowS.Length > 0)
                {
                    for (int i = 0; i < alterRowS.Length; i++)
                    {
                        int statusid = (int)Status.C_RICHIESTA_PAGAMENTO_KO;
                        decimal id = (alterRowS[i] as ProfiloDownload.CertificatiRow).ID;
                        string ciu = (alterRowS[i] as ProfiloDownload.CertificatiRow).CIU;
                        Com.Unisys.CdR.Certi.DataLayer.Dao.getDaoFactory(StoreType.ORACLE).DaoImpl.Richiesta.UpdatePagamentoExt(statusid, null, id, ciu);
                    }
                }
                log.Debug("dopo controllo pagamenti");
            }
            catch (ManagedException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + ex.Message,
                            "ERR_262",
                            "Certi.WebApp.Business.BUSFlussoRichiesta",
                            "ControllaPagamento",
                            "Controllo pagamenti",
                            "ClientID: " + clientID,
                            "ActiveObjectCF: " + codiceFiscale,
                            ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            certs = lst.GetElencoDownload(codiceFiscale, tipoRitiro, int.Parse(clientID));
            return (ProfiloDownload.CertificatiRow[])certs.Select("STATUS_ID <>" + (int)Status.C_RICHIESTA_PAGAMENTO_KO,
                "T_DATA_EMISSIONE desc");
        }

    }
}
