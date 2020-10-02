using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    public partial class Component1 : Component
    {
        public Component1()
        {
            InitializeComponent();
        }

        public Component1(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
