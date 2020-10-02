using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Oracle.DataAccess.Client;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    public partial class ComponentCertificati : Component
    {
        public ComponentCertificati()
        {
            InitializeComponent();
        }

        public ComponentCertificati(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public void setConnection(OracleConnection conn)
        {
            certificatiOracleDataAdapter1.InsertCommand.Connection = conn;
            certificatiOracleDataAdapter1.SelectCommand.Connection = conn;
            certificatiOracleDataAdapter1.UpdateCommand.Connection = conn;
            certificatiOracleDataAdapter1.DeleteCommand.Connection = conn;
        }

        private void certificatiOracleDataAdapter1_RowUpdated(object sender, OracleRowUpdatedEventArgs e)
        {

        }

        private void certificatiOracleDataAdapter1_RowUpdating(object sender, OracleRowUpdatingEventArgs e)
        {
            Com.Unisys.CdR.Certi.Objects.Common.ProfiloRichiesta.CertificatiRow row = e.Row as Com.Unisys.CdR.Certi.Objects.Common.ProfiloRichiesta.CertificatiRow;
            if (row != null && row.RowState != System.Data.DataRowState.Deleted)
            {
                OracleCommand cmd = e.Command;
                foreach (OracleParameter p in cmd.Parameters)
                {
                    if (p.Direction == System.Data.ParameterDirection.Input || p.Direction == System.Data.ParameterDirection.InputOutput)
                    {
                        if (row.IsNull(row.Table.Columns[p.SourceColumn], p.SourceVersion))
                        {
                            p.IsNullable = true;
                            p.Value = null;
                        }
                        else if (row[p.SourceColumn] is String && string.IsNullOrEmpty((string)row[p.SourceColumn]))
                        {
                            p.IsNullable = true;
                            p.Value = null;
                        }
                    }
                }
            }
            //if (e.StatementType == System.Data.StatementType.Insert)
            //{

            //    if (row.IsDATA_EMISSIONENull())
            //    {
            //        cmd.Parameters[":V_DATA_EMISSIONE"].IsNullable = true;
            //        cmd.Parameters[":V_DATA_EMISSIONE"].Value = null;
            //    }
            //    if (row.IsESENZIONE_IDNull())
            //    {
            //        cmd.Parameters[":V_ESENZIONE_ID"].IsNullable = true;
            //        cmd.Parameters[":V_ESENZIONE_ID"].Value = null;
            //    }
            //}
            //else if (e.StatementType == System.Data.StatementType.Update)
            //{ 


            //}
        }

        public OracleDataAdapter CertificatiDataAdapter
        {
            get { return this.certificatiOracleDataAdapter1; }
        }
    }
}
