using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.Utils;    

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public class CIU
    {
        public static readonly int RAND_LENGTH = 9;
        public readonly String fragment;
        public readonly DateTime timestamp;
        internal CIU(){}
        
        internal CIU(string fingerPrint,System.DateTime timestamp){
            
            System.Collections.ArrayList indexes = new System.Collections.ArrayList();
            Random rnd=RandomProvider.Instance;
            int i=0;
            StringBuilder sb = new StringBuilder();
            while(i < RAND_LENGTH)
            {
                int temp=rnd.Next(0, fingerPrint.Length - 1);
                if (!indexes.Contains(temp))
                {
                    indexes.Add(temp);
                    sb.Append(fingerPrint.Substring(temp,1));
                    i=i+1;
                }
            }
            this.fragment=sb.ToString();
            this.timestamp=timestamp;
           
        }

        //internal CIU(string CIU){
        //    this.fragment = CIU.Substring(0, RAND_LENGTH);
        //    this.timestamp = new DateTime(int.Parse(CIU.Substring(RAND_LENGTH + 7, 2))+2000,
        //                                int.Parse(CIU.Substring(RAND_LENGTH + 4, 2)),
        //                                int.Parse(CIU.Substring(RAND_LENGTH + 1, 2)));
           
        //}
       
		public override String ToString()
		{
            //StringBuilder b = new StringBuilder(fragment.ToString());
            StringBuilder b = new StringBuilder();
            String parsed = b.Append(fragment.ToString().Substring(0,3)).
                Append("-").
                Append(fragment.ToString().Substring(3,3)).
                Append("-").
                Append(fragment.ToString().Substring(6,3)).
			    Append("-").
				Append(timestamp.Day.ToString().PadLeft(2, '0')).
                Append(timestamp.Month.ToString().PadLeft(2, '0').Substring(0,1)).
                Append("-").
				Append(timestamp.Month.ToString().PadLeft(2, '0').Substring(1,1)).
				Append(timestamp.Year.ToString().Substring(2, 2)).ToString();
			return parsed;
		}
    }
}
