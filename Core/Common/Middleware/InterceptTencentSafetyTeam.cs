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
    /// InterceptTencentSafetyTeam 中间件
    /// </summary>
    public class InterceptTencentSafetyTeam
    {
        /// <summary>
        /// 代理 架构要求的
        /// </summary>
        private readonly RequestDelegate _next;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="next"></param>
        public InterceptTencentSafetyTeam(RequestDelegate next)
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
            var ips = ConfigHelper.Get(ConfigHelper.WebBlackIps);
            var ip = SystemHelper.GetClientIp();
            var ua = HttpContextHelper.GetUa();
            if (context.Request.Headers.ContainsKey("Tencent-Leakscan") || context.Request.Headers.ContainsKey("X-Scanner-Uuid") || ua.Contains("tencent_security_team") || ips.Contains(ip))
            {
                await context.Response.WriteAsync("success");
                return;
            }
            await _next(context);
        }

    }
}
