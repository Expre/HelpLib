using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace System
{
    public class HttpHelper
    {
        private static readonly HttpClient httpClient;
        static HttpHelper()
        {
            TimeSpan timeSpan = new TimeSpan(0, 1, 0);
            HttpClientHandler httpClientHandler = new HttpClientHandler { UseProxy = false };
            httpClient = new HttpClient(httpClientHandler) { Timeout = timeSpan, };
        }

        #region Post文本
        public static string Post(string url, string data, Dictionary<string, string> headers = null)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certifice, chain, sslerror) => true);
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                if (url.StartsWith("https"))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certifice, chain, sslerror) => { return true; };
                LogHelper.Write("url：\r" + url, "request");
                //LogHelper.WriteLog("data：\r" + data, "request");
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "Post";
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
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;
                    using (Stream s = request.GetRequestStream())
                    {
                        s.Write(bytes, 0, bytes.Length);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    //LogHelper.WriteLog("响应：\r" + result, "request");
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                return null;
            }
        }
        public static string Get(string url, string data, Dictionary<string, string> headers = null)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                if (url.StartsWith("https"))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certifice, chain, sslerror) => { return true; };
                if (!string.IsNullOrEmpty(data))
                    url += string.Format("?{0}", data);
                LogHelper.Write("url：\r" + url, "request");
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "Get";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8;";
                request.ProtocolVersion = HttpVersion.Version11;
                request.Timeout = 20000;
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
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    //LogHelper.WriteLog("响应：\r" + result, "request");
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                return null;
            }
        }
        public static HttpWebResponse GetResponse(string url, string data = "")
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                if (url.StartsWith("https"))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certifice, chain, sslerror) => { return true; };
                if (!string.IsNullOrEmpty(data))
                    url += string.Format("?{0}", data);
                LogHelper.Write("url：\r" + url, "request");
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "Get";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8;";
                request.ProtocolVersion = HttpVersion.Version11;
                request.Timeout = 20000;
                request.CookieContainer = new CookieContainer();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                return null;
            }
        }
        public static Stream GetImageStream(string url, string data = "")
        {
            if (string.IsNullOrEmpty(url))
                return null;
            if (url.StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback += (sender, certifice, chain, sslerror) => { return true; };
            if (!string.IsNullOrEmpty(data))
                url += string.Format("?{0}", data);
            LogHelper.Write("url：\r" + url, "request");
            WebRequest request = WebRequest.Create(url);
            request.Method = "Get";
            request.Timeout = 20000;
            var response = request.GetResponse();
            return response.GetResponseStream();
        }
        #endregion


        #region 向远程地址post文件以及键值数据
        /// <summary>
        /// 向远程地址post文件以及键值数据
        /// </summary>
        /// <param name="url">远程url</param>
        /// <param name="namepath">文件参数名及本地文件物理地址字典</param>
        /// <param name="param">参数字典</param>
        public static string PostFile(string url, Dictionary<string, string> namepath, Dictionary<string, string> param)
        {
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
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
            foreach (KeyValuePair<string, string> item in namepath)
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
            if (param != null)
                foreach (string key in param.Keys)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, param[key]);
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

        #region 向远程地址post文件以及键值数据
        /// <summary>
        /// 向远程地址post文件以及键值数据
        /// </summary>
        /// <param name="url">远程url</param>
        /// <param name="file">文件</param>
        /// <param name="param">参数字典</param>
        public static string PostFile(string url, Stream inputStream, string fileName, Dictionary<string, string> param)
        {
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
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
            if (param != null)
                foreach (string key in param.Keys)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, param[key]);
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

        #region 使用HttpClient模拟form表单提交键值和本地文件
        public static void ClientPostFile(string url, Dictionary<string, string> fileData, Dictionary<string, object> formData)
        {
            using (MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent())
            {
                if (formData != null)
                {
                    foreach (var item in formData)
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
                if (fileData != null)
                {
                    FileStream fileStream;
                    foreach (var item in fileData)
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

                HttpResponseMessage result = httpClient.PostAsync(url, multipartFormDataContent).Result;
            }
        }
        #endregion
    }
}
