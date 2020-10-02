using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    class FormatterProvider
    {

        public static IFormatter formatDocument(String FormatName)
        {
            if (FormatName == "HTML") return HtmlFormatter.Instance;
            else if (FormatName == "PDF") return PdfFormatter.Instance;
            else return null;

        }
    }
}
