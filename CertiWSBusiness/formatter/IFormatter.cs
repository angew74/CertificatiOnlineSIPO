using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    interface IFormatter
    {
        System.IO.MemoryStream formatData(System.Xml.XmlDocument rawData, IList<string> Layout);      
    }
}
