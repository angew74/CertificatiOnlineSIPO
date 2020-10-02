using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Oracle.DataAccess.Client;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    public partial class ComponentClients : Component
    {
        public ComponentClients()
        {
            InitializeComponent();
        }

        public ComponentClients(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void setConnection(OracleConnection conn)
        {
            clientsOracleDataAdapter1.InsertCommand.Connection = conn;
            clientsOracleDataAdapter1.SelectCommand.Connection = conn;
            clientsOracleDataAdapter1.UpdateCommand.Connection = conn;
            clientsOracleDataAdapter1.DeleteCommand.Connection = conn;
        }
    }
}
