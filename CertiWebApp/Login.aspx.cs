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


public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["TEST"] == "false")
        {
            Response.Redirect("~/emissione/Emissione.aspx");
        }
    }

    protected void Accedi_Click(object sender, EventArgs e)
    {

        string testavv = System.Configuration.ConfigurationManager.AppSettings["TEST_ACCOUNTAVV"];
        string testport = System.Configuration.ConfigurationManager.AppSettings["TEST_ACCOUNTPORT"];

        // if (username.Text.Trim() == testavv.Trim() || username.Text.Trim() == testport.Trim())
        // {
        Session["TEST"] = username.Text;
        Response.Redirect("~/emissione/Emissione.aspx");

        //  }
        //  else
        // {

        // }
    }
}

