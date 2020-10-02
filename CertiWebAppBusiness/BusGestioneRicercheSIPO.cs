using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.Objects.SIPO;
using Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS;
using Com.Unisys.CdR.Certi.WebApp.Business.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;

namespace Com.Unisys.CdR.Certi.WebApp.Business
{
    public class BusGestioneRicercheSIPO
    {

       
        public static NCRIRICIND FindByDatiAnagrafici(string Nome, string Cognome,
            string AnnoNascita, string MeseNascita, string GiornoNascita, string Sesso, string cfrichiedente)
        {
            NCRIRICIND risposta = new NCRIRICIND();
            ResponseRichiestaToken r = new ResponseRichiestaToken();
            string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];
            string AuthRequest = "BASIC " + SerializationUtils.ToBase64Encode(utenzadominio);
            string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
            SIPORequest sipoRequestToken = new SIPORequest(RichiestaToken, "RichiestaToken", AuthRequest, "", cfrichiedente);
            r = sipoRequestToken.CallingRichiestaToken(cfrichiedente);
            string accesstoken = r.access_token;
            RicercaPosAnag ricercaPosAnag = new RicercaPosAnag();
            ricercaPosAnag.cognome = Cognome;
            ricercaPosAnag.nome = Nome;
            ricercaPosAnag.dataDiNascita = GiornoNascita + "/" + MeseNascita + "/" + AnnoNascita;
            ricercaPosAnag.sesso = Sesso;
            ricercaPosAnag.hostname = ConfigurationManager.AppSettings["hostname"];
            ricercaPosAnag.cfUser = ConfigurationManager.AppSettings["cfuser"];
            string ricerca = JsonConvert.SerializeObject(ricercaPosAnag);
            string AccessTokenRicerca = "bearer " + accesstoken;
            string serviceRicerca = ConfigurationManager.AppSettings["ServiceRicercaPosAnag"];
            SIPORequest sipoRequest = new SIPORequest(serviceRicerca, "RicercaPosAnag", AccessTokenRicerca, ricerca, cfrichiedente);
            List<MyArray> myArrays = sipoRequest.CallingRicercaPosizione(cfrichiedente);
            if (myArrays.Count == 0)
            {
                risposta.Messaggi.AddMessaggiRow("1", "2", "Nessun riscontro alla ricerca");
            }
            if (myArrays.Count == 1 && myArrays[0].famigliaConvivenza == null)
            {
                risposta.PersonaElenco.AddPersonaElencoRow("", "", myArrays[0].idSoggetto.ToString(), myArrays[0].sesso,
                      myArrays[0].cognome, myArrays[0].nome, myArrays[0].nascita.dataEvento, "", "", myArrays[0].codiceFiscale, null);
            }
            else
            {
                var ls = myArrays.Where(x => x.famigliaConvivenza == null).ToList();
                var l = myArrays.Where(x=>x.famigliaConvivenza != null).OrderBy(x => x.famigliaConvivenza.idFamigliaConv).ToList();
                int codiceFamiglia = 0;
                foreach (MyArray my in l)
                {
                    if (my.famigliaConvivenza.idFamigliaConv != codiceFamiglia)
                    {
                        risposta.PersonaElenco.AddPersonaElencoRow("", "", my.idSoggetto.ToString(), my.sesso,
                           my.cognome, my.nome, my.nascita.dataEvento, my.famigliaConvivenza.codiceFamiglia, "", my.codiceFiscale, null);
                        codiceFamiglia = (int)my.famigliaConvivenza.idFamigliaConv;
                    }
                }
                foreach(MyArray my in ls)
                {
                    risposta.PersonaElenco.AddPersonaElencoRow("", "", my.idSoggetto.ToString(), my.sesso,
                    my.cognome, my.nome, my.nascita.dataEvento, "", "", my.codiceFiscale, null);
                }
            }
            risposta.Elenco.AddElencoRow("1", risposta.PersonaElenco.Rows.Count.ToString(), risposta.PersonaElenco.Rows.Count.ToString());
            return risposta;
        }

        public static ComponenteFamigliaType[] FindComponenti(string codFiscale)
        {
            ComponenteFamigliaType[] componenteFamiglias = null;
            ResponseRichiestaToken r = new ResponseRichiestaToken();
            string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];
            string AuthRequest = "BASIC " + SerializationUtils.ToBase64Encode(utenzadominio);
            string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
            SIPORequest sipoRequestToken = new SIPORequest(RichiestaToken, "RichiestaToken", AuthRequest, "", codFiscale);
            r = sipoRequestToken.CallingRichiestaToken(codFiscale);
            string accesstoken = r.access_token;
            RicercaPosAnag ricercaPosAnag = new RicercaPosAnag();
            ricercaPosAnag.codiceFiscale = codFiscale;
            ricercaPosAnag.hostname = ConfigurationManager.AppSettings["hostname"];
            ricercaPosAnag.cfUser = ConfigurationManager.AppSettings["cfuser"];
            ricercaPosAnag.flagOrdineProfessionale = "N";
            string ricerca = JsonConvert.SerializeObject(ricercaPosAnag);
            string AccessTokenRicerca = "bearer " + accesstoken;
            string serviceRicerca = ConfigurationManager.AppSettings["ServiceRicercaPosAnag"];
            SIPORequest sipoRequest = new SIPORequest(serviceRicerca, "RicercaPosAnag", AccessTokenRicerca, ricerca, codFiscale);
            List<MyArray> myArrays = sipoRequest.CallingRicercaPosizione(codFiscale);
            List<ComponenteFamigliaType> l = new List<ComponenteFamigliaType>();
            for (int i = 0; i < myArrays.Count; i++)
            {
                ComponenteFamigliaType componenteFamigliaType = new ComponenteFamigliaType();
                componenteFamigliaType.codiceFiscale = myArrays[i].codiceFiscale;
                componenteFamigliaType.codiceIndividuale = myArrays[i].idSoggetto.ToString();
                componenteFamigliaType.nome = myArrays[i].nome;
                componenteFamigliaType.cognome = myArrays[i].cognome;
                componenteFamigliaType.rapportoParentela = myArrays[i].confCodiceLegameFamigliaConv.descrizione;
                l.Add(componenteFamigliaType);
            }
            componenteFamiglias = l.ToArray();
            return componenteFamiglias;
        }

        public static NCRIRICIND FindByCodiceFiscale(string codFiscale, string cfrichiedente)
        {
            NCRIRICIND risposta = new NCRIRICIND();
            ResponseRichiestaToken r = new ResponseRichiestaToken();
            string utenzadominio = ConfigurationManager.AppSettings["utenzadominiotoken"];
            string AuthRequest = "BASIC " + SerializationUtils.ToBase64Encode(utenzadominio);
            string RichiestaToken = ConfigurationManager.AppSettings["ServiceRichiestaToken"] + "username=" + ConfigurationManager.AppSettings["usernamedomain"] + "&password=" + ConfigurationManager.AppSettings["passworddomain"] + "&grant_type=" + ConfigurationManager.AppSettings["grant_type"];
            SIPORequest sipoRequestToken = new SIPORequest(RichiestaToken, "RichiestaToken", AuthRequest, "", cfrichiedente);
            r = sipoRequestToken.CallingRichiestaToken(cfrichiedente);
            string accesstoken = r.access_token;
            RicercaPosAnag ricercaPosAnag = new RicercaPosAnag();
            ricercaPosAnag.codiceFiscale = codFiscale;
            ricercaPosAnag.hostname = ConfigurationManager.AppSettings["hostname"];
            ricercaPosAnag.cfUser = ConfigurationManager.AppSettings["cfuser"];
            string ricerca = JsonConvert.SerializeObject(ricercaPosAnag);
            string AccessTokenRicerca = "bearer " + accesstoken;
            string serviceRicerca = ConfigurationManager.AppSettings["ServiceRicercaPosAnag"];
            SIPORequest sipoRequest = new SIPORequest(serviceRicerca, "RicercaPosAnag", AccessTokenRicerca, ricerca, cfrichiedente);
            List<MyArray> myArrays = sipoRequest.CallingRicercaPosizione(cfrichiedente);
            if(myArrays.Count == 0)
            {
                risposta.Messaggi.AddMessaggiRow("2", "2", "Nessun riscontro alla ricerca");
            }
            if (myArrays.Count == 1 && myArrays[0].famigliaConvivenza == null)
            {
                risposta.PersonaElenco.AddPersonaElencoRow("", "", myArrays[0].idSoggetto.ToString(), myArrays[0].sesso,
                   myArrays[0].cognome, myArrays[0].nome, myArrays[0].nascita.dataEvento, "", "", myArrays[0].codiceFiscale, null);
            }
            else
            {
                MyArray my = myArrays[0];     
                risposta.PersonaElenco.AddPersonaElencoRow("", "", my.idSoggetto.ToString(), my.sesso,
                   my.cognome, my.nome, my.nascita.dataEvento, my.famigliaConvivenza.codiceFamiglia, "", my.codiceFiscale, null);
            }
            risposta.Elenco.AddElencoRow("1", "1", "1");
            return risposta;
        }
    }
}
