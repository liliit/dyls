using DYLS.Common.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.Common.Filter.Web.Applet
{
    public class AuthorAttributeFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.ContainsKey("DYLS-Applet") || !request.Headers.ContainsKey("X-Scanner-Ud"))
            {
                context.Result = JsonResultHelper.Fail();
                return;
            }
            string timesTamp = request.Headers["timesTamp"];

            string signature = request.Headers["signature"];

            string appletKey = ConfigHelper.Get(ConfigHelper.Wx_Applet_Key);

            string appId = ConfigHelper.Get(ConfigHelper.Wx_Applet_AppId);

            string apiSignature = SystemHelper.Md5(timesTamp + appletKey + appId);

            if (signature != apiSignature)
            {
                context.Result = JsonResultHelper.Fail();
                return;
            }
        }
    }
}
