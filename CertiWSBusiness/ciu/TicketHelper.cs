using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public sealed class TicketHelper
    {


        static readonly TicketHelper instance = new TicketHelper();

        static TicketHelper() { }

        TicketHelper() { }

        public static TicketHelper Instance
        {
            get
            {
                return instance;
            }
        }

        
        public Ticket getNewTicket(System.IO.MemoryStream document)
        {
            return new Ticket(document);
        }
        

        public Ticket getNewTicket(string document)
        {
            return new Ticket(document);
        }
    }
}
