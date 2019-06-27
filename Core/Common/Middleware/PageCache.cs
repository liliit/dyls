
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DYLS.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace DYLS.Common.Middleware
{
    /// <summary>
    /// PageCache 中间件
    /// </summary>
    public class PageCache
    {
        /// <summary>
        /// 代理 架构要求的
        /// </summary>
        private readonly RequestDelegate _next;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="next"></param>
        public PageCache(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 主调用方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                await _next(context);
                return;
            }

            var url = SystemHelper.GetCurrentUrl();
            if (string.IsNullOrEmpty(url) || url.ToLower().Contains("/my"))
            {
                await _next(context);
                return;
            }
            var query = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal));
            if (string.IsNullOrEmpty(query) || query.Contains('.'))
            {
                await _next(context);
                return;
            }

            var key = SystemHelper.Md5(url, SystemHelper.Md5Len16);
            if (RedisHelper.Exists(key))
            {
                await _next(context);
                return;
            }

            using (var responseBody = new MemoryStream())
            {
                var original = context.Response.Body;
                context.Response.Body = responseBody;
                await _next(context);

                if (context.Response.StatusCode != 200)
                {
                    return;
                }

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var bytes = new byte[context.Response.Body.Length];
                context.Response.Body.Read(bytes, 0, bytes.Length);

                if (!RedisHelper.Lock(RedisHelper.PageCacheLock + key))
                {
                    original.Write(bytes, 0, bytes.Length);
                    return;
                }

                var response = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                RedisHelper.Set(key, response, new TimeSpan(0, 0, int.Parse(ConfigHelper.Get(ConfigHelper.PageCacheDuration))));
                RedisHelper.LockRelease(RedisHelper.PageCacheLock + key);
                //context.Response.Redirect(url);
                original.Write(bytes, 0, bytes.Length);
            }

        }

    }
}
