using DYLS.Common.Utils;
using DYLS.Common.Wx.Applet;
using DYLS.IDal;
using DYLS.IDal.Wx.Applet;
using DYLS.Model.Db.Wx;
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
                var dalWxUser = DalFactory.GetInstance<IDalWxAppletUser>();

                var wxUser = dalWxUser.GetByOpenId(login.Openid);
                if(wxUser==null)
                {
                    dalWxUser.Insert(new Wx_Applet_User
                    {
                        OpenId=login.Openid,
                        UnionId=login.unionid
                    });
                }

                return JsonResultHelper.Success(login);
            }
            else
            {
                return JsonResultHelper.Error(login);
            }
        }

        public ActionResult UserInfo([FromBody] Wx_Applet_User user)
        {
            long? t = 0;
            var dalWxUser = DalFactory.GetInstance<IDalWxAppletUser>();
            var wxUser = dalWxUser.GetByOpenId(user.OpenId);
            if (wxUser == null)
            {
                t=dalWxUser.Insert(user);
            }
            else
            {
                user.Id = wxUser.Id;
                t=dalWxUser.Update(user);
            }

            if(t.Value>0)
                return JsonResultHelper.Success(user);
            
            else
                return JsonResultHelper.Error(user);
        }
    }
}
