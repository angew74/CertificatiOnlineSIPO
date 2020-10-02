using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.DataLayer.Contract;
using Com.Unisys.Data;

namespace Com.Unisys.CdR.Certi.DataLayer
{
    public interface ISession: IDaoBaseSession<ISession>
    {
        IDAOListaSemplice ListaSemplice { get;}
        IDAORichiesta Richiesta { get;}
        IDAOEntity1 Entity1 { get;}
        IDAOEntity2 Entity2 { get;}
    }

}
