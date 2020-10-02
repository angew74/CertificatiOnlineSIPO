using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public sealed class HtmlFormatter : BaseFormatter
    {
        static readonly HtmlFormatter instance = new HtmlFormatter();

        static HtmlFormatter()
        {

        }

        HtmlFormatter()
        {
            NestingPoint = "body";
            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

        }

        public static HtmlFormatter Instance
        {
            get
            {
                return instance;
            }
        }

        public override System.IO.MemoryStream formatData(System.Xml.XmlDocument rawData, IList<string> Fragments)
        {
            System.Xml.XmlDocument xslt = BuildLayOut(Fragments);
            //return copiaverServerEngine.Util.XmlUtil.XsltToMemoryStream(rawData, xslt);
            return null;
        }
      
    }
}
