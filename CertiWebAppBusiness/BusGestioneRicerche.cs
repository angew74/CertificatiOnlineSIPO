using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Configuration;
using Com.Unisys.CdR.Certi.WebApp.Business.Utility;

namespace Com.Unisys.CdR.Certi.WebApp.Business
{
    public class BusGestioneRicerche
    {

        public static NCRIRICIND PersonaElenco(string AnnoPratica, string NumeroPratica, string CodiceIndiv,
               string SessoPersona, string CognomePersona, string NomePersona, string DataDiNascitaPersona,
               string CodiceFamiglia, string Descrizione, string codiceFiscale)
        {
            NCRIRICIND resp = new NCRIRICIND();
            resp.PersonaElenco.AddPersonaElencoRow(AnnoPratica, NumeroPratica, CodiceIndiv, SessoPersona,
                    CognomePersona, NomePersona, DataDiNascitaPersona, CodiceFamiglia, Descrizione, codiceFiscale, null);
            return resp;
        }        
    
   
      

    }
}
