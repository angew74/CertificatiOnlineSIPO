using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.DataObjects.CertificatiAnagrafici;
using Com.Unisys.CdR.Certi.Objects;    
using Com.Unisys.CdR.Certi.WS.Dati;     
using log4net;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSEmissione
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(BUSEmissione));
        EmissioneCertificatiResponse _emiResponse = new EmissioneCertificatiResponse(); 
 
        #region Class Properties


        public EmissioneCertificatiResponse EmiResponse
        {
            get { return _emiResponse; }
            set { _emiResponse = value; }
        }

		#endregion

        /// <summary>
		/// Costruttore
		/// </summary>
		public BUSEmissione()
		{
		}


        /// <summary>
        /// Chiamata al backend per l'estrazione dei dati dell'intestatario
		/// </summary>
		/// <param name="codiceFiscale">Codice fiscale dell'intestatario</param>
		/// <returns></returns>
        public void RichiestaEmissione(string codiceIndividuale, ProfiloRichiesta.CertificatiRow[] certificatiRichiesti)
        {
            
            string funzione = certificatiRichiesti[0].T_SERVIZIO.Equals("SUPERNETVA")?
                MapperFunctionsNames.emissioneCertificatiVA:
                certificatiRichiesti[0].T_SERVIZIO.Equals("NETVFAM")?
                MapperFunctionsNames.emissioneCertificatiVFAM:
                MapperFunctionsNames.emissioneCertificatiVAVFAM;   
            EmissioneCertificatiRequest emiRequest = new EmissioneCertificatiRequest(); 
            emiRequest.Persona.AddPersonaRow(codiceIndividuale, "", "", "", "", "", "", "");   
            foreach (ProfiloRichiesta.CertificatiRow row in certificatiRichiesti)
            {
                emiRequest.CodiciCertificato.AddCodiciCertificatoRow(row.T_BACKEND_ID);
            }
            _emiResponse = CommonMapperRequests.ExecuteDataSet<EmissioneCertificatiResponse>(emiRequest, funzione);

        }
    }
}
