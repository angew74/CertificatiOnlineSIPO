using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.DataObjects.Common.RicercheAnagrafiche;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Caching;
       
using log4net;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSRalpo
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BUSRalpo));
        RicercaRalpoResponse _ralpoResponse = new RicercaRalpoResponse();

        #region Class Properties

        public RicercaRalpoResponse RalpoResponse
        {
            get { return _ralpoResponse; }
            set { _ralpoResponse = value; }
        }

        #endregion

        /// <summary>
        /// Costruttore
        /// </summary>
        public BUSRalpo()
        {
        }

        /// <summary>
        /// Chiamata alla ricerca in backend della persona con il codice fiscale dato
        /// </summary>
        /// <param name="codiceFiscale">Codice fiscale dell'intestatario</param>
        /// <returns>true-false</returns>
        public bool FindByCodiceFiscale(string codiceFiscale)
        {
            bool bRet = false;
            string funzione = MapperFunctionsNames.ricercaCodiceFiscale;
            RicercaRalpoRequest ralpoRequest = new RicercaRalpoRequest();
            ralpoRequest.Persona.AddPersonaRow("", codiceFiscale, "", "", "", "", "", "");
            _ralpoResponse = DoradoProxy.ExecuteDataSet<RicercaRalpoResponse>(ralpoRequest, funzione);
            if (_ralpoResponse.Messaggi.Count == 0)
                bRet = true;
            return bRet;

        }

        /// <summary>
        /// Chiamata alla ricerca in backend dei componenti della famiglia della persona
        /// con il codice fiscale dato
        /// </summary>
        /// <param name="codiceFiscale">Codice fiscale del richiedente</param>
        /// <returns>true-false</returns>
        public bool FindComponentiByCodiceFiscale(string codiceFiscale)
        {
            bool bRet = false;
            string funzione = MapperFunctionsNames.ricercaComponentiFamiglia;
            RicercaRalpoRequest ralpoRequest = new RicercaRalpoRequest();
            ralpoRequest.Persona.AddPersonaRow("", codiceFiscale, "", "", "", "", "", "");
            _ralpoResponse = DoradoProxy.ExecuteDataSet<RicercaRalpoResponse>(ralpoRequest, funzione);
            if (_ralpoResponse.Messaggi.Count == 0)
                bRet = true;
            return bRet;
        }
    }
}
