using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Unisys.CdR.Certi.Objects.SIPO
{
   public class RecuperaCertificato
    {
        public string idCertificato;

        public string idIntestatario { get; set; }
            public string tipoRichiedente { get; set; }
            public string idRichiedente { get; set; }
            public string nomeRichiedente { get; set; }
            public string cognomeRichiedente { get; set; }
            public string sessoRichiedente { get; set; }
            public string dataNascitaRichiedente { get; set; }
            public string codiceFiscaleRichiedente { get; set; }
            public string codiceIndividualeRichiedente { get; set; }
            public string tipoDocumentoRichiedente { get; set; }
            public string numeroDocumentoRichiedente { get; set; }
            public string dataRilascioDocRichiedente { get; set; }
            public IList<string> idCertificatiAnpr { get; set; }
            public string flgAnagSC { get; set; }
            public string flgSempliceBollata { get; set; }
            public string idEsenzioneAnpr { get; set; }
            public string paEstera { get; set; }
            public string flgAnteprima { get; set; }
            public string hostname { get; set; }
            public string cfUser { get; set; }
        

    }
}
