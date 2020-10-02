using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.Utils
{
    /// <summary>
    /// Elenco dei passaggi del ciclo di vita di una richiesta (flusso)
    /// </summary>
    public enum Status : int
    {
        /// <remarks/>
        R_INZIALIZZAZIONE = 1,
        /// <remarks/>
        R_INIZIALIZZAZIONE_OK,
        /// <remarks/>
        R_INIZIALIZZAZIONE_KO,
        /// <remarks/>
        R_VERIFICA_EMETTIBILITA,
        /// <remarks/>
        R_VERIFICA_EMETTIBILITA_OK,
        /// <remarks/>
        R_VERIFICA_EMETTIBILITA_KO,
        /// <remarks/>
        R_EMISSIONE,
        /// <remarks/>
        R_EMISSIONE_OK,
        /// <remarks/>
        R_EMISSIONE_KO,
        /// <remarks/>
        R_CONFERMA_STAMPA,
        /// <remarks/>
        R_CONFERMA_STAMPA_COMPLETA,
        /// <remarks/>
        R_CONFERMA_STAMPA_PARZIALE,
        /// <remarks/>
        C_VERIFICA_EMETTIBILITA,
        /// <remarks/>
        C_VERIFICA_EMETTIBILITA_OK,
        /// <remarks/>
        C_VERIFICA_EMETTIBILITA_KO,
        /// <remarks/>
        C_EMISSIONE,
        /// <remarks/>
        C_EMISSIONE_OK,
        /// <remarks/>
        C_EMISSIONE_KO,
        /// <remarks/>
        C_RICHIESTA_PAGAMENTO,
        /// <remarks/>
        C_RICHIESTA_PAGAMENTO_OK ,
        /// <remarks/>
        C_RICHIESTA_PAGAMENTO_KO,
        /// <remarks/>
        C_GENERAZIONE_PDF,
        /// <remarks/>
        C_GENERAZIONE_PDF_OK,
        /// <remarks/>
        C_GENERAZIONE_PDF_KO,
        /// <remarks/>
        C_RITIRATO

    }

    /// <summary>
    /// Elenco step di  una richiesta (flusso)
    /// </summary>
    public enum StepRichiesta
    {
        /// <remarks/>
        INIZIALIZZAZIONE,
        /// <remarks/>
        VERIFICA_EMETTIBILITA,
        /// <remarks/>
        RICHIESTA_CERTIFICATI,
        /// <remarks/>
        CONTABILIZZAZIONE,
        /// <remarks/>
        CONFERMA_STAMPA
    }

    public enum IDUsoCertificatoType
    {
        /// <remarks/>
        SEMPL = 1,

        /// <remarks/>
        BOLLO,

        /// <remarks/>
        NODIRSG,
    }
}
