using System;
using System.Collections.Generic;
using System.Text;
using System.Collections; 
using System.EnterpriseServices;  
using Com.Unisys.CdR.Certi.Objects;
using DistributedTransaction;
using StorageComponent;
//using Com.Unisys.CdR.Certi.DataLayer;  

namespace Com.Unisys.CdR.Certi.Component
{
    public class CertificatoTransactionInputData
    {
        public StorageTransactionInputData StorageData;
        public CertificatoDocDataIndex IndexData;
    }

    public class CertificatoComponentRoot : DistributedTransactionProvider
    {


        public CertificatoComponentRoot()
        {
        }

        private string name = null;
        public bool Store(Byte[][] packet, CertificatoDocDataIndex index, Hashtable connections, string certName)
        {
            bool ret = true;
            name = certName;
            CertificatoEngine eng = new CertificatoEngine(name);

            StorageDBData[] DBData = eng.MakeBodyData(packet);
            CertificatoStore CustomBody = eng.MakeCustomBody(DBData, index.CIU);
            byte[] BinXMLBody = MetadataBuilder.CreateCustomBody(CustomBody);

            StorageTransactionInputData data = new StorageTransactionInputData();
            data.DBData = DBData;
            data.CustomBody = BinXMLBody;
            data.Father = packet[2];

            CertificatoTransactionInputData master = new CertificatoTransactionInputData();
            master.StorageData = data;
            master.IndexData = index;

            try
            {
                InvokeTransaction(master, connections);
            }
            catch
            {
                ret = false;
            }

            return ret;
        }


        protected SQLTransactionalStatement GetInsertIndexStatement(CertificatoDocDataIndex index, byte[] handle, string conn)
        {
            SQLTransactionalStatement stmt = new SQLTransactionalStatement();
            stmt.Connection = conn;
            stmt.Type = "ORACLE";
            SQLTransationalTypeDispatcher disp = new SQLTransationalTypeDispatcher();
            StringBuilder sb = new StringBuilder("BEGIN INSERT INTO TAB_INDEX (CIU,CODICEFISCALE,HANDLE,DATAEMISSIONE,EXTENSION) VALUES (:1,:2,:3,:4,:5);");
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, index.CIU, "ciu", System.Data.ParameterDirection.Input, System.Data.DbType.String));
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, index.CodiceFiscale, "codicefiscale", System.Data.ParameterDirection.Input, System.Data.DbType.String));
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, handle, "handle", System.Data.ParameterDirection.Input, System.Data.DbType.Binary));
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, index.DataEmissione, "dataemissione", System.Data.ParameterDirection.Input, System.Data.DbType.DateTime));
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, index.Extension, "extension", System.Data.ParameterDirection.Input, System.Data.DbType.String));

            sb.Append("Update Certificati set STATUS_ID = :6 WHERE id=:7;");
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, index.Status , "status", System.Data.ParameterDirection.Input, System.Data.DbType.Int32));
            stmt.Parameters.Add(disp.GetParameter(stmt.Type, index.IDCertificato, "idcertificato", System.Data.ParameterDirection.Input, System.Data.DbType.Int32));
            stmt.Sql = sb.ToString() + " END;";
            return stmt;
        }


        protected override void RunTransaction(object obj, Hashtable connections)
        {

            SQLTransactionalStatementList list = new SQLTransactionalStatementList();
            string indConn = connections["IDX"].ToString();
            TransactionalEngine engine = new TransactionalEngine();

            StorageComponentRoot storageComp = null;

            try
            {

                storageComp = new StorageComponentRoot();
                CertificatoTransactionInputData master = (CertificatoTransactionInputData)obj;
                StorageTransactionInputData data = master.StorageData;
                byte[] gl;

                if (storageComp.Store(data.DBData, data.CustomBody, data.Father, connections, name, out gl))
                {
                    SQLTransactionalStatement stmt = GetInsertIndexStatement(master.IndexData, gl, indConn);
                    list.Add(stmt);
                    engine.RunTransaction(list);
                    ContextUtil.SetComplete();
                }
                else
                {
                    ContextUtil.SetAbort();
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                ContextUtil.SetAbort();
                throw new Exception();
            }
            finally
            {
                if (storageComp != null)
                    storageComp.Dispose();
            }

        }

        protected override void RunTransactionWithMessage(object obj, Hashtable connections, ref string msg)
        {
            throw new NotImplementedException();
        }
    }

    public class CertificatoComponentNode
    {
        public CertificatoComponentNode()
        {
        }
        public CertificatoComponentNode(string str)
        {
            name = str;
        }

        private string name;

        public byte[][] Retrieve(byte[] globalHandle, Hashtable conns)
        {
            byte[][] ret = null;

            byte[] esito = new byte[1];
            esito[0] = 0;
            ArrayList DocsVector = new ArrayList();
            ArrayList HashVector = new ArrayList();
            byte[] body = null;

            //byte[] globalHandle = GetPdfHandle(indice, conns["IDX"].ToString());

            if (globalHandle != null)
            {
                StorageComponentNode storage = new StorageComponentNode(name);
                AssociazioneBean AssBean = storage.ReadAssociazione(globalHandle, conns["STORAGE"].ToString());
                if (AssBean != null)
                {
                    if (storage.VerifyAssociazione(AssBean))
                    {
                        Storage StorageEntity = storage.DeserializeMetadata(AssBean.Metadata);
                        body = StorageEntity.Item.Body.Metadata;
                        foreach (StorageContainerBodyDocument doc in StorageEntity.Item.Header.Documents)
                        {
                            byte[] docHash = doc.Hash;
                            DocumentoBean DocBean = storage.ReadDocumento(docHash, conns["STORAGE"].ToString());
                            if (DocBean != null)
                            {
                                if (DocBean.Pwd != null)
                                {
                                    DocBean.Document = storage.DecryptDocument(DocBean);
                                }

                                if (storage.VerifyDocument(DocBean))
                                {
                                    DocsVector.Add(DocBean.Document);
                                    HashVector.Add(docHash);
                                }
                                else
                                {
                                    esito[0] = 5;
                                }
                            }
                            else
                            {
                                esito[0] = 4;
                            }
                        }
                    }
                    else
                    {
                        esito[0] = 3;
                    }
                }
                else
                {
                    esito[0] = 2;
                }
            }
            else
            {
                esito[0] = 1;
            }

            if (DocsVector.Count == 2 && ((byte)esito[0]) == 0)
            {
                ret = new byte[][] { esito, body, HashVector[0] as byte[], DocsVector[0] as byte[], HashVector[1] as byte[], DocsVector[1] as byte[] };
            }
            else
            {
                ret = new byte[][] { esito, new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };
            }

            return ret;
        }

        

    }
}
