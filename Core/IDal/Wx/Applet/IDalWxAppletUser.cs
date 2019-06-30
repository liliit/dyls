using DYLS.Model.Db.Wx;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.IDal.Wx.Applet
{
    public interface IDalWxAppletUser:IDalBase<Wx_Applet_User>
    {
        Wx_Applet_User GetByOpenId(string openId);
    }
}
