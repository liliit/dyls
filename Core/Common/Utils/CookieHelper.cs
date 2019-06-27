

using System;
using Microsoft.AspNetCore.Http;

namespace DYLS.Common.Utils
{
    public class CookieHelper
    {

        /// <summary>
        /// WebUserKey
        /// </summary>
        public const string WebUserKey = "webuser";

        /// <summary>
        /// WxUserKey
        /// </summary>
        public const string WxUserKey = "wxuser";

        /// <summary>
        /// WebSource
        /// </summary>
        public const string WebSource = "websource";


        /// <summary>
        /// ManageUserKey
        /// </summary>
        public const string ManageUserKey = "manageuser";

        /// <summary>
        /// Session
        /// </summary>
        public const string Session = "ka_session";

        /// <summary>
        /// WebSession
        /// </summary>
        public const string WebSession = "kaw_session";


        /// <summary>
        /// 从cookie中拿到一个值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            if (HttpContextHelper.GetCurrent().Request.Cookies.ContainsKey(key))
            {
                var source = HttpContextHelper.GetCurrent().Request.Cookies[key];
                if (string.IsNullOrEmpty(source))
                {
                    return "";
                }
                var result = "";
                try
                {
                    result = RsaHelper.DecryptData(source, RsaHelper.privateKey);
                }
                catch
                {
                    // ignored
                }
                return result;
            }
            return "";
        }

        /// <summary>
        /// 往cookie中设置一个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Set(string key, string value)
        {
            var result = "";

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    result = RsaHelper.EncryptData(value, RsaHelper.publicKey);
                }
                catch
                {
                    // ignored
                }
            }


            HttpContextHelper.GetCurrent().Response.Cookies.Append(key, result, new CookieOptions()
            {
                Domain = ConfigHelper.Get(ConfigHelper.CookieDomain),
                Path = "/"
            });
        }


        /// <summary>
        /// 往cookie中设置一个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static void Set(string key, string value, DateTime expires)
        {
            var result = "";

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    result = RsaHelper.EncryptData(value, RsaHelper.publicKey);
                }
                catch
                {
                    // ignored
                }
            }

            HttpContextHelper.GetCurrent().Response.Cookies.Append(key, result, new CookieOptions()
            {
                Domain = ConfigHelper.Get(ConfigHelper.CookieDomain),
                Path = "/",
                Expires = expires
            });
        }

        /// <summary>
        /// 清掉一个key
        /// </summary>
        /// <param name="key"></param>
        public static void Clear(string key)
        {
            HttpContextHelper.GetCurrent().Response.Cookies.Append(key, null, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1)
            });
        }
    }
}
