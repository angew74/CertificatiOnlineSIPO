using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace CertiWebTest
{
    public class WebFormatter
    {
        public Encoding encoding = Encoding.UTF8;
        /// <summary>    
        /// Effettua post di dati come multipart form    
        /// i parametri post di tipo byte[] vengono passati come file mentr i valori di tipo stringa come 
        /// copie nome/valore

        /// </summary>  
        public HttpWebResponse PostMultipartFormData(string postUrl, List<PostDataParam> postParameters)
        {
            string formDataBoundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = this.GetMultipartFormData(postParameters, formDataBoundary);
            return this.PostForm(postUrl, contentType, formData);
        }
        /// <summary> 
        /// 
        /// Post a form 
        /// /// </summary>
        private HttpWebResponse PostForm(string postUrl, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            if (request == null)
            {
                throw new NullReferenceException("La richiesta non è del tipo http request");
            }
            request.Method = "POST";  
            request.ContentType = contentType;
            request.CookieContainer = new CookieContainer();

            request.ContentLength = formData.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// Prepara la richiesta multipart
        /// </summary>    
        private byte[] GetMultipartFormData(List<PostDataParam> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            string start = String.Empty;
            foreach (PostDataParam param in postParameters)
            {
                if (param.Type == PostDataParamType.File)
                {
                    byte[] fileData = param.Value as byte[];
                  
                    string header = string.Format(start + "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary, param.Key, param.Key);
                    formDataStream.Write(encoding.GetBytes(header), 0, header.Length);
                    formDataStream.Write(fileData, 0, fileData.Length);
                }
                else
                {
                    string postData = string.Format(start + "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", boundary, param.Key, param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, postData.Length);
                }
                start = "\r\n"; 
            }
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);

            // Dump the Stream into a byte[]     
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }
    }
}
