using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.DataObjects.CertificatiAnagrafici;
using Com.Unisys.CdR.Certi.Objects;    
using Com.Unisys.CdR.Certi.WS.Dati;     
using log4net;

namespace Com.Unisys.CdR.Certi.WS.Business 
{
    public class BUSContabilizzazione
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(BUSContabilizzazione));
        ContabilizzazioneCertificatiResponse  _contResponse = new ContabilizzazioneCertificatiResponse(); 
 
        #region Class Properties


        public ContabilizzazioneCertificatiResponse  ContResponse
        {
            get { return _contResponse; }
            set { _contResponse = value; }
        }

		#endregion

        /// <summary>
		/// Costruttore
		/// </summary>
        public BUSContabilizzazione()
		{
		}


        /// <summary>
        /// Chiamata al backend per la contabilizzazione dei certificati emessi
        /// </summary>
        /// <param name="certificatiRichiesti">lista righe con i dati dei certificati richiesti</param>
        public void RichiestaContabilizzazione(ProfiloRichiesta.CertificatiRow[] certificatiRichiesti)
        {
            string funzione = MapperFunctionsNames.contabilizzazione;
            ContabilizzazioneCertificatiRequest contRequest = new ContabilizzazioneCertificatiRequest(); 
            foreach (ProfiloRichiesta.CertificatiRow row in certificatiRichiesti)
            {
                contRequest.CodiciCertificato.AddCodiciCertificatoRow(row.T_BACKEND_ID);
            }
            _contResponse = CommonMapperRequests.ExecuteDataSet<ContabilizzazioneCertificatiResponse>(contRequest, funzione);

        }
    }
}
