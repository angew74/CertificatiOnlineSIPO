using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Com.Unisys.CdR.Certi.Caching
{
    public enum CacheKeys
    {

        CERTIFICATI_ATTIVI,
        CERTIFICATI_ATTIVI_AVVOCATI,
        MOTIVAZIONI_ATTIVI,
        CLIENTS,
        LISTA_DECODIFICA_MESSAGGI,
        LISTA_ERRORI_SOAP,
        Base_TO_PDF,
        Testa_TO_PDF,
        Nascita_TO_XML,
        Nascita_TO_PDF,
        Matrimonio_TO_XML,
        Matrimonio_TO_PDF,
        Decesso_TO_XML,
        Decesso_TO_PDF,
        Cittadinanza_TO_XML,
        Cittadinanza_TO_PDF,
        CittadinanzaAIRE_TO_XML,
        CittadinanzaAIRE_TO_PDF,
        Residenza_TO_XML,
        Residenza_TO_PDF,
        ResidenzaAIRE_TO_XML,
        ResidenzaAIRE_TO_PDF,
        StatodiFamiglia_TO_XML,
        StatodiFamiglia_TO_PDF,
        GodimentoDirittiPolitici_TO_XML,
        GodimentoDirittiPolitici_TO_PDF,
        ResidenzaCittadinanza_TO_XML,
        ResidenzaCittadinanza_TO_PDF,
        ResidenzaCittadinanzaAIRE_TO_XML,
        ResidenzaCittadinanzaAIRE_TO_PDF,
        ResidenzaCittadinanzaStatoCivileNascitaDirittiPolitici_TO_XML,
        ResidenzaCittadinanzaStatoCivileNascitaDirittiPolitici_TO_PDF,
        ResidenzaCittadinanzaStatoCivileNascitaDirittiPoliticiStatodiFamiglia_TO_PDF,
        ResidenzaCittadinanzaStatoCivileNascitaDirittiPoliticiStatodiFamiglia_TO_XML,
        ResidenzaCittadinanzaStatoLibero_TO_XML,
        ResidenzaCittadinanzaStatoLibero_TO_PDF,
        ResidenzaStatoLibero_TO_XML,
        ResidenzaStatoLibero_TO_PDF,
        StatodiFamigliaAIRE_TO_XML,
        StatodiFamigliaAIRE_TO_PDF,
        StoricoAnagrafico_TO_XML,
        StoricoAnagrafico_TO_PDF,
        StatoLibero_TO_XML,
        StatoLibero_TO_PDF,
        EsitoLeva_TO_XML,
        EsitoLeva_TO_PDF,
        IscrizioneListaLeva_TO_XML,
        IscrizioneListaLeva_TO_PDF,
        IscrizioneListaGenerale_TO_XML,
        IscrizioneListaGenerale_TO_PDF,
        Coda_TO_PDF,
        Coda_PA_TO_PDF,
        CERTIFICATI_ATTIVI_WS,
        ESENZIONI_ATTIVI_WS,
        TIPI_USO_WS,
        CLIENTS_WS,
        DICITURE_PAGAMENTI_WS
    }
}
