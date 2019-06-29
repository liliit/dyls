using DYLS.Common.Utils;
using DYLS.Common.Wx.Applet;
using DYLS.Model.Request.Wx.Applet;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.AppletApiController
{
    public class MyController:ControllerBase
    {
        public ActionResult Login([FromBody] dynamic code)
        {
            Wx_Login login= WxHelper.Login(code.code.ToString());
            if(login.Errcode==0)
            {
                return JsonResultHelper.Success(login);
            }
            else
            {
                return JsonResultHelper.Error(login);
            }
        }

        public ActionResult UserInfo()
        {
            return JsonResultHelper.Success();
        }
    }
}
