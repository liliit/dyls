using Dapper;
using DYLS.IDal.Wx.Applet;
using DYLS.Model.Db.Wx;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.Dal.Wx.Applet
{
    public class WxUserDal : BaseDal<Wx_User>, IDalWxUser
    {
        public Wx_User GetByOpenId(string openId)
        {
            string sql = $" SELECT * FROM {TableName} WHERE Applet_OpenId=@openoid ";
            var param = new DynamicParameters();
            param.Add("openoid",openId);
            return conn.QueryFirstOrDefault<Wx_User>(sql, param);
        }
    }
}
