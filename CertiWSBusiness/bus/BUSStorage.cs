using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Component;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.Logging;
using log4net;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class BUSStorage
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BUSStorage));
        public X509Certificate Certificate;
        private CertificatoDocData data;

        public BUSStorage()
        {
        }

        public BUSStorage(string ciu, string codFis, DateTime dataEmissione, string extension, byte[] pdf, int status, int idCertificato)
        {
            data = new CertificatoDocData();
            data.Index = new CertificatoDocDataIndex();
            data.Index.CIU = ciu;
            data.Index.CodiceFiscale = codFis;
            data.Index.DataEmissione = dataEmissione;
            data.Index.Extension = extension;
            data.Index.Status = status;
            data.Index.IDCertificato = idCertificato;

            int len = pdf.Length;
            if (len % 2 != 0)
            {
                byte[] app = new byte[len + 1];
                for (int i = 0; i < len; i++)
                {
                    app[i] = pdf[i];
                }

                app[len] = 0;
                pdf = app;
            }
            data.BlobFile = pdf;
        }

        protected string CertificateName
        {
            get
            {
                return ConfigurationManager.AppSettings["CertificateName"];
            }
        }

        protected string IndexConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["IndexConnection"];
            }
        }

        protected string StorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["StorageConnection"];
            }
        }

        public System.Security.Cryptography.X509Certificates.X509Certificate2 X509Certificate { get; private set; }

        /// <summary>
        /// Flusso archiviazione documento
        /// </summary>
        /// <returns>true-false</returns>
        public bool SaveCertificato()
        {
            bool ret = false;

            try
            {
               

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                log.Debug("istanza store fatta");
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);               
                log.Debug("store aperto");
                if(store != null)
                { log.Debug("store pieno"); }
                if(store.Certificates != null)
                { log.Debug(store.Certificates.Count);}
                if (store.Certificates.Count > 0)
                {
                    log.Debug("inizio ciclo store certificates");                  
                    X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, CertificateName.Substring(3), false);
                    log.Debug("letto certificati");
                    if (certs.Count > 0)
                    {
                        log.Debug("sto per leggere il primo certificato");                    
                        this.X509Certificate = (System.Security.Cryptography.X509Certificates.X509Certificate2)(certs[0]);
                        log.Debug(certs[0].FriendlyName);                   
                        log.Debug("certificato x509 fatto");
                    }
                }
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di archiviazione"
                    + " del documento pdf con CIU: " + data.Index.CIU+ " dettagli" + ex.Message,
                    "ERR_164",
                    "Certi.WS.Business.BUSStorage",
                    "SaveCertificato",
                    "Flusso archiviazione del documento generato",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }

            if (this.X509Certificate != null)
            {

                try
                {
                    bool allOk = false;
                    string errorMsg = String.Empty;

                    DistributedTransaction.Utils ut = new DistributedTransaction.Utils();
                    byte[] chiave = ut.MakePKCS(data.BlobFile, this.X509Certificate, false);

                    if (chiave != null)
                    {
                        CertificatoMDData objMD = BuildObjectMD(chiave);
                        Byte[] xml = MetadataBuilder.CreateMetadata(objMD);
                        Byte[] pkcsImg = ut.MakePKCS(data.BlobFile, this.X509Certificate, true);

                        if (pkcsImg != null)
                        {
                            Byte[] pkcsXml = ut.MakePKCS(xml, this.X509Certificate, true);
                            if (pkcsXml != null)
                            {
                                Byte[][] packet = new Byte[][] { pkcsImg, pkcsXml, new byte[0] };
                                try
                                {
                                    ret = Store(packet, data.Index);
                                    if (ret)
                                        allOk = true;
                                }
                                catch (Exception ex)
                                {
                                    errorMsg = ex.Message;
                                }
                            }
                            else
                            {
                                errorMsg = "Errore nel firmare i metadati serializzati"
                                    + " creando un unico file contenente il documento in chiaro e la firma(PKCS#7). - pkcsXml";
                            }
                        }
                        else
                        {
                            errorMsg = "Errore nel firmare il pdf"
                                + " creando un unico file contenente il documento in chiaro e la firma(PKCS#7). - pkcsImg";
                        }
                    }
                    else
                    {
                        errorMsg = "Errore nel firmare il pdf"
                            + " salvando l'hash firmato in un file separato dai dati in chiaro. - chiave";
                    }

                    if (!allOk)
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di archiviazione"
                            + " del documento pdf con CIU: " + data.Index.CIU + "Dettagli: " + errorMsg,
                            "ERR_165",
                            "Certi.WS.Business.BUSStorage",
                            "SaveCertificato",
                            "Flusso archiviazione del documento generato",
                            string.Empty,
                            "PassiveObjectCF: " + data.Index.CodiceFiscale,
                            null);
                        Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                        log.Error(error);
                        throw mex;
                    }
                    else
                    {
                        ret = true;
                    }
                }
                catch(Exception ex)
                {
                    ManagedException mex = new ManagedException("Errore nel metodo di archiviazione"
                    + " del documento pdf con CIU: " + data.Index.CIU + " dettagli" + ex.Message,
                    "ERR_189",
                    "Certi.WS.Business.BUSStorage",
                    "SaveCertificato",
                    "Flusso archiviazione del documento generato",
                    string.Empty,
                    string.Empty,
                    ex.InnerException);
                    Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                    log.Error(error);
                    throw mex;

                }
            }
            return ret;
        }

        /// <summary>
        /// Flusso recupero documento dallo storage
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns>byte[][] contenente l'esito e il documento pdf</returns>
        public byte[][] LoadCertificato(string ciu)
        {
            byte[][] ret = null;

            try
            {
                byte[][] pdfRetrieved = this.Retrieve(ciu);
                byte[] esito = pdfRetrieved[0];
                if (esito[0] == 0)
                {
                    byte[] doc1 = pdfRetrieved[3];

                    DistributedTransaction.Utils ut = new DistributedTransaction.Utils();

                    byte[] pdf = ut.ExtractPKCS(doc1, false, false);
                    ret = new byte[][] { esito, pdf as byte[] };
                }
                else
                {
                    ret = new byte[][] { esito, new byte[0] };
                }
            }
            catch (ManagedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di gestione del flusso recupero del documento pdf dettagli: "+ ex.Message,
                    "ERR_165",
                    "Certi.WS.Business.BUSStorage",
                    "LoadCertificato",
                    "Recupero del documento generato" + "- " + ex.StackTrace,
                    null,
                    "CIU: " + ciu,
                    ex);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return ret;
        }

        /// <summary>
        /// Preparazione oggetto rappresentante i metadati da salvare in storage.(indice e pdf) 
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        private CertificatoMDData BuildObjectMD(byte[] sign)
        {
            CertificatoMDData md = new CertificatoMDData();

            // Dati documento
            md.Doc = new CertificatoMDDataDoc();
            md.Doc.CIU = data.Index.CIU;
            md.Doc.CodiceFiscale = data.Index.CodiceFiscale;
            md.Doc.DataEmissione = data.Index.DataEmissione;

            //file pdf
            md.BlobFile = new CertificatoMDDataBlobFile();
            md.BlobFile.Hash = sign;
            md.BlobFile.Formato = data.Index.Extension;
            md.BlobFile.Dimensione = data.BlobFile.LongLength.ToString();
            return md;
        }

        /// <summary>
        /// Metodo di storage usando i componenti com
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="index"></param>
        /// <returns>true-false</returns>
        private bool Store(Byte[][] packet, CertificatoDocDataIndex index)
        {
            bool ret = false;

            Hashtable listConns = new Hashtable();

            listConns.Add("STORAGE", StorageConnectionString);
            listConns.Add("IDX", IndexConnectionString);

            CertificatoComponentRoot component = null; ;

            try
            {
                component = new CertificatoComponentRoot();
                ret = component.Store(packet, index, listConns, CertificateName);
            }
            catch (Exception ex)
            {
                Com.Unisys.Logging.Certi.CertiErrorLogInfo error = new Com.Unisys.Logging.Certi.CertiErrorLogInfo();
                error.freeTextDetails = "Errore nello storage del documento pdf con CIU: " + index.CIU
                    + " Errore CWSB0016. Dettagli: Errore " + ex.Message;
                error.logCode = "ERR";
                error.loggingAppCode = "CWS";
                error.flussoID = String.Empty;
                error.clientID = String.Empty;
                error.activeObjectCF = String.Empty;
                error.activeObjectIP = String.Empty;
                error.passiveObjectCF = index.CodiceFiscale;
                log.Error(error);
            }
            finally
            {
                if (component != null)
                    component.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// Metodo di recupero del documento pdf usando i componenti com+  
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        private byte[][] Retrieve(string indice)
        {
            byte[][] ret = null;

            Hashtable listConns = new Hashtable();

            listConns.Add("IDX", IndexConnectionString);
            listConns.Add("STORAGE", StorageConnectionString);

            CertificatoComponentNode component = null; ;

            try
            {
                component = new CertificatoComponentNode(CertificateName);
                byte[] handle=DataLayer.Dao.getDaoFactory(Com.Unisys.Data.StoreType.ORACLE).DaoImpl.Richiesta.ReadHandle(indice);
                ret = component.Retrieve(handle, listConns);
            }
            catch (Exception ex)
            {
                ManagedException mex = new ManagedException("Errore nel metodo di gestione del flusso recupero del documento pdf dettagli: " + ex.Message,
                    "ERR_166",
                    "Certi.WS.Business.BUSStorage",
                    "Retrieve",
                    "Recupero del documento pdf" + " - " + ex.StackTrace,
                    string.Empty,
                    "Indice: " + indice,
                    ex.InnerException);
                Com.Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWS", mex);
                log.Error(error);
                throw mex;
            }
            return ret;
        }
    }
}
