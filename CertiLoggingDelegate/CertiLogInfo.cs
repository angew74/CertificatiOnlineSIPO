using System;
using System.Collections.Generic;
using System.Text;
using System.Xml; 
using Com.Unisys.Logging;

namespace Com.Unisys.CdR.Certi.LoggingDelegate
{
    public delegate void LogInfoHandler(BaseLogInfo i);

    /// <summary>
    /// Classe di serializzazione e desserializzazione di log 
    /// </summary>
    [Serializable]
    public class CertiLogInfo : Com.Unisys.Logging.BaseLogInfo
    {
        public static bool isEventNull()
        {
            if (onNotify != null) return false;
            return true;
        }

        public static event LogInfoHandler onNotify;

        public CertiLogInfo() : base() { }

        public void Notify()
        {
            if (onNotify != null)
                onNotify(this);

        }

        /// <summary>
        /// Costruttore generico che estende il costruttore della classe astratta BaseLogInfo
        /// </summary>
        /// <param name="appCode">Codice applicazione</param>
        /// <param name="logCode">Tipo di log</param>
        /// <param name="details">Dettagli e informazioni aggiuntive sul log</param>
        /// <param name="flussoID">Identificativo flusso</param>
        /// <param name="clientID">Identificativo client chiamante</param>
        /// <param name="activeObjectCF">Codice fiscale utente attivo (richiedente)</param>
        /// <param name="activeObjectIP">Ip dell'utente attivo (richiedente)</param>
        /// <param name="passiveObjectCF">Codice fiscale dell'utente passivo (intestatario)</param>
        /// 
        public CertiLogInfo(string appCode, string logCode, string details, string flussoID, string clientID, string activeObjectCF, string activeObjectIP, string passiveObjectCF)
            : base(appCode, logCode, details)
        {
            this.flussoID = flussoID;
            this.clientID = clientID;
            this.activeObjectCF = activeObjectCF;
            this.activeObjectIP = activeObjectIP;
            this.passiveObjectCF = passiveObjectCF;
        }

        /// <summary>
        /// ID Flusso 
        /// </summary>
        public string flussoID;
        /// <summary>
        /// ID Client 
        /// </summary>
        public string clientID;
        /// <summary>
        /// codice fiscale oggetto attivo (richiedente)
        /// </summary>
        public string activeObjectCF;
        /// <summary>
        /// IP Utente (richiedente)
        /// </summary>
        public string activeObjectIP;
        /// <summary>
        /// codice fiscale oggetto passiveo (intestatario)
        /// </summary>
        public string passiveObjectCF;


        /// <summary>
        /// Override del metodo to string che scrive in esteso il file xml del log
        /// </summary>
        /// <returns><c>string</c> stringa dell'XML da accodare</returns>
        public override string ToString()
        {
            return base.ToString() + 
                "|flussoID:" + flussoID + 
                "|clientID:" + clientID + 
                "|activeObjectCF:" + activeObjectCF + 
                "|activeObjectIP:" + activeObjectIP + 
                "|passiveObjectCF:" + passiveObjectCF;
        }

        /// <summary>
        ///  Metodo per la deserializzazione dell'XML document già accodato nel logger queue e la trasformazione 
        /// in una classe per la gestione delle informazioni da accodare in base dati
        /// </summary>
        /// <param name="doc">Xml Document da deserializzare</param>
        /// <returns><c>CertiLogInfo</c>classe che espone le proprietà dell'oggetto CertiLogInfo per la scrittura in banca dati</returns>
        public static new CertiLogInfo Deserialize(XmlDocument doc)
        {

            return (CertiLogInfo)ConversionUtils.XmlToObject(doc, typeof(CertiLogInfo));


        }

    }
}
