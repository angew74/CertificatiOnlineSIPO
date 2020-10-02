using System;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Utils;    

namespace Com.Unisys.CdR.Certi.WebApp
{
    public class PagamentiHandler : IHttpHandler, IRequiresSessionState 
    {
        private string _esito = "KO";
        private string _retCode = "";

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Form["esito"] != null)
            {
                _esito = context.Request.Form["esito"];
            }
            else
            {
                _esito = context.Request.QueryString.Get("esito");//1:OK - 0:KO
                //valori possibili di retCode:
                //CONCLUSO:Pagamento concluso correttamente
                //ABORT: L’utente non ha concluso il pagamento
                //ERROR: Si è verificato un errore durante il pagamento
                _retCode = context.Request.QueryString.Get("retCode");
            }

            SessionManager<String>.set(SessionKeys.ESITO_PAGAMENTO, _esito);

            switch (_retCode)
            {
                case "CONCLUSO":
                    context.Response.Redirect(ConfigurationManager.AppSettings["HandlerPagamentoOK"]);
                    break;
                case "ERROR":
                    context.Response.Redirect(ConfigurationManager.AppSettings["HandlerPagamentoKO"]);
                    break;
                case "ABORT":
                    context.Response.Redirect(ConfigurationManager.AppSettings["HandlerPagamentoKO"]);
                    break;
                default:
                    context.Response.Redirect(ConfigurationManager.AppSettings["HandlerPagamentoDefault"]);
                    break;
            }

        }
    }
}
