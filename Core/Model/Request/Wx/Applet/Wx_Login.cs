using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.Model.Request.Wx.Applet
{
    [Serializable]
    public class Wx_Login
    {
        public string Openid { get; set; }
        public string Session_key { get; set; }
        public string unionid { get; set; }

        /// <summary>
        /// -1	系统繁忙，此时请开发者稍候再试	
        /// 0	请求成功	
        /// 40029	code 无效	
        /// 45011	频率限制，每个用户每分钟100次
        /// </summary>
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }
}
