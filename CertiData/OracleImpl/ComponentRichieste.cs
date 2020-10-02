using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Oracle.DataAccess.Client;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    public partial class ComponentRichieste : Component
    {
        public ComponentRichieste()
        {
            InitializeComponent();
        }

        public ComponentRichieste(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public void setConnection(OracleConnection conn)
        {
            richiesteOracleDataAdapter1.InsertCommand.Connection = conn;
            richiesteOracleDataAdapter1.SelectCommand.Connection  = conn;
            richiesteOracleDataAdapter1.UpdateCommand.Connection  = conn;
            richiesteOracleDataAdapter1.DeleteCommand.Connection  = conn;
        }
    }
}
