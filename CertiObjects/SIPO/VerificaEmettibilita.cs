using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Unisys.CdR.Certi.Objects.SIPO
{
    public class VerificaEmettibilita
    {
        public bool IsEmettibile { get; set; }
        public byte[] Certificato { get; set; }
        public object Risposta { get; set; }
        public string CIU { get; set; }
     
    }
}
