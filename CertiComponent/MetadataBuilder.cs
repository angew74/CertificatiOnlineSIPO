using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Com.Unisys.CdR.Certi.Objects;



namespace CertificatiComponent
{
    public class MetadataBuilder
    {
        public MetadataBuilder()
        {
        }

        public static Byte[] CreateMetadata(CertificatoMDData doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CertificatoMDData));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, doc);
            Byte[] ret = stream.ToArray();

            if (ret.Length % 2 != 0)
            {
                byte[] app = new byte[ret.Length + 1];
                int len = ret.Length;
                for (int i = 0; i < len; i++)
                {
                    app[i] = ret[i];
                }

                app[len] = 32;
                ret = app;
            }

            stream.Close();

            return ret;
        }


        public static Byte[] CreateCustomBody(CertificatoStore body)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CertificatoStore));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, body);
            Byte[] ret = stream.ToArray();
            stream.Close();

            return ret;
        }
    }
}
