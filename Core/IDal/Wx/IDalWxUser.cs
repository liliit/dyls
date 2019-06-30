using DYLS.Model.Db.Wx;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.IDal.Wx.Applet
{
    public interface IDalWxUser:IDalBase<Wx_User>
    {
        Wx_User GetByOpenId(string openId);
    }
}
