using System;
using System.Collections.Generic;
using System.Text;
using StorageComponent;
using Com.Unisys.CdR.Certi.Objects;    

namespace Com.Unisys.CdR.Certi.Component
{
    /// <summary>
    /// Summary description for CertEngine.
    /// </summary>
    public class CertificatoEngine
    {
        public CertificatoEngine()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public CertificatoEngine(string str)
        {
            name = str;
        }

        protected string name;

        public CertificatoStore MakeCustomBody(StorageDBData[] data, string id)
        {
            CertificatoStore storeBody = null;

            try
            {
                storeBody = new CertificatoStore();
                storeBody.Container = new CertificatoStoreContainer();
                storeBody.Container.FileID = id;
                storeBody.Container.Documents = new CertificatoStoreContainerDocument[2];

                storeBody.Container.Documents[0] = new CertificatoStoreContainerDocument();
                storeBody.Container.Documents[0].Hash = data[0].hash;
                storeBody.Container.Documents[0].Type = data[0].type;

                storeBody.Container.Documents[1] = new CertificatoStoreContainerDocument();
                storeBody.Container.Documents[1].Hash = data[1].hash;
                storeBody.Container.Documents[1].Type = data[1].type;
            }
            catch (Exception)
            {
            }

            return storeBody;
        }

        public StorageDBData[] MakeBodyData(byte[][] packet)
        {
            StorageDBData[] ob = new StorageDBData[2];

            StorageDBData DBImg = new StorageDBData();
            StorageDBData DBXml = new StorageDBData();

            byte[] img = packet[0];
            byte[] xml = packet[1];

            DistributedTransaction.Utils ut = new DistributedTransaction.Utils(name, true);

            byte[] hashImg = ut.ComputeHash(img);
            byte[] hashXml = ut.ComputeHash(xml);

            DBImg.doc = img;
            DBImg.hash = hashImg;
            DBImg.type = "IMG";
            DBImg.crypted = true;

            DBXml.doc = xml;
            DBXml.hash = hashXml;
            DBXml.type = "XML";
            DBImg.crypted = true;

            ob[1] = DBXml;
            ob[0] = DBImg;

            return ob;

        }
    }
}
