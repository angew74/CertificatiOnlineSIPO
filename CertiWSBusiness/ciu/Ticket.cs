using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.Utils;    

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class Ticket
    {
        
        internal Ticket(System.IO.MemoryStream document)
        {
            this.timeStamp = System.DateTime.Now;
            TicketHelper c = TicketHelper.Instance;
            this.CIU = new CIU(CryptoUty.getEncodedHash(document, EncType.Trentadue), this.timeStamp);
        }
        
        internal Ticket(string document)
        {
            this.timeStamp = System.DateTime.Now;
            TicketHelper c = TicketHelper.Instance;
            this.CIU = new CIU(CryptoUty.getEncodedHash(String.Concat(document, this.timeStamp.ToString("o")) , EncType.Trentadue), this.timeStamp);
        }
        public readonly DateTime timeStamp;
        public readonly CIU CIU;
    }
}
