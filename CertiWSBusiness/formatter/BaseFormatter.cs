using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Com.Unisys.CdR.Certi.Caching;    

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public abstract class BaseFormatter : IFormatter
    {
        protected string NestingPoint;
        protected System.Xml.XmlNamespaceManager nsmgr;

        internal XslCompiledTransform BuildLayoutXslTransform(IList<string> frags)
        {
            XmlDocument xmlDocument1 = CacheManager<XmlDocument>.get((CacheKeys)6, (VincoloType)1);
            XmlDocument xmlDocument2 = new XmlDocument();
            xmlDocument2.InnerXml = xmlDocument1.InnerXml;
            foreach (string frag in (IEnumerable<string>)frags)
            {
                XmlNode xmlNode1 = CacheManager<XmlDocument>.get((CacheKeys)Enum.Parse(typeof(CacheKeys), frag + "_TO_PDF"), (VincoloType)1).SelectSingleNode("//xsl:template[@name='" + frag + "']", this.nsmgr);
                string innerXml = xmlDocument2.DocumentElement.InnerXml;
                xmlDocument2.DocumentElement.InnerXml = innerXml + xmlNode1.OuterXml;
                XmlNode xmlNode2 = xmlDocument2.SelectSingleNode("//" + this.NestingPoint, this.nsmgr);
                xmlNode2.InnerXml = xmlNode2.InnerXml + "<xsl:call-template name='" + frag + "' />";
                XmlNode element = (XmlNode)xmlDocument2.CreateElement("xsl", "call-template");
            }
            XslCompiledTransform compiledTransform = new XslCompiledTransform();
            XmlReader stylesheet = (XmlReader)new XmlNodeReader((XmlNode)xmlDocument2);
            compiledTransform.Load(stylesheet);
            return compiledTransform;
        }


        internal System.Xml.XmlDocument BuildLayOut(IList<string> frags)
        {
            System.Xml.XmlDocument BaseLayout = CacheManager<XmlDocument>.get(CacheKeys.Base_TO_PDF, VincoloType.FILESYSTEM);
            System.Xml.XmlDocument Layout = new XmlDocument();
            Layout.InnerXml = BaseLayout.InnerXml; 

            foreach (string frag in frags)
            {
                CacheKeys key = (CacheKeys)Enum.Parse(typeof(CacheKeys), String.Concat(frag, "_TO_PDF")); 
                XmlDocument LayoutElement = CacheManager<XmlDocument>.get(key, VincoloType.FILESYSTEM);
                XmlNode singleTemplateNode = LayoutElement.SelectSingleNode(
                    string.Concat("//xsl:template[@name='", frag, "']"), nsmgr);
                string inner = Layout.DocumentElement.InnerXml;
                Layout.DocumentElement.InnerXml = string.Concat(inner, singleTemplateNode.OuterXml);

                XmlNode bodyNode = Layout.SelectSingleNode(string.Concat("//", NestingPoint), nsmgr);
                bodyNode.InnerXml = string.Concat(bodyNode.InnerXml, "<xsl:call-template name='", frag, "' />");
                XmlNode callTemplateNode = Layout.CreateElement("xsl", "call-template");

            }
            return Layout;
        }
        public abstract System.IO.MemoryStream formatData(System.Xml.XmlDocument rawData, IList<string> Fragments);       
    }
}