using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Com.Unisys.CdR.Certi.WS.Business.bus
{
    public class MapperDeserializer
    {

        // Caratteri speciali per il dialogo con Mapper (NON alterare) 
        private const char CHARTOCLEAN = '#';
        // [35] questo carattere, se presente, va tolto dai dati in uscita 
        private const char CHAR4CRLF = '§';
        // [231] questo è il CRLF nei dati in uscita 
        private const char CHAR4SPLITVAL = '|';
        // [124] divide i valori dai marcatori (tag|valore) 
        private const char CHAR4SPACE = '¤';
        // [164] lo spazio, nei dati in entrata, va sostituito con questo 


        //<remarks> Trasforma la risposta Mapper in una struttura dati XML eventualmente 
        // gerarchica a partire da un plain text formattato con separatori appositi. 
        // Invece di produrre direttamente i tag per il documento, creo degli elementi 
        // finti in un array in modo da poter avere uno o più di essi aperto per 
        // inserirvi i successivi: quando avrò terminato il giro tutti i tag saranno 
        // comunque chiusi (altrimenti i dati sono errati all'origine) e potrò caricarli 
        // in un XMLDocument o in un XMLNode (lo farà il chiamante, io restituisco uno stream STRINGA) 
        // ATTENZIONE: la utilizziamo anche da Unisys.CdR.CacheAnagrafe 
        //</remarks> 
        private static string PlainToXMLSerialized(string plainTextData, bool TraceOnFile, bool SkipSpecials)
        {

            AppSettingsReader myConfig = new AppSettingsReader();
            string mySpecialResultsElements = (string)myConfig.GetValue("SpecialResultsElements", typeof(string));
            string[] SpecialResultsElements = mySpecialResultsElements.Split(',');


            XmlDocument xmlTmp = new XmlDocument();
            string[] arrayRitorno = stripEmptyData(plainTextData, SkipSpecials);
            System.Text.StringBuilder arrayXML = new System.Text.StringBuilder();
            string tmpValue = null;
            string tmpTag = null;
            Int32 i = default(Int32);
            Int32 l = default(Int32);
            XmlNode myNode = default(XmlNode);

            l = arrayRitorno.Length - 2;
            for (i = 0; i <= l; i++)
            {
                string[] tag = arrayRitorno[i].Trim().Split(CHAR4SPLITVAL);
                tmpTag = tag[0].ToString().TrimEnd();

                // PER ORA se c'è un dato sporco saltiamo il tag!!! 
                if (!string.IsNullOrEmpty(tmpTag))
                {
                    // Faccio un cambio di case solo per i tag non sensibili 
                    if (SkipSpecials)
                    {
                        tmpValue = tag[1].ToString().TrimStart();
                    }
                    else
                    {
                        if (Array.BinarySearch(SpecialResultsElements, tmpTag, System.Collections.CaseInsensitiveComparer.DefaultInvariant) > -1)
                        {
                            tmpValue = tag[1].ToString().TrimStart();
                        }
                        else
                        {
                            // per convertire la prima lettera di ogni parola in Maiuscolo
                            //   tmpValue = Strings.StrConv(tag[1].ToString().TrimStart(), VbStrConv.ProperCase);
                            tmpValue = tag[1].Length > 0 ? (tag[1].Substring(0, 1).ToString().ToUpper() +
        (tag[1].Length > 1 ? tag[1].Substring(1, tag[1].Length - 1).ToLower() : "")) : tag[1];
                            /*
                                                      string s="abcDeF;
                          s = s.Length>0 ? (s[0].ToString().ToUpper() + 
                              (s.Length>1 ? s.Substring(1, s.Length-1).ToLower() :"") ): s;
                                                      */

                        }
                    }

                    if (tmpValue == "{")
                    {
                        arrayXML.Append("<" + tmpTag + ">");
                    }
                    else if (tmpValue == "}")
                    {
                        arrayXML.Append("</" + tmpTag + ">");
                    }
                    else
                    {
                        myNode = xmlTmp.CreateElement(tmpTag);
                        myNode.InnerText = tmpValue;
                        arrayXML.Append(myNode.OuterXml);
                    }
                }
            }

            // Aggiungo un timestamp e il nome del server per controlli e riscontri 
            // Inserisco anche le info del ClearPath per capire chi ho chiamato 
            //  System.DateTime d = System.DateTime.Now();
            /*  DateTime d = DateTime.Now;
              arrayXML.Append(string.Concat("<SystemInfo><QueryDateTime>", d.Day.ToString().PadLeft(2, '0'), "/", d.Month.ToString.PadLeft(2, '0'), "/", d.Year, " ", d.Hour.ToString.PadLeft(2, '0'), ":", d.Minute.ToString.PadLeft(2, '0'),
              ":", d.Second.ToString().PadLeft(2, '0'), "</QueryDateTime></SystemInfo>"));
              */
            try
            {
                // 6/10/2006: restituiamo una stringa, sarà il chiamante a caricarla 
                // in un documento o in un nodo; la modifica è necessaria poiché questo metodo 
                // viene ora utilizzato anche durante l'elaborazione dei dati dalla cache (Oracle) 
                //xmlTmp.LoadXml("<OpenTI>" & arrayXML.ToString() & "</OpenTI>") 
                //lk 29/11/2006 
                //l'elemento root lo decide il chiamante 
                //Return "<OpenTI>" & arrayXML.ToString() & "</OpenTI>" 
                return "<OpenTI>" + arrayXML.ToString() + "</OpenTI>";
            }
            catch (System.Xml.XmlException)
            {
                if (TraceOnFile)
                {
                    //  BrutalLog.WriteOnDisk("Caratteri illegali nello stream dati di ritorno da BIS. Il documento XML di risposta non può essere creato.");
                }
                throw new Exception("Errore imprevisto durante la elaborazione dei dati di ritorno dal sistema centrale.");
            }

        }
        // 
        // Rimuove i caratteri sporchi, elimina i TAG vuoti (senza valori) e mi restituisce un array 
        // spacchettando i TAG rimasti..... 
        private static string[] stripEmptyData(string Dati, bool SkipSpecials)
        {
            if (SkipSpecials)
            {
                return Dati.Replace(CHARTOCLEAN, ' ').Split(CHAR4CRLF);
            }
            else
            {
                // I TAG privi di valori li eliminiamo dallo stream restituito da Mapper: usiamo una RegExp per questo 
                System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(string.Concat("(", MapperDeserializer.CHAR4CRLF, "[A-Z]*\\", MapperDeserializer.CHAR4SPLITVAL, MapperDeserializer.CHAR4CRLF, ")"), System.Text.RegularExpressions.RegexOptions.Compiled);
                string t = Dati.Replace(CHARTOCLEAN, ' ');
                while (re.IsMatch(t))
                {
                    string b = CHAR4CRLF.ToString();
                    t = re.Replace(t, b);
                }
                return t.Split(CHAR4CRLF);
            }
        }

        public static System.Xml.XmlDocument PlainToVerificaXML(string plainTextData, XslCompiledTransform xsl)
        {
            //string s = PlainToXMLSerialized(plainTextData, false, false);
            string s = PlainToXMLSerialized(plainTextData, false, true);


            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(s);
            //    return xdoc;


            /*
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            StreamReader rd = new StreamReader(ms);
            xsl.Transform(xdoc, writer);   // forse non mi serve
            ms.Position = 0;
            string strHtml = rd.ReadToEnd();
            rd.Close();
            ms.Close();
            */

            /*INIZIO MODIFICA*/

            MemoryStream ms = new MemoryStream();

            //Eseguo la trasformazione e metto il risultato in un flusso MemoryStream
            xsl.Transform(xdoc, null, ms);

            //devo settare queste proprietà
            ms.Flush();
            ms.Position = 0;

            //Creo l'oggetto StreamReader che legge il MemoryStream e SPECIFICO LA CODIFICA
            StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("UTF-8"));

            //Prendo la stringa
            string strHtml = sr.ReadToEnd();

            /*FINE MODIFICA*/

            XmlDocument xmlCut = new XmlDocument();
            xmlCut.LoadXml(strHtml);
            return xmlCut;

        }





    }
}
