using java.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Com.Unisys.CdR.Certi.WS.Business.formatter
{
    class DotNetOutputMemoryStream : OutputStream
    {
        private System.IO.MemoryStream ms = new System.IO.MemoryStream();
        public System.IO.MemoryStream Stream
        {
            get
            {
                return ms;
            }
        }
        public override void write(int i)
        {
            ms.WriteByte((byte)i);
        }
        public override void write(byte[] b, int off, int len)
        {
            ms.Write(b, off, len);
        }
        public override void write(byte[] b)
        {
            ms.Write(b, 0, b.Length);
        }
        public override void close()
        {
            ms.Close();
        }
        public override void flush()
        {
            ms.Flush();
        }
    }
}
