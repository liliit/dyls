

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace DYLS.Common.Utils
{
    public static class SystemHelper
    {
        /// <summary>
        ///     Md5 32位加密
        /// </summary>
        public const int Md5Len32 = 32;

        /// <summary>
        ///     Md5 16位加密
        /// </summary>
        public const int Md5Len16 = 16;

        /// <summary>
        /// XmlToDict
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlToDict(string xml)
        {
            //创建xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var dict = new Dictionary<string, string>();
            foreach (var xmlDocChildNode in xmlDoc.FirstChild.ChildNodes)
            {
                var element = (XmlElement)xmlDocChildNode;
                dict.Add(element.Name, element.InnerText);
            }
            return dict;
        }

        /// <summary>
        /// 将 Unix 时间, 变成C#时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>    
        /// DateTime时间格式转换为Unix时间戳格式    
        /// </summary>    
        /// <param name="time"> DateTime时间格式</param>    
        /// <returns>Unix时间戳格式</returns>    
        public static int DateTimeToUnixTime(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetIp()
        {
            string userHostAddress = "";
            var httpXForwardedFor = HttpContextHelper.GetCurrent().Request.Headers["X-Forwarded-For"].ToString();
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            if (!string.IsNullOrEmpty(httpXForwardedFor))
            {
                userHostAddress = httpXForwardedFor.Split(':')[0].Trim();
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContextHelper.GetCurrent().Connection.RemoteIpAddress.ToString();
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        ///     将一个字符串, 执行md5加密
        /// </summary>
        /// <param name="str">要加密的串</param>
        /// <param name="len">Md5Len32 or Md5Len16</param>
        /// <returns>返回加密后的串, 小写形式</returns>
        public static string Md5(string str, int len = Md5Len32)
        {
            var md5 = new MD5CryptoServiceProvider();
            var result = Encoding.UTF8.GetBytes(str);
            var output = md5.ComputeHash(result);
            var value = BitConverter.ToString(output).Replace("-", "").ToLower();
            if (len == 32)
            {
                return value;
            }
            value = value.Substring(8, 16);
            return value;
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sha1(string str)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_in = Encoding.UTF8.GetBytes(str);
            byte[] bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();
            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            return result;
        }

        /// <summary>
        /// base64
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str)
        {
            string code = "";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            code = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            return code;
        }

        /// <summary>
        ///     返回当前访问者的客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            return GetIp();
        }

        /// <summary>
        /// 获取当前请求的url
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUrl()
        {
            var path = HttpContextHelper.GetCurrent().Request.Headers["X-Original-URL"];
            var proto = HttpContextHelper.GetCurrent().Request.Headers["X-Client-Proto"];
            var url = "";
            // LogHelper.Debug("1GetCurrentUrl path=" + path);
            // LogHelper.Debug("1GetCurrentUrl proto=" + proto);
            if (string.IsNullOrEmpty(path))
            {
                var request = HttpContextHelper.GetCurrent().Request;
                //为空, 走正常
                url = string.Concat(
                    request.Scheme,
                    "://",
                    request.Host.ToUriComponent(),
                    request.PathBase.ToUriComponent(),
                    request.Path.ToUriComponent(),
                    request.QueryString.ToUriComponent());
            }
            else
            {
                url = ConfigHelper.Get(ConfigHelper.WebHost) + path;
            }

            if (string.IsNullOrEmpty(url))
            {
                url = ConfigHelper.Get(ConfigHelper.WebHost);
            }
            //url = url.ToLower();
            if (!string.IsNullOrEmpty(proto))
            {
                if (url.IndexOf("http://", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    url = Regex.Replace(url, "http://", proto + "://", RegexOptions.IgnoreCase);
                }
                else if (url.IndexOf("https://", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    url = Regex.Replace(url, "https://", proto + "://", RegexOptions.IgnoreCase);
                }
            }
            //LogHelper.Debug("1GetCurrentUrl url=" + url);
            return url;

        }

        /// <summary>
        /// 当前时间的日期部分
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDate()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
        }


        /// <summary>
        /// 用时间形式返回一个编号
        /// </summary>
        /// <param name="fix"></param>
        /// <param name="len">生成的数字长度, 默认是8</param>
        /// <returns></returns>
        public static string GenNumber(string fix = "", short len = 8)
        {
            var prefix = fix;
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return prefix + String.Format("{0:D" + len + "}", random);
        }

        /// <summary>
        /// GetRefererUrl
        /// </summary>
        /// <returns></returns>
        public static string GetRefererUrl()
        {
            var headers = HttpContextHelper.GetCurrent().Request.Headers;
            if (headers.ContainsKey("Referer"))
            {
                return headers["Referer"];
            }
            if (headers.ContainsKey("referer"))
            {
                return headers["referer"];
            }

            return "";
        }

        /// <summary>
        /// GetGuid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid(byte len = 32)
        {
            var g = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            if (len == 16)
            {
                return g.Substring(8, 16);
            }
            return g;
        }

        /// <summary>
        /// CheckFromUrl
        /// </summary>
        /// <param name="fromurl"></param>
        /// <returns></returns>
        public static bool CheckFromUrl(string fromurl)
        {
            if (string.IsNullOrEmpty(fromurl))
            {
                return false;
            }

            var pass = false;
            var host = ConfigHelper.Get(ConfigHelper.SecurityHost);

            foreach (var s in host.Split(','))
            {
                if (s.Contains(fromurl) || fromurl.Contains(s))
                {
                    pass = true;
                    break;
                }
            }

            return pass;
        }

        /// <summary>
        /// ConvertImagePath
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string ConvertImagePath(string image, int width, int height)
        {
            if (string.IsNullOrEmpty(image) || image.IndexOf(".", StringComparison.Ordinal) == -1)
            {
                return "";
            }
            var extName = image.Substring(image.LastIndexOf(".", StringComparison.Ordinal));
            var embed = "_" + width + "_" + height + "_auto";
            if (image.IndexOf("_", StringComparison.Ordinal) == -1)
            {
                return image.Substring(0, image.IndexOf(".", StringComparison.Ordinal)) + embed + extName;
            }
            return image.Substring(0, image.IndexOf("_", StringComparison.Ordinal)) + embed + extName;
        }

        /// <summary>
        /// GetCurrentSource
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentSource()
        {
            var source = HttpUtility.HtmlEncode(HttpContextHelper.GetCurrent().Request.Query["name"]);
            if (string.IsNullOrEmpty(source))
            {
                source = CookieHelper.Get(CookieHelper.WebSource);
            }
            else
            {
                CookieHelper.Set(CookieHelper.WebSource, source);
            }
            return source;
        }

        /// <summary>
        /// 用来生成一个导出的文件名
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExportFileName(string dir, out string path)
        {
            path = "/Files/Export/" + dir;
            var guide = Guid.NewGuid().ToString();
            path += "/" + guide + ".xlsx";
            return HttpContextHelper.GetPath(path);
        }
    }
}