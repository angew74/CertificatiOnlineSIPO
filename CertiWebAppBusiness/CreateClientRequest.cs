using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Objects.Pagamenti;
using Com.Unisys.CdR.Certi.WebApp.Business.Utility;
using Com.Unisys.Logging;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Security.Policy;
using System.Text;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Xml;

namespace Com.Unisys.CdR.Certi.WebApp.Business
{
    public class CreateClientRequest
    {

        private static readonly ILog log = LogManager.GetLogger("CreateClientRequest");
        private HttpWebRequest request = null;


        public CreateClientRequest(string url, string service, metadata m, string tipoIdentificativo, string identificativo)
        {
            CreateObject(url, service, m, null, tipoIdentificativo, identificativo);
        }
        public CreateClientRequest(string url, string service, metadata m, string Data)
        {
            CreateObject(url, service, m, Data, null, null);

        }

        private void CreateObject(string url, string service, metadata m, string data, string tipoIdentificativo, string identificativo)
        {
            // System.Diagnostics.HttpRawTraceListener.Initialize();
            request = (HttpWebRequest)WebRequest.Create(url);
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
                string metadata = JsonConvert.SerializeObject(m);
                request.ContentType = "application/json";
                request.Accept = "application/json";
                string ticks = System.DateTime.Now.Ticks.ToString();
                StringBuilder body = new StringBuilder();
                body.Append("{\"header\":{\"msgUid\": \"" + ticks + "\", \"timestamp\":  null , \"codApplication\": \"" + ConfigurationManager.AppSettings["codApplication"] + "\", \"codEnte\": \"" + ConfigurationManager.AppSettings["codEnte"] + "\",");
                body.Append("\"caller\": \"" + ConfigurationManager.AppSettings["caller"] + "\", \"callee\": \"" + ConfigurationManager.AppSettings["callee"] + "\",\"metadata\":" + metadata + ",");
                body.Append("\"user\":  null ,\"service\": \"" + service + "\", \"method\": null ");
                body.Append("}");
                if (data == null)
                {
                    body.Append(", \"body\":{\"@dto\":\"map\", \"elements\" : {\"tipoidentificativo\" : \"" + tipoIdentificativo + "\",  \"identificativo\" : \"" + identificativo + "\"}}}");
                }
                else
                {
                    string bytedata = SerializationUtils.ToBase64Encode(data);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(data);
                  //   xmlDocument.Save(@"c:\temp\xmlchiamata_" + service+ ".xml");
                    log.Debug("salvato xml");
                    if (service == "crediti.invioPosizioneCreditoria")
                    {
                        body.Append(", \"body\":{\"@dto\":\"binary\",\"content\":\"" + bytedata + "\"}}");
                    }
                    else
                    {
                        body.Append(", \"body\":{\"@dto\":\"map\", \"elements\" : {\"canale\" : \"\",  \"data\" : {\"@dto\" : \"binary\",\"content\":\"" + bytedata + "\"}}}}");
                    }
                }
                //  byte[] byteArray = Convert.FromBase64String(body);
                byte[] byteArray = Encoding.UTF8.GetBytes(body.ToString());
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + ex.Message,
                           "456",
                           "Certi.WebApp.Business.CreateClientRequest",
                           "CreateObject",
                           "Preparazione Rest",
                           "Service: " + service,
                           "ActiveObjectCF: " + identificativo,
                           ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
        }

        public string Calling(string method)
        {
            string content = null;
            try
            {
                WebResponse webResponse = request.GetResponse();
                HttpStatusCode code = ((HttpWebResponse)webResponse).StatusCode;
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                bool success = false;
                string message = "";
                string id = "";
                if (method == "predisponi")
                {
                    var r = JsonConvert.DeserializeObject<ResponseJsonPredisponi>(response);
                    success = r.Header.Success;
                    message = r.Header.Message;
                    id = r.Header.RequestRef.Metadata.TrackerBizidProgPosizione;
                    if (success)
                    {
                        content = r.Body.Elements.Ticket.ToString();
                    }
                    else
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                         "ERR_452",
                         "Certi.WebApp.Business.CreateClientRequest",
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
                else if (method == "invioposizione")
                {
                    var r = JsonConvert.DeserializeObject<ResponseJson>(response);
                    success = r.Header.Success;
                    message = r.Header.Message;
                    id = r.Header.RequestRef.Metadata.TrackerBizidProgPosizione;
                    if (success)
                    {
                        if (r.Header.RetCode == 200)
                        { content = r.Header.Metadata.TrackerBizidIuv; }
                        else
                        {
                            ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                         "ERR_453",
                         "Certi.WebApp.Business.CreateClientRequest",
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
                    else
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                         "ERR_454",
                         "Certi.WebApp.Business.CreateClientRequest",
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
                else if (method == "controlla")
                {
                    var r = JsonConvert.DeserializeObject<ResponseJson>(response);
                    success = r.Header.Success;
                    message = r.Header.Message;
                    id = r.Header.RequestRef.Metadata.TrackerBizidProgPosizione;
                    if (success)
                    {
                        string codeRET = r.Header.RetCode.ToString();
                        if (codeRET == "200")
                        {
                            content = SerializationUtils.ToBase64Decode(r.Body.Content);
                        }
                        else
                        {
                            content = null;
                        }
                    }
                    else
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                         "ERR_456",
                         "Certi.WebApp.Business.CreateClientRequest",
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
                if (success == false)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + message,
                          "ERR_457",
                          "Certi.WebApp.Business.CreateClientRequest",
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
            catch (WebException w)
            {
                log.Error(w.Message);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di business (CertiWebAppBusiness) Dettagli:  " + ex.Message,
                           "ERR_458",
                           "Certi.WebApp.Business.CreateClientRequest",
                           "Calling",
                           "Invocazione rest",
                           "Service: " + request.RequestUri,
                           "",
                           ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CSWB", mex);
                log.Error(error);
                throw mex;
            }
            return content;
        }

    }
}
