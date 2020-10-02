using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.Data;
using Com.Unisys.CdR.Certi.DataLayer.Contract;
using Com.Unisys.CdR.Certi.DataLayer.OracleImpl;

namespace Com.Unisys.CdR.Certi.DataLayer
{
    public class OracleSessionManager : Com.Unisys.Data.Oracle10.OracleDaoSession<OracleSessionManager, ISession>, ISession
    {
        public OracleSessionManager() 
        {
            base.Daos = this;
        }
       
        public IDAOListaSemplice ListaSemplice 
        {
            get { return new DAOListaSemplice(this); }
        }

        public IDAORichiesta Richiesta
        {
            get { return new DAORichiesta(this); }
        }

        public IDAOEntity1  Entity1
        {
            get { return new DAOEntity1(this); }
        }
        
        public IDAOEntity2 Entity2
        {
           get{return new DAOEntity2(this);}
        }
    }
}
