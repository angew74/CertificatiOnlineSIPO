using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;
using Com.Unisys.CdR.Certi.Utils;        

namespace Com.Unisys.CdR.Certi.Caching
{
    public enum VincoloType
    {
        BACKEND,   //l'item è vincolato al backend e viene ricaricaro automaticamente se nullo in cache
        FILESYSTEM,//L'item è vincolato al filesystem e viene ricaricao automaticamente se nullo in cahe
        NONE       //L'item è gestito in modo applicativo quindi è stat fatta una set esplicita
    }

    public class CacheManager<T> where T : new()
    {
        private static readonly ILog _log = LogManager.GetLogger("CacheManager");

        public static void set(CacheKeys key, T data)
        {
            if (exist(key)) HttpContext.Current.Cache[Convert.ToString(key)] = data;
            else HttpContext.Current.Cache.Insert(Convert.ToString(key), data);
        }

        public static void del(CacheKeys key)
        {
            HttpContext.Current.Cache.Remove(Convert.ToString(key));
        }

        public static T get(CacheKeys key, VincoloType vincoloType)
        {
            string ky = Convert.ToString(key);
            bool exsist = false;

            if (HttpContext.Current.Cache[ky] != null)
                return (T)HttpContext.Current.Cache[ky];

            switch (vincoloType)
            {
                case VincoloType.NONE:
                    if (exsist)
                        return (T)HttpContext.Current.Cache[ky];
                    else
                        return default(T);
                    break;

                case VincoloType.FILESYSTEM:
                    if (typeof(T) == typeof(System.Xml.XmlDocument))
                    {
                        T data = default(T);
                        data = getXml(key);
                        return data;
                    }
                    if (typeof(T) == typeof(System.Xml.Xsl.XslCompiledTransform))
                    {
                        T data = default(T);
                        data = getXslt(key);
                        return data;
                    }
                    if ((typeof(T)).BaseType == typeof(System.Data.DataSet) ||
                        typeof(T) == typeof(System.Data.DataSet))
                    {
                        return getDatasetFromFileSystem(key);
                    }
                    break;

                case VincoloType.BACKEND:
                    throw new Exception("Non ancora sviluppato");
                    break;

                default:
                    throw new Exception("CACHEMANAGER:Tipo di sorgente non valido:");
            }

            return default(T);
        }



        public static bool exist(CacheKeys key)
        {
            bool bRet = false;
            if (HttpContext.Current.Cache[Convert.ToString(key)] != null)
                bRet = true;
            return bRet;
        }

        private static T getXml(CacheKeys XmlName)
        {
            T data = new T();
            string key = Convert.ToString(XmlName);
            if (HttpContext.Current.Cache[key] == null)
            {
                try
                {
                    string path = getResourcePath(key + ".xml", "XSLT");
                    (data as System.Xml.XmlDocument).Load(path);
                    HttpContext.Current.Cache.Add(key, data, new System.Web.Caching.CacheDependency(path), System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                catch (System.Exception errore)
                {
                    _log.Error(errore.ToString());
                    throw new Exception("Impossibile caricare il foglio XSLT.", errore);
                }
            }
            return (T)HttpContext.Current.Cache[key];
        }

        private static T getXslt(CacheKeys XsltName)
        {
            T data = new T();
            string key = Convert.ToString(XsltName);
            if (HttpContext.Current.Cache[key] == null)
            {
                try
                {
                    string path = getResourcePath(key + ".xslt", "XSLT");
                    (data as System.Xml.Xsl.XslCompiledTransform).Load(path);
                    HttpContext.Current.Cache.Add(key, data, new System.Web.Caching.CacheDependency(path), System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                catch (System.Exception errore)
                {
                    _log.Error(errore.ToString());
                    throw new Exception("Impossibile caricare il foglio XSLT.", errore);
                }
            }
            return (T)HttpContext.Current.Cache[key];
        }

        private static T getDatasetFromFileSystem(CacheKeys datasetName)
        {
            T data = new T();
            string key = Convert.ToString(datasetName);
            if (HttpContext.Current.Cache[key] == null)
            {
                try
                {
                    string path = getResourcePath(key + ".xml", "XML");

                    (data as DataSet).ReadXml(path);
                    HttpContext.Current.Cache.Add(key, data, new System.Web.Caching.CacheDependency(path), System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                catch (System.Exception errore)
                {
                    _log.Error(errore.ToString());
                    throw new Exception("Impossibile caricare il Dataset", errore);
                }
            }
            else data = (T)HttpContext.Current.Cache[key];
            return data;
        }

        private static string getResourcePath(string resourceName, string Tipo)
        {
            StartupFoldersConfigSection section = (StartupFoldersConfigSection)ConfigurationManager.GetSection("StartupFolders");
            string where = null, relativepath = string.Empty;

            if (section != null)
            {
                for (int i = 0; i < section.FolderItems.Count; i++)
                    if (section.FolderItems[i].FolderType.Equals(Tipo))
                    {
                        relativepath = HttpContext.Current.Server.MapPath(section.FolderItems[i].Path);
                        where = System.IO.Path.Combine(relativepath, resourceName);
                    }
            }
            if (where == null)
                throw new Exception("Impossibile individuare il percorso di caricamento dei file XSLT: controllare il file di configurazione");
            else
                if (!System.IO.File.Exists(where))
                    throw new Exception("Il file XSLT [" + where + "] non è stato individuato");

            return where;
        }

    }
}
