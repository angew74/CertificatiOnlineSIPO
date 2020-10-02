namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    partial class ComponentClients
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Oracle.DataAccess.Client.OracleParameter oracleParameter6 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter7 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter8 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter9 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter10 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter11 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter12 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter13 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter14 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter15 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter16 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter17 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter18 = new Oracle.DataAccess.Client.OracleParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentClients));
            Oracle.DataAccess.Client.OracleParameter oracleParameter1 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter2 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter3 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter4 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter5 = new Oracle.DataAccess.Client.OracleParameter();
            this.clientsOracleDataAdapter1 = new Oracle.DataAccess.Client.OracleDataAdapter();
            this.clientsSelectOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            this.clientsOracleConnection1 = new Oracle.DataAccess.Client.OracleConnection();
            this.clientsInsertOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            this.clientsUpdateOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            this.clientsDeleteOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            // 
            // clientsOracleDataAdapter1
            // 
            this.clientsOracleDataAdapter1.DeleteCommand = this.clientsDeleteOracleCommand1;
            this.clientsOracleDataAdapter1.InsertCommand = this.clientsInsertOracleCommand1;
            this.clientsOracleDataAdapter1.SelectCommand = this.clientsSelectOracleCommand1;
            this.clientsOracleDataAdapter1.UpdateCommand = this.clientsUpdateOracleCommand1;
            // 
            // clientsSelectOracleCommand1
            // 
            this.clientsSelectOracleCommand1.CommandText = "SELECT * FROM CLIENTS";
            this.clientsSelectOracleCommand1.Connection = this.clientsOracleConnection1;
            // 
            // clientsOracleConnection1
            // 
            this.clientsOracleConnection1.ConnectionString = "USER ID=certiol_idx;DATA SOURCE=sviluppo;";
            // 
            // clientsInsertOracleCommand1
            // 
            this.clientsInsertOracleCommand1.CommandText = "INSERT INTO \"CLIENTS\"( \"ID\", \"LIVELLO\", \"FLAG_RICERCA\", \"DESCRIZIONE\") VALUES ( :" +
                "current_ID_param0, :current_LIVELLO_param1, :current_FLAG_RICERCA_param2, :curre" +
                "nt_DESCRIZIONE_param3)";
            this.clientsInsertOracleCommand1.Connection = this.clientsOracleConnection1;
            oracleParameter6.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter6.ParameterName = ":current_ID_param0";
            oracleParameter6.SourceColumn = "ID";
            oracleParameter7.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter7.ParameterName = ":current_LIVELLO_param1";
            oracleParameter7.SourceColumn = "LIVELLO";
            oracleParameter8.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter8.ParameterName = ":current_FLAG_RICERCA_param2";
            oracleParameter8.SourceColumn = "FLAG_RICERCA";
            oracleParameter9.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter9.ParameterName = ":current_DESCRIZIONE_param3";
            oracleParameter9.SourceColumn = "DESCRIZIONE";
            this.clientsInsertOracleCommand1.Parameters.Add(oracleParameter6);
            this.clientsInsertOracleCommand1.Parameters.Add(oracleParameter7);
            this.clientsInsertOracleCommand1.Parameters.Add(oracleParameter8);
            this.clientsInsertOracleCommand1.Parameters.Add(oracleParameter9);
            // 
            // clientsUpdateOracleCommand1
            // 
            this.clientsUpdateOracleCommand1.CommandText = resources.GetString("clientsUpdateOracleCommand1.CommandText");
            this.clientsUpdateOracleCommand1.Connection = this.clientsOracleConnection1;
            oracleParameter10.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter10.ParameterName = ":current_ID_param0";
            oracleParameter10.SourceColumn = "ID";
            oracleParameter11.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter11.ParameterName = ":current_LIVELLO_param1";
            oracleParameter11.SourceColumn = "LIVELLO";
            oracleParameter12.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter12.ParameterName = ":current_FLAG_RICERCA_param2";
            oracleParameter12.SourceColumn = "FLAG_RICERCA";
            oracleParameter13.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter13.ParameterName = ":current_DESCRIZIONE_param3";
            oracleParameter13.SourceColumn = "DESCRIZIONE";
            oracleParameter14.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter14.ParameterName = ":original_ID_param4";
            oracleParameter14.SourceColumn = "ID";
            oracleParameter14.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter15.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter15.ParameterName = ":original_LIVELLO_param5";
            oracleParameter15.SourceColumn = "LIVELLO";
            oracleParameter15.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter16.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter16.ParameterName = ":original_FLAG_RICERCA_param6";
            oracleParameter16.SourceColumn = "FLAG_RICERCA";
            oracleParameter16.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter17.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter17.ParameterName = ":original_DESCRIZIONE_param7";
            oracleParameter17.SourceColumn = "DESCRIZIONE";
            oracleParameter17.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter18.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter18.ParameterName = ":original_DESCRIZIONE_param8";
            oracleParameter18.SourceColumn = "DESCRIZIONE";
            oracleParameter18.SourceVersion = System.Data.DataRowVersion.Original;
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter10);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter11);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter12);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter13);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter14);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter15);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter16);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter17);
            this.clientsUpdateOracleCommand1.Parameters.Add(oracleParameter18);
            // 
            // clientsDeleteOracleCommand1
            // 
            this.clientsDeleteOracleCommand1.CommandText = resources.GetString("clientsDeleteOracleCommand1.CommandText");
            this.clientsDeleteOracleCommand1.Connection = this.clientsOracleConnection1;
            oracleParameter1.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter1.ParameterName = ":original_ID_param0";
            oracleParameter1.SourceColumn = "ID";
            oracleParameter1.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter2.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter2.ParameterName = ":original_LIVELLO_param1";
            oracleParameter2.SourceColumn = "LIVELLO";
            oracleParameter2.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter3.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter3.ParameterName = ":original_FLAG_RICERCA_param2";
            oracleParameter3.SourceColumn = "FLAG_RICERCA";
            oracleParameter3.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter4.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter4.ParameterName = ":original_DESCRIZIONE_param3";
            oracleParameter4.SourceColumn = "DESCRIZIONE";
            oracleParameter4.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter5.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter5.ParameterName = ":original_DESCRIZIONE_param4";
            oracleParameter5.SourceColumn = "DESCRIZIONE";
            oracleParameter5.SourceVersion = System.Data.DataRowVersion.Original;
            this.clientsDeleteOracleCommand1.Parameters.Add(oracleParameter1);
            this.clientsDeleteOracleCommand1.Parameters.Add(oracleParameter2);
            this.clientsDeleteOracleCommand1.Parameters.Add(oracleParameter3);
            this.clientsDeleteOracleCommand1.Parameters.Add(oracleParameter4);
            this.clientsDeleteOracleCommand1.Parameters.Add(oracleParameter5);

        }

        #endregion

        private Oracle.DataAccess.Client.OracleDataAdapter clientsOracleDataAdapter1;
        private Oracle.DataAccess.Client.OracleCommand clientsDeleteOracleCommand1;
        private Oracle.DataAccess.Client.OracleConnection clientsOracleConnection1;
        private Oracle.DataAccess.Client.OracleCommand clientsInsertOracleCommand1;
        private Oracle.DataAccess.Client.OracleCommand clientsSelectOracleCommand1;
        private Oracle.DataAccess.Client.OracleCommand clientsUpdateOracleCommand1;
    }
}
