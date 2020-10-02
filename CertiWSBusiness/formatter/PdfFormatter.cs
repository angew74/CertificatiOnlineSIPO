using java.io;
using javax.xml.transform;
using javax.xml.transform.sax;
using javax.xml.transform.stream;
using org.apache.fop.apps;
using org.xml.sax;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public sealed class PdfFormatter : BaseFormatter
    {
        static readonly PdfFormatter instance = new PdfFormatter();


        static PdfFormatter()
        {

        }

        PdfFormatter()
        {
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block";
            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");
        }

        public static PdfFormatter Instance
        {
            get
            {
                return instance;
            }
        }

        //public override System.IO.MemoryStream formatDataIKVM(System.Xml.XmlDocument rawData, IList<string> Fragments)
        //{
        //    System.Xml.XmlDocument xslt = BuildLayOut(Fragments);
        //    string step = XmlUty.XsltToString(rawData, xslt);
        //    MemoryStream stream;
        //    FopFactory fopFactory = FopFactory.newInstance();
        //    OutputStream o = new DotNetOutputMemoryStream();
        //    try
        //    {
        //        Fop fop = fopFactory.newFop("application/pdf", o);
        //        TransformerFactory factory = TransformerFactory.newInstance();
        //        Transformer transformer = factory.newTransformer();
              
        //      //   StreamSource ss = new StreamSource(step);
        //        //read the template from disc
        //        Source src = new StreamSource(step);
        //        Result res = new SAXResult(fop.getDefaultHandler());
        //        transformer.transform(src, res);
        //    }
        //    finally
        //    {
        //        o.close();
        //    }
        //    using (System.IO.FileStream fs = System.IO.File.Create("HelloWorld.pdf"))
        //    {
        //        //write from the .NET MemoryStream stream to disc the generated pdf file
        //        var data = ((DotNetOutputMemoryStream)o).Stream.GetBuffer();
        //        fs.Write(data, 0, data.Length);
        //       stream = StreamUty.byteArray2MemoryStream(data);

        //    }
        //    //Engine en = new Engine();
        //    //sbyte[] pdf = en.Run(step);
        //    //return StreamUty.sbyteArray2MemoryStream(pdf);
        //    return stream;
        //}

       // public override System.IO.MemoryStream formatData(System.Xml.XmlDocument rawData, IList<string> Fragments)
       // {
            //System.Xml.XmlDocument xslt = BuildLayOut(Fragments);
            //string step = XmlUty.XsltToString(rawData, xslt);
            //Engine en = new Engine();
            //sbyte[] pdf = en.Run(step);
            //return StreamUty.sbyteArray2MemoryStream(pdf);
        //}
             public override MemoryStream formatData(XmlDocument rawData, IList<string> Fragments)
        {
            XslCompiledTransform compiledTransform = this.BuildLayoutXslTransform(Fragments);         
            MemoryStream memoryStream = new MemoryStream();
            compiledTransform.Transform((IXPathNavigable)rawData, (XsltArgumentList)null, (Stream)memoryStream);
            ByteArrayInputStream arrayInputStream = new ByteArrayInputStream(memoryStream.ToArray());
            ByteArrayOutputStream arrayOutputStream = new ByteArrayOutputStream();
            Fop fop = FopFactory.newInstance().newFop("application/pdf", (OutputStream)arrayOutputStream);
            fop.getUserAgent();
            var s = new com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl();
            var t = new com.sun.org.apache.xalan.@internal.xsltc.trax.TransformerFactoryImpl();
            TransformerFactory.newInstance().newTransformer().transform((Source)new StreamSource((InputStream)arrayInputStream), (Result)new SAXResult((ContentHandler)fop.getDefaultHandler()));
            return new MemoryStream(arrayOutputStream.toByteArray());
        }
    
    }
}
