using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.Utils
{
    public class GenericUty
    {
        //senza diritti di segreteria
        public static bool IsFree(string certificato, bool IsDescrizione)
        {
            bool resp = false;
            switch (IsDescrizione)
            { 
                case true:
                    if (certificato.ToUpper().Equals("NASCITA") 
                        || 
                        certificato.ToUpper().Equals("MATRIMONIO") 
                        || 
                        certificato.ToUpper().Equals("DECESSO"))
                        resp = true;
                    break;
                case false:
                    if (certificato.ToUpper().Equals("C0001") 
                        || certificato.ToUpper().Equals("C0002")
                        || certificato.ToUpper().Equals("C0003"))
                        resp = true;
                    break;
            }
            return resp;
        }
    }
}
