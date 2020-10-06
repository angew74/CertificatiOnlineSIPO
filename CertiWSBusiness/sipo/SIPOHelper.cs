using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business.sipo
{
    public class SIPOHelper
    {
        internal static List<string> GetCertificatoSIPO(string idcertificato)
        {
            List<string> ids = new List<string>();
            switch(idcertificato)
            {
                case "1":
                case "C0001":
                    ids.Add("1");
                    break;
                case "2":
                case "C0002":
                    ids.Add("3");
                    break;
                case "C0003":
                case "3":
                    ids.Add("2");
                    break;
                case "C0004":
                case "C0005":
                case "4":
                case "5":
                    ids.Add("5");
                    break;
                case "C0006":
                case "6":
                    ids.Add("8");
                    break;
                case "C0007":
                case "7":
                    ids.Add("10");
                    break;
                case "C0008":              
                case "8":                
                    ids.Add("12");
                    break;
                case "C0009":
                case "9":
                    ids.Add("15");
                    break;
                case "10":
                case "C0010":
                    ids.Add("21");
                    break;
                case "11":
                case "C0011":
                    ids.Add("17");
                    break;
                case "12":               
                case "C0012":               
                    ids.Add("5");
                    ids.Add("8");
                    break;
                case "13":
                case "C0013":
                    ids.Add("5");
                    ids.Add("10");
                    break;
                case "14":
                case "C0014":
                    ids.Add("17");
                    ids.Add("8");
                    break;
                case "15":
                case "C0015":
                    ids.Add("17");
                    ids.Add("5");
                    ids.Add("8");
                    break;
                case "16":
                case "C0016":
                    ids.Add("11");
                    ids.Add("1");
                    ids.Add("5");
                    ids.Add("8");
                    break;
                case "17":
                case "C0017":
                    ids.Add("11");
                    ids.Add("12");
                    ids.Add("1");
                    ids.Add("5");
                    ids.Add("8");
                    break;
                case "20":
                case "C0020":
                    ids.Add("22");
                    break;

            }

            return ids;
        }

        internal static string GetEsenzioneSIPO(string esenzioneId)
        {
            string id = "";
            switch (esenzioneId)
            {
                case "30":
                    id = "2";
                    break;
                case "14":
                    id = "6";
                    break;
                case "24":
                    id = "7";
                    break;
            }

            return id;
        }
    }
}
