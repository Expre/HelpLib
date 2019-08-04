using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;

namespace HelpLib.WebRequest
{
    public class HttpHelper
    {
        private static readonly HttpClient _httpClient;
        static HttpHelper()
        {
            TimeSpan timeSpan = new TimeSpan(0, 3, 0);
            HttpClientHandler httpClientHandler = new HttpClientHandler { UseProxy = false };
            _httpClient = new HttpClient(httpClientHandler) { Timeout = timeSpan, };
        }
        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certifice, chain, sslerror) => true);
        private static RemoteCertificateValidationCallback RemoteCertificateValidationCallback => (sender, certifice, chain, sslerror) => { return true; };
        public static string Post(string url, string json, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            if (url.StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
            HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ProtocolVersion = HttpVersion.Version11;
            request.Timeout = 3 * 60 * 1000;
            request.CookieContainer = new CookieContainer();
            if (headers != null)
            {
                string value;
                foreach (string key in headers.Keys)
                {
                    value = headers[key];
                    switch (key.ToLower())
                    {
                        case "useragent":
                            request.UserAgent = value;
                            break;
                        case "host":
                            request.Host = value;
                            break;
                        case "accept":
                            request.Accept = value;
                            break;
                        case "keepalive":
                            request.KeepAlive = true;
                            break;
                        case "cookie":
                            request.CookieContainer.SetCookies(new Uri(url), value);
                            break;
                        case "cookiecontainer":
                            //request.CookieContainer = JsonConvert.DeserializeObject<CookieContainer>(value);
                            break;
                        default:
                            request.Headers.Add(key, value);
                            break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(json))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = bytes.Length;
                using (Stream s = request.GetRequestStream())
                {
                    s.Write(bytes, 0, bytes.Length);
                }
            }
            WebResponse response = request.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
        public static string Get(string url, string data, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            if (url.StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
            if (!string.IsNullOrEmpty(data))
                url += string.Format("?{0}", data);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "Get";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8;";
            request.ProtocolVersion = HttpVersion.Version11;
            request.Timeout = 3 * 60 * 1000;
            if (headers != null)
            {
                string value;
                foreach (string key in headers.Keys)
                {
                    value = headers[key];
                    switch (key.ToLower())
                    {
                        case "useragent":
                            request.UserAgent = value;
                            break;
                        case "host":
                            request.Host = value;
                            break;
                        case "accept":
                            request.Accept = value;
                            break;
                        case "keepalive":
                            request.KeepAlive = true;
                            break;
                        case "cookie":
                            //request.CookieContainer = JsonConvert.DeserializeObject<CookieContainer>(value);
                            break;
                        default:
                            request.Headers.Add(key, value);
                            break;
                    }
                }
            }
            WebResponse response = request.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
        public static Stream GetImageStream(string url, string data = "")
        {
            if (string.IsNullOrEmpty(url))
                return null;
            if (url.StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
            if (!string.IsNullOrEmpty(data))
                url += string.Format("?{0}", data);
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 3 * 60 * 1000;
            var response = request.GetResponse();
            return response.GetResponseStream();
        }


        #region 向远程地址post多个文件以及键值数据
        /// <summary>
        /// 向远程地址post文件以及键值数据
        /// </summary>
        /// <param name="url">远程url</param>
        /// <param name="namePathKeyValues">文件参数名及本地文件物理地址字典</param>
        /// <param name="paramKeyValues">参数字典</param>
        public static string PostFile(string url, Dictionary<string, string> namePathKeyValues, Dictionary<string, string> paramKeyValues)
        {
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest req = System.Net.WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.Method = "POST";
            req.KeepAlive = true;
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream stream = new System.IO.MemoryStream();
            string startTag = "\r\n--" + boundary + "\r\n";
            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes(startTag);
            stream.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            string header;
            byte[] headerbytes;
            FileStream fileStream;
            byte[] buffer;
            int bytesRead;
            foreach (KeyValuePair<string, string> item in namePathKeyValues)
            {
                header = string.Format(headerTemplate, item.Key, Path.GetFileName(item.Value));
                headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                stream.Write(headerbytes, 0, headerbytes.Length);
                fileStream = new FileStream(item.Value, FileMode.Open, FileAccess.Read);
                buffer = new byte[1024];
                bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                fileStream.Close();
            }

            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
            #region 写入post参数
            if (paramKeyValues != null)
                foreach (string key in paramKeyValues.Keys)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, paramKeyValues[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    stream.Write(formitembytes, 0, formitembytes.Length);
                }
            #endregion
            #region 组装结尾
            string endTag = "\r\n--" + boundary + "--";
            byte[] endByte = Encoding.UTF8.GetBytes(endTag);
            stream.Write(endByte, 0, endByte.Length);
            #endregion
            req.ContentLength = stream.Length;
            Stream requestStream = req.GetRequestStream();
            stream.Position = 0;
            byte[] tempBuffer = new byte[stream.Length];
            stream.Read(tempBuffer, 0, tempBuffer.Length);
            stream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            WebResponse resp = req.GetResponse();

            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                string result = sr.ReadToEnd();
                return result;
            }
        }
        #endregion

        #region 向远程地址post单个文件以及键值数据
        /// <summary>
        /// 向远程地址post文件以及键值数据
        /// </summary>
        /// <param name="url">远程url</param>
        /// <param name="file">文件</param>
        /// <param name="paramKeyValues">参数字典</param>
        public static string PostFile(string url, string fileName, Stream inputStream, Dictionary<string, string> paramKeyValues)
        {
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest req = System.Net.WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.Method = "POST";
            req.KeepAlive = true;
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream stream = new System.IO.MemoryStream();
            string startTag = "\r\n--" + boundary + "\r\n";
            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes(startTag);
            stream.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            string header;
            byte[] headerbytes;
            Stream fileStream;
            byte[] buffer;
            int bytesRead;
            header = string.Format(headerTemplate, Path.GetFileNameWithoutExtension(fileName), fileName);
            headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            stream.Write(headerbytes, 0, headerbytes.Length);
            fileStream = inputStream;
            buffer = new byte[1024];
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }
            stream.Write(boundarybytes, 0, boundarybytes.Length);
            fileStream.Close();

            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
            #region 写入post参数
            if (paramKeyValues != null)
                foreach (string key in paramKeyValues.Keys)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, paramKeyValues[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    stream.Write(formitembytes, 0, formitembytes.Length);
                }
            #endregion
            #region 组装结尾
            string endTag = "\r\n--" + boundary + "--";
            byte[] endByte = Encoding.UTF8.GetBytes(endTag);
            stream.Write(endByte, 0, endByte.Length);
            #endregion
            req.ContentLength = stream.Length;
            Stream requestStream = req.GetRequestStream();
            stream.Position = 0;
            byte[] tempBuffer = new byte[stream.Length];
            stream.Read(tempBuffer, 0, tempBuffer.Length);
            stream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            WebResponse resp = req.GetResponse();

            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                string result = sr.ReadToEnd();
                return result;
            }
        }
        #endregion

        #region 使用HttpClient模拟form表单提交键值和本地多个文件
        public static void ClientPostFile(string url, Dictionary<string, string> namePathKeyValues, Dictionary<string, object> formDataKeyValues)
        {
            using (MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent())
            {
                if (formDataKeyValues != null)
                {
                    foreach (var item in formDataKeyValues)
                    {
                        ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(item.Value.ToString()));
                        //byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("FormData")
                        {
                            Name = item.Key,
                        };
                        multipartFormDataContent.Add(byteArrayContent);
                    }
                }
                if (namePathKeyValues != null)
                {
                    FileStream fileStream;
                    foreach (var item in namePathKeyValues)
                    {
                        fileStream = new FileStream(item.Value.ToString(), FileMode.Open, FileAccess.Read);
                        byte[] bytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            fileStream.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                        ByteArrayContent byteArrayContent = new ByteArrayContent(bytes);
                        //byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("FileData")
                        {
                            Name = item.Key,
                            FileName = System.IO.Path.GetFileName(item.Value)
                        };
                        multipartFormDataContent.Add(byteArrayContent);
                    }
                }

                HttpResponseMessage result = _httpClient.PostAsync(url, multipartFormDataContent).Result;
            }
        }
        #endregion
    }
}
