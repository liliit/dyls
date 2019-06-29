using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DYLS.Common.Utils
{
    public class HttpHelper
    {
        private const string SUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private const string SContentType = "application/json";

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="url"></param>
        public static string DownFile(string url, string name)
        {
            WebClient client = new WebClient();
            try
            {
                Stream str = client.OpenRead(url);
                StreamReader reader = new StreamReader(str);
                byte[] mbyte = new byte[1000000];
                int allmybyte = (int)mbyte.Length;
                int startmbyte = 0;

                if (allmybyte <= 0)
                {
                    return "";
                }

                while (allmybyte > 0)
                {
                    int m = str.Read(mbyte, startmbyte, allmybyte);
                    if (m == 0)
                    {
                        break;
                    }
                    startmbyte += m;
                    allmybyte -= m;
                }

                reader.Dispose();
                str.Dispose();

                string path = HttpContextHelper.GetPath(name);
                if (File.Exists(path))
                {
                    return path;
                }

                FileStream fstr = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fstr.Write(mbyte, 0, startmbyte);
                fstr.Flush();
                fstr.Close();
                return path;
            }
            catch
            {
                return "";
            }


        }

        /// <summary>
        ///     发起一个Get请求, 返回字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, string> header = null)
        {
            var str = "";
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                return str;
            }
            request.Method = "GET";
            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            if (stream == null)
            {
                return str;
            }
            var sr = new StreamReader(stream);
            str = sr.ReadToEnd();
            return str;
        }

        /// <summary>
        /// 发起post请求, 并带有一个证书
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="certPath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Posts(string url, Dictionary<string, string> dic, string certPath, string password)
        {
            ServicePointManager.ServerCertificateValidationCallback = ValidationResult;

            //证书
            X509Certificate2 cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            //请求对象
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.ClientCertificates.Add(cert);
            req.Method = "POST";

            //准备数据
            var content = @"<xml>";
            foreach (var obj in dic)
            {
                content += "<" + obj.Key + "><![CDATA[" + obj.Value + "]]></" + obj.Key + ">";
            }
            content += "</xml>";
            var bytes = Encoding.UTF8.GetBytes(content);
            req.ContentLength = bytes.Length;

            //发送
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();
            }

            //响应
            string responseData;
            using (var response = (HttpWebResponse)req.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseData = reader.ReadToEnd();
                }
            }
            return responseData;
        }
        private static bool ValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        ///<summary>
        /// Post data到url
        ///</summary>
        ///<param name="data">要post的数据</param>
        ///<param name="url">目标url</param>
        ///<param name="contentType">contentType</param>
        ///<returns>服务器响应</returns>
        public static string PostDataToUrlUtf8(string data, string url, string contentType = "")
        {
            Encoding encoding = Encoding.UTF8;
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostDataToUrlUtf8(bytesToPost, url, contentType);
        }

        public static byte[] Post(string url, string msg)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json;charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 2000;
            byte[] btBodys = Encoding.UTF8.GetBytes(msg);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream streamReaderPost = httpWebResponse.GetResponseStream();

            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                streamReaderPost.CopyTo(stream);
                bytes = stream.ToArray();
            }

            httpWebResponse.Close();
            streamReaderPost.Close();
            httpWebRequest.Abort();
            return bytes;
        }

        ///<summary>
        /// Post data到url
        ///</summary>
        ///<param name="data">要post的数据</param>
        ///<param name="url">目标url</param>
        ///<param name="contentType">contentType</param>
        ///<returns>服务器响应</returns>
        public static string PostDataToUrlUtf8(byte[] data, string url, string contentType)
        {

            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }

            httpRequest.KeepAlive = false;
            httpRequest.UserAgent = SUserAgent;
            httpRequest.ContentType = string.IsNullOrEmpty(contentType) ? SContentType : contentType;
            httpRequest.Method = "POST";

            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            Stream responseStream;
            //var response = httpRequest.GetResponse();
            //var contentType = response.Headers["Content-Type"];

            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                // ignored
                LogHelper.Error("PostDataToUrlUtf8 Error to " + url + "-------------" + e.Message);
                return "";
            }

            string stringResponse = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();

            return stringResponse;
        }
    }
}