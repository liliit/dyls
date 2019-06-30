using Dapper;
using DYLS.IDal.Wx.Applet;
using DYLS.Model.Db.Wx;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.Dal.Wx.Applet
{
    public class WxAppletUserDal : BaseDal<Wx_Applet_User>, IDalWxAppletUser
    {
        public Wx_Applet_User GetByOpenId(string openId)
        {
            string sql = $" SELECT * FROM {TableName} WHERE OpenID=@openoid ";
            var param = new DynamicParameters();
            param.Add("openoid",openId);
            return conn.QueryFirstOrDefault<Wx_Applet_User>(sql, param);
        }
    }
}
