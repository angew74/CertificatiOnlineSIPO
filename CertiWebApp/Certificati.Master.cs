using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Com.Unisys.CdR.Certi.WebApp
{
    public partial class Certificati : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
        }

        public string LabelCodfis
        {
            set
            {
                this.Lbl_CodFis.InnerHtml = value;
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
    }
}