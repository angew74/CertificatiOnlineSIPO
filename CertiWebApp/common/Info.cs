using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.WebApp.baseLayoutUnisys
{
	public class Info
	{
		//public enum mtype
		//{
		//    info,
		//    warning,
		//    error,
		//    details
		//}

		private class message
		{
            public message(string message, LivelloMessaggio tipo)
			{
				this.tipo = tipo;
				this.msg = message;
			}
            public LivelloMessaggio tipo;
			public string msg;
		}

		private IList<message> messageList = new List<message>();

        public void AddMessage(string message, LivelloMessaggio t)
		{
			messageList.Add(new message(message, t));
		}

        public void AddMessage(DataTable table, LivelloMessaggio l)
		{
			foreach (DataRow row in table.Rows)
			{
				string error = string.Format("Errore {0}: {1}",
					row["Codice"].ToString(),row["Descrizione"].ToString());
				messageList.Add(new message(error, l));
			}
		}

		public string renderMessage()
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder("<ul>");

			foreach (message m in messageList)
			{
				s.Append("<li style='font-weight:bold;color:#8e001c'>").Append(m.msg).Append("</li>");
			}
			s.Append("</ul>");
			return s.ToString();
		}

		public int messageCount()
		{
			return messageList.Count;
		}
	}

	public class InfoMapper : Info
	{
		public void addMessage() { }
		public void addMessage(string x, int r) { }
	}
}
