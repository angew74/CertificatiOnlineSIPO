using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.Data;

namespace Com.Unisys.CdR.Certi.DataLayer
{
    public class Dao
    {
         public static ISession getDaoFactory(StoreType type)
         {
            switch (type)
            {
                case StoreType.ORACLE:
                    return new OracleSessionManager();
                case StoreType.OPENLDAP:
                    return null;
                default:
                    return null;
            }
        }
    }
}
