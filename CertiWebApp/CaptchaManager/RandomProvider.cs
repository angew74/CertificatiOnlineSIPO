using System;

using System.Text;

namespace ImageKit.Util
{
    class RandomProvider
    {
        static readonly RandomProvider instance = new RandomProvider();
        
        static RandomProvider(){}

        private static Random rand=new Random();
        private static DateTime renewTime=System.DateTime.Now;

        RandomProvider()
        {
            
        }

        public static Random Instance
        {
            get
            {
                
                if (rand==null || (renewTime.AddHours(8)< System.DateTime.Now)){
                    lock(rand){
                        rand= new Random();
                        renewTime=System.DateTime.Now;
                    }
                }
                return RandomProvider.rand;
            }
        }

    }
}
