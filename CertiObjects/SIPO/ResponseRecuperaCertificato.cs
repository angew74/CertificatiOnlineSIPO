using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Unisys.CdR.Certi.Objects.SIPO
{

    //   Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ResponseRecuperaCertificato
    {
        public bool esito { get; set; }
        public string idPraticaCertificati { get; set; }
        public string idCertificatoAggiunto { get; set; }
        public object risposta { get; set; }
        public byte[] certificato { get; set; }
    }
}

