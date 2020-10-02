using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.WebApp.Business.Utility
{
    static class ExceptionUtils
    {
        public static string GetMessageFromSOAPException(string codiceErrore)
        {
            string message = string.Empty;

            ListaErroriSOAP errors = CacheManager<ListaErroriSOAP>.get(CacheKeys.LISTA_ERRORI_SOAP, VincoloType.FILESYSTEM);
            if (errors.Errore.Rows.Count != 0)
            {
                ListaErroriSOAP.ErroreRow[] rows = errors.Errore.Select(
                    "InternalError = '" + codiceErrore + "'") as ListaErroriSOAP.ErroreRow[] ;
                if (rows.Length != 0)
                {
                    message = rows[0].InternalDescription;
                }
            }
            return message;
        }
    }
}
