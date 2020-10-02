using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.DataLayer.Contract;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    public class DAOEntity2 : Com.Unisys.Data.Oracle10.OracleDao<OracleSessionManager,ISession>, IDAOEntity2
    {
        #region IDAOEntity2 Members

        public DAOEntity2(OracleSessionManager session) : base(session) { }

        public System.Data.DataTable UpdatePiffero()
        {
            return null;
        }

        #endregion
    }
}