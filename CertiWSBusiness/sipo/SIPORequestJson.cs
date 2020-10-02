using Com.Unisys.CdR.Certi.Objects.SIPO;
using Com.Unisys.Logging;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business.sipo
{
    public class SIPORequestJson
    {
        private static readonly ILog log = LogManager.GetLogger("SIPORequest");
        private HttpWebRequest request = null;



        public SIPORequestJson(string url, string service, string Authorization, string Data, string cf)
        {
            CreateObject(url, service, Authorization, Data, cf);
        }
        private void CreateObject(string url, string s, string authorization, string data, string cf)
        {
            // System.Diagnostics.HttpRawTraceListener.Initialize();
            request = (HttpWebRequest)WebRequest.Create(url);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            log.Debug("sto prima di use proxy");
            if (ConfigurationManager.AppSettings["useproxy"] == "1")
            {
                request.Proxy = new WebProxy(ConfigurationManager.AppSettings["ipproxy"], int.Parse(ConfigurationManager.AppSettings["portproxy"]));
                log.Debug("dentro use proxy");
            }
            request.Method = "POST";
            try
            {
                log.Debug("sto per chiamare");
                JsonSerializer json = new JsonSerializer();
                request.ContentType = "application/json";
                request.UserAgent = "Apache-HttpClient/4.1.1";
                request.Headers.Add("Authorization", authorization);
                request.PreAuthenticate = true;
                string ticks = System.DateTime.Now.Ticks.ToString();
                StringBuilder body = new StringBuilder();
                if (!string.IsNullOrEmpty(data))
                {
                    body.Append(data);
                    byte[] byteArray = Encoding.UTF8.GetBytes(body.ToString());
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
             
               
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + ex.Message,
                           "426",
                           "Certi.WebApp.Business.SIPORequest",
                           "CreateObject",
                           "Preparazione Rest",
                           "Service: " + s,
                           "ActiveObjectCF: " + cf,
                           ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }

        public ResponseRecuperaCertificato CallingRecuperaCertificato(string id)
        {
            ResponseRecuperaCertificato r = null;
            string message = "Errore nel collegamento con ANPR per il recupero del Certificato";
            try
            {
                WebResponse webResponse = request.GetResponse();
                HttpStatusCode code = ((HttpWebResponse)webResponse).StatusCode;
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                r = JsonConvert.DeserializeObject<ResponseRecuperaCertificato>(response);
                if (r == null)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                     "ERR_427",
                     "Certi.WebApp.Business.SIPORequest",
                     "Calling",
                     "Invocazione rest",
                     "Service: " + request.RequestUri + " richiesta: " + id,
                     message,
                      null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                    log.Error(error);
                    throw mex;
                }


            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                   "ERR_453",
                   "Certi.WebApp.Business.CreateClientRequest",
                   "Calling",
                   "Invocazione rest",
                   "Service: " + request.RequestUri + " richiesta: " + id,
                   ex.Message,
                    null);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

            return r;
        }

        public List<MyArray> CallingRicercaPosizione(string id)
        {
            List<MyArray> myArrays = new List<MyArray>();
            string message = "Errore nel collegamento con ANPR per la ricerca della Persona";
            try
            {
                WebResponse webResponse = request.GetResponse();
                HttpStatusCode code = ((HttpWebResponse)webResponse).StatusCode;
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                myArrays = JsonConvert.DeserializeObject<List<MyArray>>(response);                

            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                   "ERR_453",
                   "Certi.WebApp.Business.CreateClientRequest",
                   "Calling",
                   "Invocazione rest",
                   "Service: " + request.RequestUri + " richiesta: " + id,
                   ex.Message,
                    null);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

            return myArrays;
        }

        public ResponseRichiestaToken CallingRichiestaToken(string id)
        {
            ResponseRichiestaToken r = null;
            string message = "Errore nell'autenticazione con ANPR";
            try
            {
                WebResponse webResponse = request.GetResponse();
                HttpStatusCode code = ((HttpWebResponse)webResponse).StatusCode;
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                r = JsonConvert.DeserializeObject<ResponseRichiestaToken>(response);
                if (r == null)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                     "ERR_427",
                     "Certi.WebApp.Business.SIPORequest",
                     "Calling",
                     "Invocazione rest",
                     "Service: " + request.RequestUri + " richiesta: " + id,
                     message,
                      null);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                    log.Error(error);
                    throw mex;
                }


            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                   "ERR_453",
                   "Certi.WebApp.Business.CreateClientRequest",
                   "Calling",
                   "Invocazione rest",
                   "Service: " + request.RequestUri + " richiesta: " + id,
                    ex.Message,
                    null);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }

            return r;
        }


    }
}
