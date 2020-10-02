namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    partial class ComponentRichieste
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentRichieste));
            Oracle.DataAccess.Client.OracleParameter oracleParameter1 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter2 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter3 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter4 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter5 = new Oracle.DataAccess.Client.OracleParameter();
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
            Oracle.DataAccess.Client.OracleParameter oracleParameter19 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter20 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter21 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter22 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter23 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter24 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter25 = new Oracle.DataAccess.Client.OracleParameter();
            Oracle.DataAccess.Client.OracleParameter oracleParameter26 = new Oracle.DataAccess.Client.OracleParameter();
            this.richiesteOracleDataAdapter1 = new Oracle.DataAccess.Client.OracleDataAdapter();
            this.richiesteDeleteOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            this.richiesteInsertOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            this.richiesteSelectOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            this.richiesteUpdateOracleCommand1 = new Oracle.DataAccess.Client.OracleCommand();
            // 
            // richiesteOracleDataAdapter1
            // 
            this.richiesteOracleDataAdapter1.DeleteCommand = this.richiesteDeleteOracleCommand1;
            this.richiesteOracleDataAdapter1.InsertCommand = this.richiesteInsertOracleCommand1;
            this.richiesteOracleDataAdapter1.SelectCommand = this.richiesteSelectOracleCommand1;
            this.richiesteOracleDataAdapter1.UpdateCommand = this.richiesteUpdateOracleCommand1;
            // 
            // richiesteDeleteOracleCommand1
            // 
            this.richiesteDeleteOracleCommand1.CommandText = resources.GetString("richiesteDeleteOracleCommand1.CommandText");
            oracleParameter1.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter1.ParameterName = ":original_ID_param0";
            oracleParameter1.SourceColumn = "ID";
            oracleParameter1.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter2.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter2.ParameterName = ":original_CLIENT_ID_param1";
            oracleParameter2.SourceColumn = "CLIENT_ID";
            oracleParameter2.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter3.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter3.ParameterName = ":original_UFFICIO_ID_param2";
            oracleParameter3.SourceColumn = "UFFICIO_ID";
            oracleParameter3.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter4.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter4.ParameterName = ":original_UFFICIO_ID_param3";
            oracleParameter4.SourceColumn = "UFFICIO_ID";
            oracleParameter4.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter5.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter5.ParameterName = ":original_STATUS_ID_param4";
            oracleParameter5.SourceColumn = "STATUS_ID";
            oracleParameter5.SourceVersion = System.Data.DataRowVersion.Original;
            this.richiesteDeleteOracleCommand1.Parameters.Add(oracleParameter1);
            this.richiesteDeleteOracleCommand1.Parameters.Add(oracleParameter2);
            this.richiesteDeleteOracleCommand1.Parameters.Add(oracleParameter3);
            this.richiesteDeleteOracleCommand1.Parameters.Add(oracleParameter4);
            this.richiesteDeleteOracleCommand1.Parameters.Add(oracleParameter5);
            // 
            // richiesteInsertOracleCommand1
            // 
            this.richiesteInsertOracleCommand1.CommandText = "INS_NEW_REQUEST";
            this.richiesteInsertOracleCommand1.CommandType = System.Data.CommandType.StoredProcedure;
            oracleParameter6.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter6.ParameterName = ":V_CLIENT_ID";
            oracleParameter6.SourceColumn = "CLIENT_ID";
            oracleParameter7.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter7.ParameterName = ":V_UFFICIO_ID";
            oracleParameter7.SourceColumn = "UFFICIO_ID";
            oracleParameter8.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter8.ParameterName = ":V_STATUS_ID";
            oracleParameter8.SourceColumn = "STATUS_ID";
            oracleParameter9.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter9.ParameterName = ":V_CODICE_FISCALE_RICHIEDENTE";
            oracleParameter9.SourceColumn = "CODICE_FISCALE_RICHIEDENTE";
            oracleParameter10.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter10.ParameterName = "V:CODICE_FISCALE_INTESTATARIO";
            oracleParameter10.SourceColumn = "CODICE_FISCALE_INTESTATARIO";
            oracleParameter11.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter11.ParameterName = ":V_COD_IND_INTESTATARIO";
            oracleParameter11.SourceColumn = "COD_IND_INTESTATARIO";
            oracleParameter12.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter12.ParameterName = ":V_COGNOME_INTESTATARIO";
            oracleParameter12.SourceColumn = "COGNOME_INTESTATARIO";
            oracleParameter13.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter13.ParameterName = ":V_NOME_INTESTATARIO";
            oracleParameter13.SourceColumn = "NOME_INTESTATARIO";
            oracleParameter14.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter14.ParameterName = ":V_TRANSAZIONE_ID";
            oracleParameter14.SourceColumn = "TRANSAZIONE_ID";
            oracleParameter15.Direction = System.Data.ParameterDirection.Output;
            oracleParameter15.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter15.ParameterName = ":V_RICHIESTA_ID";
            oracleParameter15.SourceColumn = "TRANSAZIONE_ID";
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter6);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter7);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter8);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter9);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter10);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter11);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter12);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter13);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter14);
            this.richiesteInsertOracleCommand1.Parameters.Add(oracleParameter15);
            // 
            // richiesteSelectOracleCommand1
            // 
            this.richiesteSelectOracleCommand1.CommandText = "SELECT * FROM RICHIESTE";
            // 
            // richiesteUpdateOracleCommand1
            // 
            this.richiesteUpdateOracleCommand1.CommandText = resources.GetString("richiesteUpdateOracleCommand1.CommandText");
            oracleParameter16.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter16.ParameterName = ":current_ID_param0";
            oracleParameter16.SourceColumn = "ID";
            oracleParameter17.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter17.ParameterName = ":current_CLIENT_ID_param1";
            oracleParameter17.SourceColumn = "CLIENT_ID";
            oracleParameter18.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter18.ParameterName = ":current_UFFICIO_ID_param2";
            oracleParameter18.SourceColumn = "UFFICIO_ID";
            oracleParameter19.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter19.ParameterName = ":current_STATUS_ID_param3";
            oracleParameter19.SourceColumn = "STATUS_ID";
            oracleParameter20.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter20.ParameterName = ":current_COG_INTESTA_param4";
            oracleParameter20.SourceColumn = "COGNOME_INTESTATARIO";
            oracleParameter21.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter21.ParameterName = ":current_NOME_INTESTA_param5";
            oracleParameter21.SourceColumn = "NOME_INTESTATARIO";
            oracleParameter22.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter22.ParameterName = ":original_ID_param6";
            oracleParameter22.SourceColumn = "ID";
            oracleParameter22.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter23.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter23.ParameterName = ":original_CLIENT_ID_param7";
            oracleParameter23.SourceColumn = "CLIENT_ID";
            oracleParameter23.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter24.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter24.ParameterName = ":original_UFFICIO_ID_param8";
            oracleParameter24.SourceColumn = "UFFICIO_ID";
            oracleParameter24.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter25.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            oracleParameter25.ParameterName = ":original_UFFICIO_ID_param9";
            oracleParameter25.SourceColumn = "UFFICIO_ID";
            oracleParameter25.SourceVersion = System.Data.DataRowVersion.Original;
            oracleParameter26.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            oracleParameter26.ParameterName = ":original_STATUS_ID_param10";
            oracleParameter26.SourceColumn = "STATUS_ID";
            oracleParameter26.SourceVersion = System.Data.DataRowVersion.Original;
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter16);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter17);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter18);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter19);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter20);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter21);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter22);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter23);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter24);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter25);
            this.richiesteUpdateOracleCommand1.Parameters.Add(oracleParameter26);

        }

        #endregion

        private Oracle.DataAccess.Client.OracleCommand richiesteDeleteOracleCommand1;
        private Oracle.DataAccess.Client.OracleCommand richiesteInsertOracleCommand1;
        private Oracle.DataAccess.Client.OracleCommand richiesteSelectOracleCommand1;
        private Oracle.DataAccess.Client.OracleCommand richiesteUpdateOracleCommand1;
        internal Oracle.DataAccess.Client.OracleDataAdapter richiesteOracleDataAdapter1;
    }
}
