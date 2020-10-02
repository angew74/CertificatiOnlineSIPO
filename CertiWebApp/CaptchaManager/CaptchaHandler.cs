using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ImageKit;
using System.Drawing.Imaging;
using WaveKit;

namespace ImageKit
{
    public class CaptchaHandler : IHttpHandler, IRequiresSessionState
    {
        public static string CAPTCHA_SESSION_KEY = "CAPTCHA_SESSION_KEY";
        public static string getCaptchaText()
        {

            if (HttpContext.Current.Session[CAPTCHA_SESSION_KEY] != null)
                return (HttpContext.Current.Session["CAPTCHA_SESSION_KEY"].ToString());
            return null;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string letters = AlfabetoWav.Instance.getRandomLetters(6);
            if (context.Session[CAPTCHA_SESSION_KEY] == null)
                context.Session.Add(CAPTCHA_SESSION_KEY, letters);
            else
                context.Session[CAPTCHA_SESSION_KEY] = letters;
            ImageUtil uty = new ImageUtil(letters, 150, 48, "Unisys");
            
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            uty.Image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            context.Response.End();
        }

       
    }
}
