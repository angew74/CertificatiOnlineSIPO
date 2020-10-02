using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;      

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class PdfModifier
    {
        public static byte[] SetMetadati(System.IO.MemoryStream inputPdf, IDictionary<string, string> md)
        {
            inputPdf.Position = 0;  
            iTextSharp.text.pdf.PdfReader prd = new iTextSharp.text.pdf.PdfReader(inputPdf);
            iTextSharp.text.Rectangle psize = prd.GetPageSize(1);
            Document doc = new Document(psize, 50, 50, 50, 70);
            System.IO.MemoryStream output = new System.IO.MemoryStream();
            output.Position = 0;

            iTextSharp.text.pdf.PdfWriter wr = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, output);
            doc.AddAuthor(md["author"]);
            doc.AddCreator(md["creator"]);
            doc.AddSubject(md["subject"]);
            wr.SetTagged();
 
            doc.Open();
            /*
            EndPage pageEvent = new EndPage();
            pageEvent.CIU = md["ciu"];
            pageEvent.IDUfficio = md["id_ufficio"];
            pageEvent.DocumentDate = md["data_emissione"];
 
            wr.PageEvent = pageEvent;
             */
            //wr.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
            wr.SetPdfVersion(PdfWriter.PDF_VERSION_1_6);
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
    }
}
