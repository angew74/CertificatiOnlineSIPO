using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.DataLayer.Contract;

namespace Com.Unisys.CdR.Certi.DataLayer.OracleImpl
{
    public class DAOEntity1 : Com.Unisys.Data.Oracle10.OracleDao<OracleSessionManager,ISession>, IDAOEntity1 
    {
        #region IDAOEntity1 Members

        public DAOEntity1(OracleSessionManager session) : base(session) { }

        public System.Data.DataTable LoadUtente(string parentcode)
        {
            
            base.Context.DaoImpl.StartTransaction(this.GetType());
            //DBWork
            base.Context.DaoImpl.Entity2.UpdatePiffero();
            base.Context.EndTransaction(this.GetType());
            return null;
        }

        #endregion
    }
}
