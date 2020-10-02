using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Com.Unisys.CdR.Certi.Caching
{
	/// <summary>
	/// 
	/// </summary>
	public enum SessionKeys
	{
        /// <summary>
        /// Type: Certi.Objects.ProfiloUtente
        /// Info: Chiave richiedente certificati
        /// </summary>
        RICHIEDENTE_CERTIFICATI,
        /// <summary>
        /// Type: Certi.Objects.ProfiloUtente
        /// Info: Chiave intestatario certificati
        /// </summary>
        INTESTATARIO_CERTIFICATI,
        /// <summary>
        /// Type: Certi.Objects.ProfiloUtente
        /// Info: Chiave intestatario certificati
        /// </summary>
        ESITO_PAGAMENTO,
        /// <summary>
        /// Type: System.String
        /// Info: Esito pagamento certificato
        /// </summary>
        THEME_SELEZIONATO
    }
}