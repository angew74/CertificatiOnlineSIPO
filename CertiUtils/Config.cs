using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Com.Unisys.CdR.Certi.Utils
{
    /// <summary>
    /// Classe generica di utilità che consente di leggere dal file di configurazione 
    /// dell'applicazione web o altro da cui la dll viene referenziata
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Costruttore di default
        /// </summary>
        public Config()
        {
        }

        /// <summary>
        /// Classe che ricerca e carica dal file di configurazione la stringa 
        /// corrispondente alla chiave specificata in input
        /// </summary>
        /// <param name="key">Parametro da ricerca sul file di configurazione</param>
        /// <returns><c>string</c>Il valore contenuto nella stringa di configurazione</returns>

        public static string ReadSetting(string key)
        {

            string val = null;
            try
            {
                AppSettingsReader myConfig = new AppSettingsReader(); 
                val = (string)myConfig.GetValue(key, typeof(string));   
                if (val == "")
                {
                    throw new Exception("Stringa corrispondente alla chiave '" + key + "' - vuota.");
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new Exception("File di configurazione non trovato!", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recuperare la stringa corrispondente alla chiave '" + key + "'. Dettagli per il servizio tecnico:" + ex.Message);
            }
            return val;
        }

        public static string PlainToSHA1(string Stringa)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            System.Text.Encoding objEncoding = System.Text.Encoding.UTF8;
            byte[] pwHashed = sha.ComputeHash(objEncoding.GetBytes(Stringa));
            return Convert.ToBase64String(pwHashed);
        }
         /* Shared Function PlainToSHA1(ByVal Stringa As String) As String
            Dim sha As New System.Security.Cryptography.SHA1CryptoServiceProvider()
            Dim objEncoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Dim pwHashed() As Byte = sha.ComputeHash(objEncoding.GetBytes(Stringa))
            Return System.Convert.ToBase64String(pwHashed)
        End Function*/
    }
}
