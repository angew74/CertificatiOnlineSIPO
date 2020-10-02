using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Unisys.CdR.Certi.Caching;

namespace Com.Unisys.CdR.Certi.WebApp
{
    public partial class UnisysPortaleCdR : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string Title
        {
            set
            {
                this.hfCenterTitle.Value = value;
            }
        }

        public string LabelCodfis
        {
            set
            {
                this.Lbl_CodFis.Text = value;
            }
        }

        public void ShowMessageList(string msg, Boolean show)
        {
            string inner = "<div class='header-panel'>";
            inner += "<div class='header-text'>";
            inner += "<div class='colonna'>";
            //inner += "<div class='header-icon' + ' ' + icon + '></div>";
            inner += "</div>";
            inner += "<div class='header-label'>";
            inner += "<label>" + "Messaggio" + "</label>";
            inner += "</div>";
            inner += "</div>";
            inner += "</div>";

            litErrore.Text = string.Empty;
            litMsgErrore.Text = msg;
            if (show)
            {
                pnlContainerMsg.Visible = show;
                litErrore.Text = inner;
            }
            else
                pnlContainerMsg.Visible = show;
        }

    //    protected void ddlTema_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        SessionManager<string>.set(SessionKeys.THEME_SELEZIONATO, ddlTema.SelectedValue);
    //        Server.Transfer(Request.FilePath);
    //    }
    }
}
