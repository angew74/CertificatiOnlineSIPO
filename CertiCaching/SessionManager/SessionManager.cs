using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace Com.Unisys.CdR.Certi.Caching
{
    public class SessionManager<T>
    {
        public static void set(SessionKeys key, T data)
        {
            if (exist(key)) HttpContext.Current.Session[Convert.ToString(key)] = data;
            else HttpContext.Current.Session.Add(Convert.ToString(key), data);
        }

        public static void del(SessionKeys key)
        {
            HttpContext.Current.Session.Remove(Convert.ToString(key));
        }

        public static T get(SessionKeys key)
        {
            if (HttpContext.Current.Session[Convert.ToString(key)] != null)
                return (T)HttpContext.Current.Session[Convert.ToString(key)];
            else return default(T);
        }

        public static bool exist(SessionKeys key)
        {
            bool bRet = false;
            if (HttpContext.Current.Session[Convert.ToString(key)] != null)
                bRet = true;
            return bRet;
        }


        public static void set_Codici(string codice, SessionKeys key)
        {
            HttpContext.Current.Session.Add(Convert.ToString(key), codice);
        }

        public static string get_Codici(SessionKeys key)
        {
            if (HttpContext.Current.Session[Convert.ToString(key)] != null)
                return HttpContext.Current.Session[Convert.ToString(key)].ToString();
            else
                return "";
        }


    }
}
