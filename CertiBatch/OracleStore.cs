using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace CertiBatch
{
    public static class OracleStore
    {
        public static DataTable SelectCertificatiByStatus(Int16 statusId)
        {
            DataTable response = new DataTable();

            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["OraConn"].ToString();
            OracleCommand command = new OracleCommand(null, _connection);
            OracleDataAdapter adapt = new OracleDataAdapter();
            _connection.Open();

            try
            {
                command.CommandText = "SELECT CERTIFICATI.CIU " +
                                      "FROM CERTIFICATI " +
                                      "WHERE CERTIFICATI.STATUS_ID = :STATUS_ID";

                OracleParameter opSTATUS_ID = new OracleParameter("STATUS_ID", OracleDbType.Int16);
                opSTATUS_ID.Value = statusId;
                command.Parameters.Add(opSTATUS_ID);

                command.ExecuteNonQuery();

                adapt.SelectCommand = command;
                adapt.Fill(response);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }

        public static void UpdateCertificato(string idCertificato, Int16 statusId)
        {
            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["OraConn"].ToString();
            OracleCommand command = new OracleCommand(null, _connection);
            _connection.Open();

            try
            {
                command.CommandText = "UPDATE CERTIFICATI " +
                                      "SET CERTIFICATI.STATUS_ID = :STATUS_ID " +
                                      "WHERE CERTIFICATI.CIU = :CIU";

                OracleParameter opSTATUS_ID = new OracleParameter("STATUS_ID", OracleDbType.Int16);
                OracleParameter opCIU = new OracleParameter("CIU", OracleDbType.Varchar2);

                opSTATUS_ID.Value = statusId;
                opCIU.Value = idCertificato;

                command.Parameters.Add(opSTATUS_ID);
                command.Parameters.Add(opCIU);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }

        public static void UpdateCertificato(string idCertificato, Int16 statusId, string xmlPagamento, string codicePagamento)
        {
            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["OraConn"].ToString();
            OracleCommand command = new OracleCommand(null, _connection);
            _connection.Open();

            try
            {
                command.CommandText = "UPDATE CERTIFICATI " +
                                      "SET CERTIFICATI.STATUS_ID = :STATUS_ID, " +
                                      "CERTIFICATI.XML_PAGAMENTO = :XML_PAGAMENTO, " +
                                      "CERTIFICATI.CODICE_PAGAMENTO = :CODICE_PAGAMENTO, " +
                                      "CERTIFICATI.SOGGETTO_RITIRO = '' " +
                                      "WHERE CERTIFICATI.CIU = :CIU";

                OracleParameter opSTATUS_ID = new OracleParameter("STATUS_ID", OracleDbType.Int16);
                OracleParameter opXML_PAGAMENTO = new OracleParameter("XML_PAGAMENTO", OracleDbType.Clob);
                OracleParameter opCODICE_PAGAMENTO = new OracleParameter("CODICE_PAGAMENTO", OracleDbType.Varchar2);
                OracleParameter opCIU = new OracleParameter("CIU", OracleDbType.Varchar2);

                opSTATUS_ID.Value = statusId;
                opXML_PAGAMENTO.Value = xmlPagamento;
                opCODICE_PAGAMENTO.Value = codicePagamento;
                opCIU.Value = idCertificato;

                command.Parameters.Add(opSTATUS_ID);
                command.Parameters.Add(opXML_PAGAMENTO);
                command.Parameters.Add(opCODICE_PAGAMENTO);
                command.Parameters.Add(opCIU);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }
    
    }
}
