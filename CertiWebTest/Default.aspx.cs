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
using ApacheFop;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf; 
using System.Xml;
using System.Text;
using System.Net;
using System.IO;

namespace CertiWebTest
{
    public partial class _Default : System.Web.UI.Page
    {
        private string ResourcesBasePath;
        private string NestingPoint;
        private System.Xml.XmlNamespaceManager nsmgr;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private System.Xml.XmlDocument BuildLayOut(System.Collections.ArrayList frags)
        {


            System.Xml.XmlDocument Layout = this.LoadXml(string.Concat(ResourcesBasePath + @"\" + "Base.xslt"));
            for (int i = 0; i < frags.Count; i++)
            {
                String path = string.Concat(ResourcesBasePath + @"\" + (string)frags[i] + ".xslt");
                XmlDocument LayoutElement = this.LoadXml(path);
                XmlNode singleTemplateNode = LayoutElement.SelectSingleNode(string.Concat("//xsl:template[@name='", frags[i], "']"), nsmgr);
                string inner = Layout.DocumentElement.InnerXml;
                Layout.DocumentElement.InnerXml = string.Concat(inner, singleTemplateNode.OuterXml);






                XmlNode bodyNode = Layout.SelectSingleNode(string.Concat("//", NestingPoint), nsmgr);
                bodyNode.InnerXml = string.Concat(bodyNode.InnerXml, "<xsl:call-template name='", (string)frags[i], "' />");

                XmlNode callTemplateNode = Layout.CreateElement("xsl", "call-template");
            }
            return Layout;
        }

        private byte[] formatData(System.Xml.XmlDocument rawData, System.Collections.ArrayList Fragments)
        {
            System.Xml.XmlDocument xslt = BuildLayOut(Fragments);
            string step = this.XsltToString(rawData, xslt);
            Engine en = new Engine();
            byte[] sbpdf = sbyteToByte(en.Run(step));
            byte[] pdf = useIText(sbpdf);
            return pdf;
        }

        private System.Xml.XmlDocument LoadXml(string path)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(path);
            return doc;
        }


        private string XsltToString(System.Xml.XmlDocument source, System.Xml.XmlDocument Xslt)
        {
            StringBuilder br = new StringBuilder();
            System.IO.StringWriter m = new System.IO.StringWriter(br);
            System.Xml.Xsl.XslTransform t = new System.Xml.Xsl.XslTransform();
            t.Load(Xslt);
            t.Transform(source, null, m);
            m.Flush();
            m.Close();
            return br.ToString();
        }

        private byte[] sbyteToByte(sbyte[] input)
        {
            byte[] output = new byte[input.Length];
            System.Buffer.BlockCopy(input, 0, output, 0, input.Length);
            return output;
        }


        private byte[] useIText(byte[] inputPdf)
        {
            iTextSharp.text.pdf.PdfReader prd = new iTextSharp.text.pdf.PdfReader(inputPdf);
            iTextSharp.text.Rectangle psize = prd.GetPageSize(1);
            Document doc = new Document(psize, 50, 50, 50, 70);
            System.IO.MemoryStream output = new System.IO.MemoryStream();
            output.Position = 0;

            iTextSharp.text.pdf.PdfWriter wr = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, output);
            doc.AddAuthor("Comune di Roma");
            doc.AddCreator("Certificati On Line");
            doc.AddSubject("Certificato di Matrimonio");
            wr.SetTagged();
            doc.Open();




            wr.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);

            PdfContentByte content = wr.DirectContent;

            for (int i = 1; i <= prd.NumberOfPages; i++)
            {
                doc.NewPage();
                PdfImportedPage pg = wr.GetImportedPage(prd, i);
                content.AddTemplate(pg, 0, 0);
            }

            doc.Close();

            return output.ToArray();
        }


        /// <summary>
        /// nascita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";
            //ResourcesBasePath = @"C:\Documents and Settings\admin.UNISYS-86F73144\Desktop\PDF CON FIRMA\xml\casistiche\NASCITA";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_Nascita.xml"); //x nascita
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_nascita.xsl" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("Nascita");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (false)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                       // System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }


        /// <summary>
        /// decesso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click1(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";
            //ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";
            //ResourcesBasePath = @"C:\Documents and Settings\admin.UNISYS-86F73144\Desktop\PDF CON FIRMA\xml\casistiche\DECESSO";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_Morte.xml"); //x matrimonio
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type='text/xsl' href='morte.xsl'?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("Morte");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }


                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }



        /// <summary>
        /// matrimonio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_Matrimonio.xml"); //x matrimonio
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "matrimonio.xsl" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("Matrimonio");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\bbb.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:47850/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);

                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }


                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);


                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_Cittadinanza.xml"); //x cittadinanza
            //doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type='text/xsl' href='0C01_comuneroma_cittadinanza.xsl'?>", doc.InnerXml);
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_cittadinanza.xsl?sha1=B63F342E33EBFC0D155C37F4932163F20160D521" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("Cittadinanza");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
           // System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertificatiOnLine\CertiWebTest\inpippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_Residenza.xml"); //x matrimonio
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type='text/xsl' href='0C01_comuneroma_residenza.xsl?sha1=132237ED13EC423255CB7974BD645211D3C8DCB0'?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("Residenza");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];



                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_Matrimonio.xml"); //x matrimonio
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type='text/xsl' href='matrimonio.xsl'?>", doc.DocumentElement.OuterXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("Matrimonio");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            byte[] pdfByteFirma = new byte[0];

            WebFormatter wf = new WebFormatter();
            System.Collections.Generic.List<PostDataParam> postParameters = new System.Collections.Generic.List<PostDataParam>();
            PostDataParam param1 = new PostDataParam("cfg", "sviluppo", PostDataParamType.Field);
            PostDataParam param2 = new PostDataParam("xml", System.Text.Encoding.UTF8.GetBytes(doc.InnerXml), PostDataParamType.File);
            PostDataParam param3 = new PostDataParam("pdf", pdfByte, PostDataParamType.File);
            postParameters.Add(param1);
            postParameters.Add(param2);
            postParameters.Add(param3);
            string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";

            try
            {

                WebResponse response = wf.PostMultipartFormData(url, postParameters);
                Stream resp = response.GetResponseStream();

                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                byte[] buffer = new byte[2048];
                int read = 0;
                do
                {
                    read = resp.Read(buffer, 0, buffer.Length);
                    mem.Write(buffer, 0, read);
                } while (read > 0);

                mem.Position = 0;
                pdfByteFirma = new byte[(int)mem.Length];
                mem.Read(pdfByteFirma, 0, (int)mem.Length);

            }
            catch (Exception ex)
            {
                ex = ex;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByteFirma);
            this.Response.End();
            return;
        }

        //stato libero

        protected void Button6_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_StatoLibero.xml"); //x stato libero
            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_residenzacittadinanzastatocivile.xsl?sha1=C4EF0EC9DE14286E6E707D8B4F47D8444DCFA909" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();
            frags.Add("Testa");
            frags.Add("StatoLibero");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                       // System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }


        // CITTADINANZA AIRE
        protected void Button7_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");


            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_CittadinanzaAIRE.xml"); //x cittadinanza aire

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "cittadinanzaaire.xsl" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("CittadinanzaAIRE");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (false)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }


        // RESIDENZA E CITTADINANZA AIRE
        protected void Button8_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");




            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaCittadinanzaAIRE.xml"); //x cittadinanza aire

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_residenzacittadaire.xsl?sha1=B63271CFCA1252B515A0EA407299FD694D683A5B" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaCittadinanzaAIRE");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
           // System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (false)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        // RESIDENZA AIRE
        protected void Button9_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";
            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");




            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaAIRE.xml"); //x cittadinanza aire

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "ResidenzaAIRE" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaAIRE_TO_PDF");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();

        }
        // RESIDENZA E STATO LIBERO
        protected void Button10_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");




            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaStatoLibero.xml"); //x cittadinanza aire


            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_residenzacittadinanzastatocivile.xsl?sha1=C4EF0EC9DE14286E6E707D8B4F47D8444DCFA909" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaStatoLibero");   
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);   


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        //RESIDENZA CITTADINANZA E STATO LIBERO
        protected void Button11_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");




            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaCittadinanzaStatoLibero.xml"); //x cittadinanza aire

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_residenzacittadinanzastatocivile.xsl?sha1=C4EF0EC9DE14286E6E707D8B4F47D8444DCFA909" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaCittadinanzaStatoLibero");   
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);   


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }
        //RESIDENZA CITTADINANZA
        protected void Button13_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");




            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaCittadinanza.xml"); //x cittadinanza aire

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_residenzacittadinanza.xsl?sha1=9B371DAFC22498F10983EF90A8E778ED95361D57" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaCittadinanza");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }


        protected void Button14_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");




            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_StatoDiFamiglia.xml"); //x stato di famiglia

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_statofamiglia.xsl" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();


                

            frags.Add("Testa");
            frags.Add("StatoDiFamiglia_TO_PDF");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";

                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button15_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");





            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_GodimentoDirittiPolitici.xml"); //x godimento diritti politici

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "GodimentoDirittiPolitici" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("GodimentoDirittiPolitici_TO_PDF");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");





            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaCittadinanzaStatoCivileNascitaDirittiPolitici.xml"); //x CONT

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_residenzacittadinanzastatocivile.xsl?sha1=C4EF0EC9DE14286E6E707D8B4F47D8444DCFA909" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaCittadinanzaStatoCivileNascitaDirittiPolitici");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button16_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; ;

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform"); 
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");





            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_StatoDiFamigliaAire.xml"); //x stato famiglia aire

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_statofamigliaAIRE.xsl" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("StatoDiFamigliaAIRE_TO_PDF");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        pdfByteFirma = new byte[(int)response.ContentLength];

                        BinaryReader rr = new BinaryReader(resp);

                        for (long ii = 0; ii < response.ContentLength; ii++)
                        {
                            pdfByteFirma[ii] = rr.ReadByte();

                        }

                        System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }

        protected void Button17_Click(object sender, EventArgs e)
        {
            ResourcesBasePath = @"C:\dev\CertificatiOnLine\CertificatiOnLine\Test\xmlXAlberto";


            NestingPoint = "fo:flow[@flow-name='xsl-region-body']/fo:block"; 

            nsmgr = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());

            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            nsmgr.AddNamespace("fo", "http://www.w3.org/1999/XSL/Format");





            System.Xml.XmlDocument doc = this.LoadXml(ResourcesBasePath + @"\Test_ResidenzaCittadinanzaStatoCivileNascitaDirittiPoliticiStatoFamiglia.xml"); //x CTSF

            doc.InnerXml = String.Concat("<?xml version='1.0' encoding='utf-8'?><?xml-stylesheet type=" + Convert.ToChar(34) + "text/xsl" + Convert.ToChar(34) + " href=" + Convert.ToChar(34) + "0C01_comuneroma_statofamigliaALL.xsl?sha1=ED4301E7F7AD99D205DDD5F927ED06B013D659F8" + Convert.ToChar(34) + "?>", doc.InnerXml);
            System.Collections.ArrayList frags = new System.Collections.ArrayList();




            frags.Add("Testa");
            frags.Add("ResidenzaCittadinanzaStatoCivileNascitaDirittiPoliticiStatodiFamiglia");
            frags.Add("Coda");

            byte[] pdfByte = formatData(doc, frags);
            //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\inppippo.pdf", pdfByte);


            if (true)
            {
                byte[] pdfByteFirma = new byte[0];




                string url = "http://10.150.110.100/ReportPresenzeAsilinido/timbri/getTimbro/";
                //string url = "http://localhost:2140/Default.aspx";
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
                string contenttype = "multipart/form-data; boundary=" + boundary;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contenttype;

                byte[] bound = System.Text.Encoding.UTF8.GetBytes(boundary);
                string header0 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"cfg\"\r\n\r\nsviluppo\r\n", boundary);
                byte[] cfgHeaderByte = System.Text.Encoding.UTF8.GetBytes(header0);

                string header1 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"xml\"; filename=\"xml\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] xmlHeaderByte = System.Text.Encoding.UTF8.GetBytes(header1);
                byte[] xmlByte = System.Text.Encoding.UTF8.GetBytes(doc.InnerXml);

                string header2 = string.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"pdf\"; filename=\"pdf\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary);
                byte[] pdfHeaderByte = System.Text.Encoding.UTF8.GetBytes(header2);

                string footer = "\r\n--" + boundary + "--\r\n";
                byte[] footerByte = System.Text.Encoding.UTF8.GetBytes(footer);


                request.ContentLength = cfgHeaderByte.Length
                    + xmlHeaderByte.Length
                    + xmlByte.Length
                    + pdfHeaderByte.Length
                    + pdfByte.Length
                    + footerByte.Length;

                bool esito = true;
                try
                {
                    Stream sendStream = request.GetRequestStream();

                    sendStream.Write(cfgHeaderByte, 0, cfgHeaderByte.Length);
                    sendStream.Write(xmlHeaderByte, 0, xmlHeaderByte.Length);
                    sendStream.Write(xmlByte, 0, xmlByte.Length);
                    sendStream.Write(pdfHeaderByte, 0, pdfHeaderByte.Length);
                    sendStream.Write(pdfByte, 0, pdfByte.Length);
                    sendStream.Write(footerByte, 0, footerByte.Length);
                    sendStream.Flush();
                    sendStream.Close();

                    WebResponse response = request.GetResponse();
                    if (request.HaveResponse)
                    {
                        Stream resp = response.GetResponseStream();


                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        byte[] buffer = new byte[2048];
                        int read = 0;
                        do
                        {
                            read = resp.Read(buffer, 0, buffer.Length);
                            mem.Write(buffer, 0, read);
                        } while (read > 0);

                        mem.Position = 0;
                        pdfByteFirma = new byte[(int)mem.Length];
                        mem.Read(pdfByteFirma, 0, (int)mem.Length);

                        //System.IO.File.WriteAllBytes(@"C:\dev\CertificatiOnLine\CertiWebTest\pippo.pdf", pdfByteFirma);



                        resp.Close();
                        response.Close();

                        esito = true;
                    }
                    else
                    {
                        TextBox1.Text = "cHIAMATA senza risposta " + esito;
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text = ex.Message;
                }
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
                this.Response.BinaryWrite(pdfByteFirma);
                this.Response.End();
                return;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/pdf";
            this.Response.AppendHeader("Content-Disposition", "attachment; filename=doc_test.pdf");
            this.Response.BinaryWrite(pdfByte);
            this.Response.End();
        }
        }
    }


