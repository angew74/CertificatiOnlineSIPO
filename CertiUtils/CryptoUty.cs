using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.Utils
{
    public enum EncType
    {
        Trentadue = 1,
        Sessantaquattro = 2
    }

    public class CryptoUty
    {
        private const int IN_BYTE_SIZE = 8;
        private const int OUT_BYTE_SIZE = 5;
        private static char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

        public static string Encode32(byte[] data)
        {
            int i = 0, index = 0, digit = 0;
            int current_byte, next_byte;
            StringBuilder result = new StringBuilder((data.Length + 7) * IN_BYTE_SIZE / OUT_BYTE_SIZE);

            while (i < data.Length)
            {
                current_byte = (data[i] >= 0) ? data[i] : (data[i] + 256); // Unsign

                /* Is the current digit going to span a byte boundary? */
                if (index > (IN_BYTE_SIZE - OUT_BYTE_SIZE))
                {
                    if ((i + 1) < data.Length)
                        next_byte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
                    else
                        next_byte = 0;

                    digit = current_byte & (0xFF >> index);
                    index = (index + OUT_BYTE_SIZE) % IN_BYTE_SIZE;
                    digit <<= index;
                    digit |= next_byte >> (IN_BYTE_SIZE - index);
                    i++;
                }
                else
                {
                    digit = (current_byte >> (IN_BYTE_SIZE - (index + OUT_BYTE_SIZE))) & 0x1F;
                    index = (index + OUT_BYTE_SIZE) % IN_BYTE_SIZE;
                    if (index == 0)
                        i++;
                }
                result.Append(alphabet[digit]);
            }

            return result.ToString();
        }

        public static string getEncodedHash(byte[] document, EncType enc)
        {
            byte[] hash = System.Security.Cryptography.SHA1Managed.Create().ComputeHash(document);
            if (enc == EncType.Trentadue) return Encode32(hash);
            else return Convert.ToBase64String(hash);
        }

        public static string getEncodedHash(System.IO.Stream document, EncType enc)
        {
            document.Position = 0;
            byte[] hash = System.Security.Cryptography.SHA1Managed.Create().ComputeHash(document);
            if (enc == EncType.Trentadue) return Encode32(hash);
            else return Convert.ToBase64String(hash);
        }

        public static string PlainToSHA1(string Stringa)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            System.Text.Encoding objEncoding = System.Text.Encoding.UTF8;
            byte[] pwHashed = sha.ComputeHash(objEncoding.GetBytes(Stringa));
            return Convert.ToBase64String(pwHashed);
        }

        public static string getEncodedHash(string document, EncType enc)
        {
            System.Text.UTF8Encoding objEnc = new UTF8Encoding();
            byte[] hash = System.Security.Cryptography.SHA1Managed.Create().ComputeHash(objEnc.GetBytes(document));
            if (enc == EncType.Trentadue) return Encode32(hash);
            else return Convert.ToBase64String(hash);
        }
    }
}
