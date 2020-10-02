using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Com.Unisys.CdR.Certi.WebApp.Business.ProxyPagamentiWS;
using log4net;
using System.Xml;
using System.Configuration;

namespace CertiBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger("CertiBatch");

            try
            {
                Console.WriteLine("Inizio della Procedura di Aggiornamento dei Certificati");
                Console.WriteLine();
                Console.WriteLine("Lettura dei Certificati con Status 19 (C_RICHIESTA_PAGAMENTO) ... ...");
                DataTable dtCertificati = OracleStore.SelectCertificatiByStatus(19);
                int countDtCertificati = dtCertificati.Rows.Count;
                Console.WriteLine("I Certificati sono: " + countDtCertificati);
                Console.WriteLine();
                Console.WriteLine("Lettura del Dettaglio dei Certificati dal Web Service: ");
                Console.WriteLine("http://10.150.130.37:6080/CDRServices/services/Pagamento_CertificatiSOAP");
        
                int i = 0;
                int countBlocco = 1;

                while (i < countDtCertificati)
                {
                    
                    String[] listaCIU = new String[10];
                    int j = 0;

                    while ((j < 10) && (i < countDtCertificati))
                    {
                        listaCIU[j] = dtCertificati.Rows[i]["CIU"].ToString();

                        j++;
                        i++;
                    }

                    Console.WriteLine();
                    Console.WriteLine("Analisi del Blocco " + countBlocco + " ... ...");

                    Pagamento_Certificati ck = new Pagamento_Certificati();
                    ck.Url = ConfigurationManager.AppSettings["UrlControlloPagamenti"];
                    System.Xml.XmlNode rp = ck.ListaTransazioni(listaCIU);

                    System.Xml.XmlNodeList xNodes = rp.SelectNodes("item");
                    
                    Console.WriteLine("Il Blocco " + countBlocco + " ha " + xNodes.Count + " Certificati Sospetti");
                    Console.WriteLine("Aggiornamento dei " + xNodes.Count + " Certificati Sospetti ... ...");

                    if (xNodes != null)
                    {
                        foreach (System.Xml.XmlNode xn in xNodes)
                        {
                            System.Xml.XmlNode nodo = xn.SelectSingleNode("esito");
                            if (!String.IsNullOrEmpty(nodo.InnerText))
                            {
                                System.Xml.XmlNode nodoIdCertificato = xn.SelectSingleNode("idCertificato");
                                if (nodo.InnerText.Equals("KO"))
                                {
                                    //OracleStore.UpdateCertificato(nodoIdCertificato.InnerText, 21);
                                    Console.WriteLine("Aggiornato il Certificato " + nodoIdCertificato.InnerText + " a Status: " + 21);
                                    log.Info("Aggiornato il Certificato " + nodoIdCertificato.InnerText + " a Status: " + 21 + " (C_RICHIESTA_PAGAMENTO_KO)");
                                }
                                else if (nodo.InnerText.Equals("OK"))
                                {
                                    String[] result = Check(rp);

                                    if (!String.IsNullOrEmpty(result[0]) && !String.IsNullOrEmpty(result[2]))
                                    {
                                        XmlDocument doc = new XmlDocument();
                                        doc.LoadXml(result[0]);
                                        XmlNode node = doc.DocumentElement;
                                        string xmlPagamento = node.SelectSingleNode("datiTransazione").OuterXml;

                                        //OracleStore.UpdateCertificato(nodoIdCertificato.InnerText, 14, xmlPagamento, result[2]); 
                                        Console.WriteLine("Aggiornato il Certificato " + nodoIdCertificato.InnerText + " a Status: " + 14);
                                        log.Info("Aggiornato il Certificato " + nodoIdCertificato.InnerText + " a Status: " + 14 + " (C_VERIFICA_EMETTIBILITA_OK)");
                                    }
                                }
                            }
                        }
                    }
                    Console.WriteLine("Aggiornamento dei Blocco " + countBlocco + " Completato");
                    countBlocco++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static String[] Check(System.Xml.XmlNode rp)
        {
            String[] result = new String[3];

            try
            {
                //Pagamento_Certificati ck = new Pagamento_Certificati();
                //ck.Url = ConfigurationManager.AppSettings["UrlControlloPagamenti");
                //System.Xml.XmlNode rp = ck.ListaTransazioni(new String[] { CIU });

                System.Xml.XmlNodeList xNodes = rp.SelectNodes("item");

                if (xNodes != null)
                {
                    foreach (System.Xml.XmlNode xn in xNodes)
                    {
                        System.Xml.XmlNode nodoEsito = xn.SelectSingleNode("esito");
                        if (!String.IsNullOrEmpty(nodoEsito.InnerText))
                        {
                            result[0] = rp.InnerXml;
                            result[1] = nodoEsito.InnerText;

                            System.Xml.XmlNodeList xNodesDatiTransazione = xn.SelectNodes("datiTransazione");

                            if (xNodesDatiTransazione != null && xNodesDatiTransazione.Count != 0)
                            {
                                //result[0] = xNodesDatiTransazione[0].OuterXml;
                                result[2] = xNodesDatiTransazione[0].SelectSingleNode("//datiPagamento/idEmissione").InnerText;

                                //System.Xml.XmlNodeList xNodesDatiPagamento = xNodesDatiTransazione[0].SelectNodes("datiPagamento");
                                //if (xNodesDatiPagamento != null)
                                //{
                                //    System.Xml.XmlNode nodoIdEmissione = xNodesDatiPagamento[0].SelectSingleNode("idEmissione");
                                //    if (!String.IsNullOrEmpty(nodoIdEmissione.InnerText))
                                //    {
                                //        result[2] = nodoIdEmissione.InnerText;
                                //    }
                                //}
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
