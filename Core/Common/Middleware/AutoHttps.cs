using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
    public class AutoHttps
    {
        /// <summary>
        /// 代理 架构要求的
        /// </summary>
        private readonly RequestDelegate _next;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="next"></param>
        public AutoHttps(RequestDelegate next)
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
            if (context.Request.Method == "GET" && ConfigHelper.Get(ConfigHelper.WebAutoHttps) == "1")
            {
                var url = SystemHelper.GetCurrentUrl();
                if (!string.IsNullOrEmpty(url) && url.IndexOf("http://", StringComparison.Ordinal) == 0)
                {
                    context.Response.Redirect(new Regex("http").Replace(url, "https", 1));
                    return;
                }
            }
            await _next(context);
        }

    }
}
