using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Unisys.CdR.Certi.WebApp.baseLayoutUnisys;
using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.Objects.Common;    

namespace Com.Unisys.CdR.Certi.WebApp.common
{
    public partial class BasePage : System.Web.UI.Page
    {
        public Info info = new Info();
        protected System.Web.UI.WebControls.Panel InfoBox;
        protected ProfiloUtente richiedente;

        protected virtual void Page_PreRender(object sender, EventArgs e)
        {
            //Rendering dellla parte di portale
            if (richiedente != null)
            {
                switch (string.IsNullOrEmpty(richiedente.Cognome))
                {
                    case true:
                        ((Certificati)this.Master).LabelCodfis = String.Concat(
                        "<b>", richiedente.CodiceFiscale, "</b>");
                        break;
                    case false:
                        ((Certificati)this.Master).LabelCodfis = String.Concat(
                            "<b>", richiedente.Nome, " ", richiedente.Cognome, "</b>");
                        break;
                }
            }



            Certificati myMaster = (Certificati)this.Master;

            if ((((BasePage)Context.PreviousHandler) != null) &&
                ((BasePage)Context.PreviousHandler).info != null &&
                ((BasePage)Context.PreviousHandler).info.messageCount() > 0)
            {
                string msg = (((BasePage)Context.PreviousHandler).info).renderMessage();
                myMaster.ShowMessageList(msg, true);
            }
            else
            {
                myMaster.ShowMessageList(info.renderMessage(), (info.messageCount() > 0));
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception e0 = Context.Error;

            if (e0.GetType().Equals(typeof(Com.Unisys.Logging.ManagedException)))
            {
                Com.Unisys.Logging.ManagedException e1 = (Com.Unisys.Logging.ManagedException)e0;
                info.AddMessage("#ERR_P01: La pagina non può essere visualizzata ", LivelloMessaggio.ERROR);
                info.AddMessage("DETTAGLI: " + e1.Message,LivelloMessaggio.DETAILS);
            }
            else
            {
                info.AddMessage("#ERR_P02: La pagina non può essere visualizzata ", LivelloMessaggio.ERROR);
                info.AddMessage("DETTAGLI: " + e0.Message, LivelloMessaggio.DETAILS);
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);            
        }

        protected void LogOff() 
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect(ConfigurationManager.AppSettings["UrlLogin"]);
        }
    }
}
