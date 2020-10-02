using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using log4net;
using System.Xml;
using CambiResidenzaUtility;
using Com.Unisys.Messaging.MapperMessages;
using Com.Unisys.CdR.DataObjects;
using Com.Unisys.CdR.Certi.Utils;
using Unisys;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class CommonMapperRequests
    {
        private static readonly ILog _log = LogManager.GetLogger("MapperGeneric");


        #region chiamate al mapper

        public static T ExecuteDataSet<T>(DataSet request, string service) where T : System.Data.DataSet, new()
        {
            string datiMapper = Execute(request, service);
            T data = new T();
            data.ReadXml(new System.IO.StringReader(datiMapper));
            return data;
        }

        public static T ExecuteDataSet<T>(DataSet request, string service, ref string xml) where T : System.Data.DataSet, new()
        {
            xml = Execute(request, service);
            T data = new T();
            data.ReadXml(new System.IO.StringReader(xml));
            return data;
        }

        public static string ExecuteXML(DataSet request, string service)
        {
            return Execute(request, service);
        }

        private static void Trim(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
                foreach (DataRow dr in dt.Rows)
                    foreach (DataColumn dc in dt.Columns)
                        if (dr[dc] is string)
                            dr[dc] = (dr[dc] as string).Trim();
        }

        private static void Pad(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
                foreach (DataRow dr in dt.Rows)
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.DataType != typeof(System.String))
                            continue;

                        if (dr[dc] is DBNull)
                        {
                            dr[dc] = " ";
                            continue;
                        }

                        if ((dr[dc] as string).Length == 0)
                            dr[dc] = " ";
                    }
        }

        public static void Replace(DataSet ds, string oldValue, string newValue)
        {
            foreach (DataTable table in ds.Tables)
                foreach (DataRow row in table.Rows)
                    foreach (DataColumn column in table.Columns)
                        if (row[column] is string)
                            row[column].ToString().Replace(oldValue, newValue);
        }

        #endregion

        #region metodi privati

        private static string Execute(DataSet request, string service)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Xml.XmlDocument doc = new XmlDocument();
            request.WriteXml(stream, System.Data.XmlWriteMode.IgnoreSchema);
            stream.Position = 0;
            System.Xml.XmlReader reader = new System.Xml.XmlTextReader(stream);
            doc.Load(reader);
            string richiesta = doc.InnerXml.Replace("\"", "'");
            richiesta += "#";
            string datiMapper = null;
            int tpur = 0;
            OS2200_IntegrationSmart _openTI = null;
            OpenTIWebService.IntegrationSmart ws = null;
            try
            {
                if (Config.ReadSetting("MapperGate").Equals("local"))
                {
                    _openTI = new OS2200_IntegrationSmart();
                    bool esito = _openTI.CallMapper(Config.ReadSetting("MapperUser"), Config.ReadSetting("MapperDep"), Config.ReadSetting("MapperPwd"), service, Config.ReadSetting("MapperEnv"), richiesta, out datiMapper, Config.ReadSetting("MapperHost"), Config.ReadSetting("MapperServ"), out tpur);
                }
                else
                {
                    ws = new OpenTIWebService.IntegrationSmart();
                    ws.Url = Config.ReadSetting("MapperGate");
                    bool esito = ws.CallMapperWebMethod(Config.ReadSetting("MapperUser"), Config.ReadSetting("MapperDep"), Config.ReadSetting("MapperPwd"), service, Config.ReadSetting("MapperEnv"), richiesta, Config.ReadSetting("MapperHost"), Config.ReadSetting("MapperServ"), out datiMapper, out tpur);
                }
            }
            catch (System.Exception e)
            {
                _log.Error(string.Format("Errore nel servizio: {0}", service));
                _log.Error(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (_openTI != null) _openTI.Dispose();
                if (ws != null) ws.Dispose();

            }
            datiMapper = datiMapper.Replace("#", " ");
            datiMapper = datiMapper.Replace("\\", "");
            return datiMapper;
        }
        #endregion
    }
}
