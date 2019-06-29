using DYLS.Common.Utils;
using DYLS.Model.Request.Wx.Applet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.Common.Wx.Applet
{
    public class WxHelper
    {
        public static Wx_Login Login(string code)
        {
            string appid = ConfigHelper.Get(ConfigHelper.Wx_Applet_AppId);
            string appSecret = ConfigHelper.Get(ConfigHelper.Wx_Applet_AppSecret);
            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={appid}&secret={appSecret}&js_code={code}&grant_type=authorization_code";
            string res= HttpHelper.Get(url);
            return JsonConvert.DeserializeObject<Wx_Login>(res);
        }
    }
}
