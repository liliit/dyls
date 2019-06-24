using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Kachannel.Tencentads.Common.Utils
{
    public static class HttpContextHelper
    {

        /// <summary>
        /// 获取当前HttpCurrent
        /// </summary>
        /// <returns></returns>
        public static HttpContext GetCurrent()
        {
            var hca = new HttpContextAccessor();
            return hca.HttpContext;
        }
        /// <summary>
        /// 上下文中存放事务的key
        /// </summary>
        public const string DbTransaction = "DbTransaction";

        /// <summary>
        /// DbConn
        /// </summary>
        public const string DbConnection = "DbConnection";

        /// <summary>
        /// Env
        /// </summary>
        public static IHostingEnvironment Env { get; set; }

        /// <summary>
        /// 从当前上下文中读取信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            var current = GetCurrent();
            if (current != null)
            {
                return (T)GetCurrent().Items[key];
            }
            return default(T);
        }

        /// <summary>
        ///  设置一个值到当前上下文中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            var current = GetCurrent();
            if (current != null)
            {
                current.Items[key] = value;
            }
        }

        /// <summary>
        /// GetPath 相对路径改成绝对路径.  注意相对于当前web项目所在目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPath(string path)
        {
            var directory = Env.ContentRootPath + "\\";
            var result = directory + path;
            var dir = result.Substring(0, result.LastIndexOf("/", StringComparison.Ordinal) + 1);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return directory + path;
        }

        /// <summary>
        /// ReadContent
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ReadContent(HttpRequest request)
        {
            request.EnableRewind();
            request.Body.Position = 0;
            Stream stream = request.Body;
            if (request.ContentLength != null)
            {
                var buffer = new byte[request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                var requestStr = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;
                return requestStr;
            }

            return "";
        }

        /// <summary>
        /// GetUa
        /// </summary>
        /// <returns></returns>
        public static string GetUa()
        {
            var current = GetCurrent().Request;
            return current.Headers.ContainsKey("User-Agent") ? current.Headers["User-Agent"].ToString().ToLower() : "";
        }
    }
}
