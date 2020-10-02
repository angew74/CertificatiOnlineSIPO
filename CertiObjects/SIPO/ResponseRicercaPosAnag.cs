using System.Collections.Generic;

public class RuoloMatricolare
{
    public int? idRuoloMatricolare { get; set; }
    public object annoIscrLeva { get; set; }
    public object progIscrLeva { get; set; }
    public object annoSuccIscr { get; set; }
    public object progSuccIscr { get; set; }
    public object flagAttivo { get; set; }
    public object comuneIscrizione { get; set; }
    public object distretto { get; set; }
    public object comuneInvio { get; set; }
    public object dataVisitaLeva { get; set; }
    public object dataArruolamento { get; set; }
    public object dataCongedo { get; set; }
    public object dataMotivo { get; set; }
    public object esitoVisita { get; set; }
    public object arma { get; set; }
    public object matricola { get; set; }
    public object corpo { get; set; }
    public object grado { get; set; }
    public object motivo { get; set; }
    public object condParticolari { get; set; }
    public object numeroOrdine { get; set; }
}

public class ConfTipoLegame
{
    public int? idTipoLegame { get; set; }
    public string descrizione { get; set; }
    public int? tipoSchedaAnpr { get; set; }
}

public class ConfMotivoCostituzione
{
    public int? idMotivoCostituzione { get; set; }
    public string descrizione { get; set; }
}

public class ConfCodiceLegameApr
{
    public int? idCodiceLegame { get; set; }
    public string descrizione { get; set; }
    public int? idCodiceLegameAnpr { get; set; }
    public ConfTipoLegame confTipoLegame { get; set; }
    public ConfMotivoCostituzione confMotivoCostituzione { get; set; }
}

public class HibernateLazyInitializer
{
}

public class ResponsabileMinore
{
    public int? idResponsabile { get; set; }
    public string cognomeResponsabile { get; set; }
    public string nomeResponsabile { get; set; }
    public string tipoResponsabile { get; set; }
    public HibernateLazyInitializer hibernateLazyInitializer { get; set; }
}

public class Provincia
{
    public int? idProvincia { get; set; }
    public string siglaProvincia { get; set; }
}

public class ComuneIscrizione
{
    public int? idComune { get; set; }
    public string istatComune { get; set; }
    public string nomeComune { get; set; }
    public string eMail { get; set; }
    public Provincia provincia { get; set; }
    public string flagSubentrato { get; set; }
    public string codiceAggior { get; set; }
    public string codiceCatastale { get; set; }
}

public class ConfTipoAtto
{
    public int? idTipoAtto { get; set; }
    public string descrizione { get; set; }
}

public class ConfStatoPratica
{
    public int? idStatoPratica { get; set; }
    public string descrizione { get; set; }
    public string isCri { get; set; }
    public string isIrr { get; set; }
    public string isAire { get; set; }
    public string isAcc { get; set; }
    public string isCre { get; set; }
    public string isDich { get; set; }
    public string isContr { get; set; }
    public string isPpt { get; set; }
}

public class Atto
{
    public int? idAtto { get; set; }
    public int? anno { get; set; }
    public string numeroAtto { get; set; }
    public string parte { get; set; }
    public string serie { get; set; }
    public int? trascritto { get; set; }
    public ComuneIscrizione comuneIscrizione { get; set; }
    public ConfTipoAtto confTipoAtto { get; set; }
    public int? annoPratica { get; set; }
    public int? numeroPratica { get; set; }
    public ConfStatoPratica confStatoPratica { get; set; }
}

public class Comune
{
    public int? idComune { get; set; }
    public string istatComune { get; set; }
    public string nomeComune { get; set; }
    public string eMail { get; set; }
    public Provincia provincia { get; set; }
    public string flagSubentrato { get; set; }
    public string codiceAggior { get; set; }
    public string codiceCatastale { get; set; }
    public string descrizioneLocalita { get; set; }
}

public class Nascita
{
    public int? idNascita { get; set; }
    public string dataEvento { get; set; }
    public object flgSenzaGiorno { get; set; }
    public object flgSenzaMese { get; set; }
    public object luogoEccezionale { get; set; }
    public object nominativoMaternita { get; set; }
    public object nominativoPaternita { get; set; }
    public object codComuneAggior { get; set; }
    public Atto atto { get; set; }
    public Comune comune { get; set; }
    public object localita { get; set; }
}

public class ConfStatoValiditaCarta
{
    public int? idStatoValiditaCarta { get; set; }
    public string descrizione { get; set; }
}

public class ConfStruttureConv
{
    public int? id { get; set; }
    public string descrizione { get; set; }
}

public class ConfStruttureInterneRc
{
    public int? idConfStruttureInterne { get; set; }
    public int? codiceResponsabile { get; set; }
    public string codiceStruttura { get; set; }
    public string cognomeResponsabile { get; set; }
    public string descrizioneStruttura { get; set; }
    public string nomeResponsabile { get; set; }
    public object flgGruppoVigili { get; set; }
    public string centroRicavo { get; set; }
    public string codiceDocumento { get; set; }
    public string codiceProceduraChiamante { get; set; }
    public int? flgAttivo { get; set; }
    public string intestazioneDip { get; set; }
    public object intestazioneUOApp { get; set; }
    public string settore { get; set; }
    public string tipoProtocollo { get; set; }
    public string numeroStruttura { get; set; }
}

public class Id
{
    public int? idComunicazione { get; set; }
    public int? idDestinatario { get; set; }
}

public class Comunicazioni
{
    public int? id { get; set; }
    public string dataComunicazione { get; set; }
    public string mittenteComunicazione { get; set; }
    public string oggetto { get; set; }
    public string testo { get; set; }
}

public class Utenti
{
    public int? id { get; set; }
    public string cognome { get; set; }
    public object dataCancellazione { get; set; }
    public string dataCreazioneUtenza { get; set; }
    public string flgAttivo { get; set; }
    public string flgCancellato { get; set; }
    public string nome { get; set; }
    public string nomeUtente { get; set; }
    public ConfStruttureConv confStruttureConv { get; set; }
    public ConfStruttureInterneRc confStruttureInterneRc { get; set; }
    public string codiceFiscale { get; set; }
    public string nomeUtenteAggior { get; set; }
    public IList<RutenteComunicazione> rutenteComunicaziones { get; set; }
}

public class RutenteComunicazione
{
    public Id id { get; set; }
    public long dataLettura { get; set; }
    public string flgDaLeggere { get; set; }
    public Comunicazioni comunicazioni { get; set; }
    public Utenti utenti { get; set; }
}


public class ConfSedeMunicipio
{
    public int? idSedeMunicipio { get; set; }
    public int? flagRipartizione { get; set; }
    public string descrizione { get; set; }
    public string descrizioneCompleta { get; set; }
    public ConfStruttureInterneRc confStruttureInterneRc { get; set; }
    public string cap { get; set; }
}

public class CartaIdentita
{
    public string numeroCartaIdentita { get; set; }
    public string dataRilascio { get; set; }
    public string dataScadenza { get; set; }
    public string espatrio { get; set; }
    public string tipologia { get; set; }
    public string stampaDonazione { get; set; }
    public Comune comune { get; set; }
    public ConfStatoValiditaCarta confStatoValiditaCarta { get; set; }
    public string chkCartaStampata { get; set; }
    public Utenti utenti { get; set; }
    public string stampaStatoCivile { get; set; }
    public ConfSedeMunicipio confSedeMunicipio { get; set; }
}

public class Cittadinanza1
{
    public int? idCittadinanza { get; set; }
    public int? codiceStato { get; set; }
    public string comunitario { get; set; }
    public string descrizioneStato { get; set; }
    public string note { get; set; }
    public string siglaNazione { get; set; }
    public string codiceISO3166 { get; set; }
}

public class Cittadinanza2
{
    public int? idCittadinanza { get; set; }
    public int? codiceStato { get; set; }
    public string comunitario { get; set; }
    public string descrizioneStato { get; set; }
    public string note { get; set; }
    public string siglaNazione { get; set; }
    public string codiceISO3166 { get; set; }
}

public class ComuneElettore
{
    public int? idComune { get; set; }
    public string descrizioneLocalita { get; set; }
    public string istatComune { get; set; }
    public string nomeComune { get; set; }
    public string eMail { get; set; }
    public Provincia provincia { get; set; }
    public string flagSubentrato { get; set; }
    public string codiceAggior { get; set; }
}

public class ConfCertificabilita
{
    public int? idCertificabilita { get; set; }
    public string descrizione { get; set; }
}

public class ConfCodiceLegameFamigliaConv
{
    public int? idCodiceLegame { get; set; }
    public string descrizione { get; set; }
    public int? idCodiceLegameAnpr { get; set; }
    public ConfTipoLegame confTipoLegame { get; set; }
    public ConfMotivoCostituzione confMotivoCostituzione { get; set; }
}

public class ConfMotivoIscrizioneApr
{
    public int? idMotivoIscrizioneApr { get; set; }
    public string descrizione { get; set; }
}

public class ConfStatusSoggetto
{
    public int? idStatusSoggetto { get; set; }
    public string descrizione { get; set; }
}

public class ConfTipoIscrizione
{
    public int? idTipoIscrizione { get; set; }
    public string descrizione { get; set; }
}

public class ConfPosizioneProfessionale
{
    public string idPosProfessionaleAnpr { get; set; }
    public string descrizione { get; set; }
    public int? idPosProfessionaleApr { get; set; }
}

public class ConfStatoCivile
{
    public int? idStatoCivile { get; set; }
    public string descrizione { get; set; }
}

public class ConfTitoliStudio
{
    public string idTitoloStudioAnpr { get; set; }
    public string descrizione { get; set; }
    public int? idTitoloStudioApr { get; set; }
}

public class MotivoCostituzione
{
    public int? idMotivoCostituzione { get; set; }
    public string descrizione { get; set; }
}

public class ConfTipoMovimentazione
{
    public int? idTipoMovimentazione { get; set; }
    public string descrizione { get; set; }
}

public class TipoIndirizzo
{
    public int? idTipoIndirizzo { get; set; }
    public string descrizione { get; set; }
}

public class ConfMunicipio
{
    public int? idMunicipio { get; set; }
    public string descrizione { get; set; }
}

public class Civico
{
    public int? idCivico { get; set; }
    public int? numero { get; set; }
}

public class ConfTipoSpecieToponimo
{
    public int? idTipoSpecieToponimo { get; set; }
    public string descrizione { get; set; }
}

public class Toponimo
{
    public int? idToponimo { get; set; }
    public object codSpecie { get; set; }
    public string codToponimo { get; set; }
    public string denominazioneToponimo { get; set; }
    public object specie { get; set; }
    public object specieFonte { get; set; }
    public object toponimoFonte { get; set; }
    public ConfTipoSpecieToponimo confTipoSpecieToponimo { get; set; }
    public string denominazioneBreve { get; set; }
}

public class Residenza
{
    public int? idResidenza { get; set; }
    public object cap { get; set; }
    public object frazione { get; set; }
    public object noteIndirizzo { get; set; }
    public TipoIndirizzo tipoIndirizzo { get; set; }
    public ConfMunicipio confMunicipio { get; set; }
    public Civico civico { get; set; }
    public object civicoInterno { get; set; }
    public Comune comune { get; set; }
    public object localita { get; set; }
    public Toponimo toponimo { get; set; }
    public object luogoEccezionale { get; set; }
    public string dataDecorrenzaResidenza { get; set; }
    public object estremiCatastali { get; set; }
    public object codComuneAggior { get; set; }
}

public class FamigliaConvivenza
{
    public int? idFamigliaConv { get; set; }
    public string idFamigliaConvivenzaComuneIstat { get; set; }
    public string dataOrigineFamiglia { get; set; }
    public string idFamigliaConvAnpr { get; set; }
    public MotivoCostituzione motivoCostituzione { get; set; }
    public ConfTipoMovimentazione confTipoMovimentazione { get; set; }
    public ConfTipoLegame confTipoLegame { get; set; }
    public string flagAttivo { get; set; }
    public string codiceFamiglia { get; set; }
    public Residenza residenza { get; set; }
}

public class Consolato
{
    public int? idConsolato { get; set; }
    public string descrizione { get; set; }
    public int? idLocalita { get; set; }
    public string descrizioneLocalita { get; set; }
    public object email { get; set; }
}

public class ConfStatoEstero
{
    public int? idStatoEstero { get; set; }
    public string comunitario { get; set; }
    public string descrizione { get; set; }
    public string siglaNazione { get; set; }
    public string codiceAnpr { get; set; }
}

public class Localita
{
    public int? idLocalita { get; set; }
    public string descrizioneLocalita { get; set; }
    public string provinciaContea { get; set; }
    public Consolato consolato { get; set; }
    public ConfStatoEstero confStatoEstero { get; set; }
    public string codiceAggior { get; set; }
}

public class Morte
{
    public int? idMorte { get; set; }
    public string dataEvento { get; set; }
    public string flgSenzaGiorno { get; set; }
    public string flgSenzaMese { get; set; }
    public string luogoEccezionale { get; set; }
    public object ordineMatrimonioPrecedente { get; set; }
    public object codComuneAggior { get; set; }
    public Atto atto { get; set; }
    public Comune comune { get; set; }
    public Localita localita { get; set; }
    public object causaDecesso { get; set; }
}

public class Censimento
{
    public int? idCensimento { get; set; }
    public int? annoCensimento { get; set; }
    public string dataRegolarizzazione { get; set; }
    public string foglioCensimento { get; set; }
    public string motivoCompilazione { get; set; }
    public string sezioneCensimento { get; set; }
}

public class ComuneLeva
{
    public int? idComune { get; set; }
    public string descrizioneLocalita { get; set; }
    public string istatComune { get; set; }
    public string nomeComune { get; set; }
    public string eMail { get; set; }
    public Provincia provincia { get; set; }
    public string flagSubentrato { get; set; }
    public string codiceAggior { get; set; }
}

public class ConfDisattivazioneSoggetto
{
    public int? idDisattivazioneSoggetto { get; set; }
    public string descrizione { get; set; }
}

public class CertificazioneSospesa
{
    public int? idCertificazioneSospesa { get; set; }
    public string flagCertificazioneNascita { get; set; }
    public string flagEstrattoNascita { get; set; }
    public string flagCertificazioneMatrimonio { get; set; }
    public string flagEstrattoMatrimonio { get; set; }
    public string flagCertificazioneMorte { get; set; }
    public string flagEstrattoMorte { get; set; }
}

public class SoggettoPadre
{
    public int? idSoggetto { get; set; }
    public string aire { get; set; }
    public object codiceAnagaire { get; set; }
    public object annoEspatrio { get; set; }
    public string codiceFiscale { get; set; }
    public string codiceIndividuale { get; set; }
    public object idFamigliaProvenienzaAnpr { get; set; }
    public object idSoggettoPrec { get; set; }
    public string cognome { get; set; }
    public object dataDecorrenzaReferente { get; set; }
    public string dataAttribuzioneValiditaCf { get; set; }
    public object dataDecorrenzaFamconv { get; set; }
    public object dataDecorrenzaLegameFamconv { get; set; }
    public object dataPrimaIscrizioneComune { get; set; }
    public string dataUltimoAggiornamento { get; set; }
    public object dataValiditaCittadinanza { get; set; }
    public object dataValiditaCittadinanza2 { get; set; }
    public string flagSoggettoAttivo { get; set; }
    public int? idSchedaAnpr { get; set; }
    public string nome { get; set; }
    public object noteStatoCivile { get; set; }
    public object progrComponenteFamconv { get; set; }
    public string sesso { get; set; }
    public object validitaCf { get; set; }
    public object possessoAutoveicoli { get; set; }
    public object ruoloMatricolare { get; set; }
    public object confCodiceLegameApr { get; set; }
    public Morte morte { get; set; }
    public object responsabileMinore { get; set; }
    public object elencoPreparatorioLeva { get; set; }
    public object listaLeva { get; set; }
    public Nascita nascita { get; set; }

    public CartaIdentita cartaIdentita { get; set; }
    public Censimento censimento { get; set; }
    public Cittadinanza1 cittadinanza1 { get; set; }
    public object cittadinanza2 { get; set; }
    public ComuneElettore comuneElettore { get; set; }
    public ComuneLeva comuneLeva { get; set; }
    public ConfCertificabilita confCertificabilita { get; set; }
    public object confCodiceLegameFamigliaConv { get; set; }
    public object confCondNonProfessionale { get; set; }
    public ConfMotivoIscrizioneApr confMotivoIscrizioneApr { get; set; }
    public ConfStatusSoggetto confStatusSoggetto { get; set; }
    public ConfDisattivazioneSoggetto confDisattivazioneSoggetto { get; set; }
    public ConfTipoIscrizione confTipoIscrizione { get; set; }
    public object confPosizioneProfessionale { get; set; }
    public ConfStatoCivile confStatoCivile { get; set; }
    public ConfTitoliStudio confTitoliStudio { get; set; }
    public object famigliaConvivenza { get; set; }
    public object senzaFissaDimora { get; set; }
    public object soggiorno { get; set; }
    public CertificazioneSospesa certificazioneSospesa { get; set; }
    public object confMotivoMemorizzazione { get; set; }
    public object operazioneAnpr { get; set; }
    public object flagCfAttivo { get; set; }
    public object motivoEliminazioneCf { get; set; }
    public object recapitoTelefonico { get; set; }
    public object email { get; set; }
    public object dataInserimentoListaLeva { get; set; }
    public object dataVariazioneAnagrafica { get; set; }
    public object dataAcquisizioneCittadinanza { get; set; }
    public object dataDecorrenzaResidenza { get; set; }
    public object comuneGiuramentoCittadinanza { get; set; }
    public object altroDocRiconoscimento { get; set; }
    public object flagVariazioneAnagrafica { get; set; }
}

public class ConfTipoSoggiorno
{
    public int? idTipoSoggiorno { get; set; }
    public string descrizione { get; set; }
    public string idTipoSoggiornoAnpr { get; set; }
}

public class Utente
{
    public int? id { get; set; }
    public string cognome { get; set; }
    public object dataCancellazione { get; set; }
    public string dataCreazioneUtenza { get; set; }
    public string flgAttivo { get; set; }
    public string flgCancellato { get; set; }
    public string nome { get; set; }
    public string nomeUtente { get; set; }
    public object confStruttureConv { get; set; }
    public ConfStruttureInterneRc confStruttureInterneRc { get; set; }
    public string codiceFiscale { get; set; }
    public object nomeUtenteAggior { get; set; }
    public IList<object> rutenteComunicaziones { get; set; }
}

public class Struttura
{
    public int? idConfStruttureInterne { get; set; }
    public int? codiceResponsabile { get; set; }
    public string codiceStruttura { get; set; }
    public string cognomeResponsabile { get; set; }
    public string descrizioneStruttura { get; set; }
    public string nomeResponsabile { get; set; }
    public object flgGruppoVigili { get; set; }
    public string centroRicavo { get; set; }
    public int? codiceDocumento { get; set; }
    public string codiceProceduraChiamante { get; set; }
    public int? flgAttivo { get; set; }
    public string intestazioneDip { get; set; }
    public string intestazioneUOApp { get; set; }
    public string settore { get; set; }
    public string tipoProtocollo { get; set; }
    public int? numeroStruttura { get; set; }
}

public class Soggiorno
{
    public int? idSoggiorno { get; set; }
    public string dataRilascio { get; set; }
    public object dataScadenza { get; set; }
    public object dataRinnovo { get; set; }
    public string dataRichiestaRinnovo { get; set; }
    public string flgTipoSoggiorno { get; set; }
    public object flgRuoloSoggiorno { get; set; }
    public object note { get; set; }
    public string numeroDocumento { get; set; }
    public string idDocumento { get; set; }
    public object numeroPermessoSogg { get; set; }
    public string numeroAttestatoSogg { get; set; }
    public object questureRilascio { get; set; }
    public string tipoAttestato { get; set; }
    public string flgCancellato { get; set; }
    public object dataCancellazione { get; set; }
    public object comune { get; set; }
    public ConfTipoSoggiorno confTipoSoggiorno { get; set; }
    public object blob { get; set; }
    public object confMotivoAnnullaAttSogg { get; set; }
    public object filenameDoc { get; set; }
    public object altraMotivazione { get; set; }
    public string dataUltimoAggiornamento { get; set; }
    public Utente utente { get; set; }
    public Struttura struttura { get; set; }
}

public class SoggettoMadre
{
    public int? idSoggetto { get; set; }
    public string aire { get; set; }
    public object codiceAnagaire { get; set; }
    public object annoEspatrio { get; set; }
    public string codiceFiscale { get; set; }
    public string codiceIndividuale { get; set; }
    public object idFamigliaProvenienzaAnpr { get; set; }
    public int? idSoggettoPrec { get; set; }
    public string cognome { get; set; }
    public object dataDecorrenzaReferente { get; set; }
    public string dataAttribuzioneValiditaCf { get; set; }
    public object dataDecorrenzaFamconv { get; set; }
    public object dataDecorrenzaLegameFamconv { get; set; }
    public object dataPrimaIscrizioneComune { get; set; }
    public string dataUltimoAggiornamento { get; set; }
    public object dataValiditaCittadinanza { get; set; }
    public object dataValiditaCittadinanza2 { get; set; }
    public string flagSoggettoAttivo { get; set; }
    public object idSchedaAnpr { get; set; }
    public string nome { get; set; }
    public object noteStatoCivile { get; set; }
    public object progrComponenteFamconv { get; set; }
    public string sesso { get; set; }
    public int? validitaCf { get; set; }
    public object possessoAutoveicoli { get; set; }
    public object ruoloMatricolare { get; set; }
    public ConfCodiceLegameApr confCodiceLegameApr { get; set; }
    public Morte morte { get; set; }
    public object responsabileMinore { get; set; }
    public object elencoPreparatorioLeva { get; set; }
    public object listaLeva { get; set; }
    public Nascita nascita { get; set; }
    public CartaIdentita cartaIdentita { get; set; }
    public Censimento censimento { get; set; }
    public Cittadinanza1 cittadinanza1 { get; set; }
    public Cittadinanza2 cittadinanza2 { get; set; }
    public ComuneElettore comuneElettore { get; set; }
    public ComuneLeva comuneLeva { get; set; }
    public ConfCertificabilita confCertificabilita { get; set; }
    public ConfCodiceLegameFamigliaConv confCodiceLegameFamigliaConv { get; set; }
    public object confCondNonProfessionale { get; set; }
    public ConfMotivoIscrizioneApr confMotivoIscrizioneApr { get; set; }
    public ConfStatusSoggetto confStatusSoggetto { get; set; }
    public ConfDisattivazioneSoggetto confDisattivazioneSoggetto { get; set; }
    public ConfTipoIscrizione confTipoIscrizione { get; set; }
    public ConfPosizioneProfessionale confPosizioneProfessionale { get; set; }
    public ConfStatoCivile confStatoCivile { get; set; }
    public ConfTitoliStudio confTitoliStudio { get; set; }
    public FamigliaConvivenza famigliaConvivenza { get; set; }
    public object senzaFissaDimora { get; set; }
    public Soggiorno soggiorno { get; set; }
    public CertificazioneSospesa certificazioneSospesa { get; set; }
    public object confMotivoMemorizzazione { get; set; }
    public object operazioneAnpr { get; set; }
    public string flagCfAttivo { get; set; }
    public object motivoEliminazioneCf { get; set; }
    public object recapitoTelefonico { get; set; }
    public object email { get; set; }
    public object dataInserimentoListaLeva { get; set; }
    public object dataVariazioneAnagrafica { get; set; }
    public object dataAcquisizioneCittadinanza { get; set; }
    public object dataDecorrenzaResidenza { get; set; }
    public object comuneGiuramentoCittadinanza { get; set; }
    public object altroDocRiconoscimento { get; set; }
    public object flagVariazioneAnagrafica { get; set; }
}

public class OperazioneAnpr
{
    public int? idOperazioneAnpr { get; set; }
    public string dataOperazioneAnpr { get; set; }
    public string idOperazioneComune { get; set; }
    public string motivoOperazioneAnpr { get; set; }
    public string tipoOperazioneAnpr { get; set; }
    public object note { get; set; }
}

public class ResponseRicercaPosAnag
{
    public List<MyArray> MyArray { get; set; }
}
public class MyArray
{
    public int? idSoggetto { get; set; }
    public string aire { get; set; }
    public object codiceAnagaire { get; set; }
    public object annoEspatrio { get; set; }
    public string codiceFiscale { get; set; }
    public string codiceIndividuale { get; set; }
    public int? idFamigliaProvenienzaAnpr { get; set; }
    public int? idSoggettoPrec { get; set; }
    public string cognome { get; set; }
    public object dataDecorrenzaReferente { get; set; }
    public object dataAttribuzioneValiditaCf { get; set; }
    public string dataDecorrenzaFamconv { get; set; }
    public string dataDecorrenzaLegameFamconv { get; set; }
    public string dataPrimaIscrizioneComune { get; set; }
    public string dataUltimoAggiornamento { get; set; }
    public object dataValiditaCittadinanza { get; set; }
    public object dataValiditaCittadinanza2 { get; set; }
    public string flagSoggettoAttivo { get; set; }
    public int? idSchedaAnpr { get; set; }
    public string nome { get; set; }
    public object noteStatoCivile { get; set; }
    public object progrComponenteFamconv { get; set; }
    public string sesso { get; set; }
    public int? validitaCf { get; set; }
    public string possessoAutoveicoli { get; set; }
    public RuoloMatricolare ruoloMatricolare { get; set; }
    public ConfCodiceLegameApr confCodiceLegameApr { get; set; }
    public object morte { get; set; }
    public ResponsabileMinore responsabileMinore { get; set; }
    public object elencoPreparatorioLeva { get; set; }
    public object listaLeva { get; set; }
    public Nascita nascita { get; set; }
    public CartaIdentita cartaIdentita { get; set; }
    public object censimento { get; set; }
    public Cittadinanza1 cittadinanza1 { get; set; }
    public Cittadinanza2 cittadinanza2 { get; set; }
    public ComuneElettore comuneElettore { get; set; }
    public object comuneLeva { get; set; }
    public ConfCertificabilita confCertificabilita { get; set; }
    public ConfCodiceLegameFamigliaConv confCodiceLegameFamigliaConv { get; set; }
    public object confCondNonProfessionale { get; set; }
    public ConfMotivoIscrizioneApr confMotivoIscrizioneApr { get; set; }
    public ConfStatusSoggetto confStatusSoggetto { get; set; }
    public object confDisattivazioneSoggetto { get; set; }
    public ConfTipoIscrizione confTipoIscrizione { get; set; }
    public ConfPosizioneProfessionale confPosizioneProfessionale { get; set; }
    public ConfStatoCivile confStatoCivile { get; set; }
    public ConfTitoliStudio confTitoliStudio { get; set; }
    public FamigliaConvivenza famigliaConvivenza { get; set; }
    public object senzaFissaDimora { get; set; }
    public SoggettoPadre soggettoPadre { get; set; }
    public SoggettoMadre soggettoMadre { get; set; }
    public object soggettoReferente { get; set; }
    public object soggiorno { get; set; }
    public object certificazioneSospesa { get; set; }
    public object confMotivoMemorizzazione { get; set; }
    public OperazioneAnpr operazioneAnpr { get; set; }
    public string flagCfAttivo { get; set; }
    public object motivoEliminazioneCf { get; set; }
    public object recapitoTelefonico { get; set; }
    public object email { get; set; }
    public object dataInserimentoListaLeva { get; set; }
    public object dataVariazioneAnagrafica { get; set; }
    public object dataAcquisizioneCittadinanza { get; set; }
    public string dataDecorrenzaResidenza { get; set; }
    public object comuneGiuramentoCittadinanza { get; set; }
    public object altroDocRiconoscimento { get; set; }
    public object flagVariazioneAnagrafica { get; set; }
}
