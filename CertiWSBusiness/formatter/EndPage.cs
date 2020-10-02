using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Com.Unisys.CdR.Certi.WS.Business
{
      public class EndPage : PdfPageEventHelper
        {
            public String CIU;
            public String DocumentDate;
            public String IDUfficio;

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                PdfPTable footer = new PdfPTable(2);

                PdfPCell cell1 = new PdfPCell(new Phrase("ID Certificato: " + this.CIU));
                cell1.Phrase.Font.Size = 8;
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.PaddingBottom = 2;
                cell1.BorderColor =  BaseColor.WHITE;

                PdfPCell cell2 = new PdfPCell(new Phrase("Questo certificato ha valore legale per sei mesi a partire dalla data di emissione"));
                cell2.Phrase.Font.Size = 8;
                cell2.BorderColor = BaseColor.WHITE;
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.PaddingBottom = 2;


                PdfPCell cell3 = new PdfPCell(new Phrase("ID Ufficio: " + this.IDUfficio));
                cell3.Phrase.Font.Size = 8;
                cell3.BorderColor = BaseColor.WHITE;
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3.PaddingBottom = 2;

                PdfPCell cell4 = new PdfPCell(new Phrase("Documento generato il " + this.DocumentDate));
                cell4.Phrase.Font.Size = 8;
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.PaddingBottom = 2;
                cell4.BorderColor = BaseColor.WHITE;


                footer.AddCell(cell1);
                footer.AddCell(cell2);
                footer.AddCell(cell3);
                footer.AddCell(cell4);


                float tot = document.PageSize.Width -30 - 44;
                float[] a = new float[2];
                a[0] = tot / 3;
                a[1] = 2 * tot / 3;
                footer.SetTotalWidth(a);
                footer.WriteSelectedRows(0, -1, 30, document.BottomMargin - 20, writer.DirectContent);
            }
        }
    }
