

using System.IO;
using Microsoft.Extensions.Configuration;

namespace DYLS.Common.Utils
{
    public static class ConfigHelper
    {
        private static string _fix = "Consumer:";

        /// <summary>
        /// PublicKey
        /// </summary>
        public const string PublicKey = "Rsa:PublicKey";

        /// <summary>
        /// PrivateKey
        /// </summary>
        public const string PrivateKey = "Rsa:PrivateKey";

        /// <summary>
        /// CookieDomain
        /// </summary>
        public const string CookieDomain = "Web:CookieDomain";

        /// <summary>
        /// WebAutoHttps
        /// </summary>
        public const string WebAutoHttps = "Web:AutoHttps";

        /// <summary>
        /// WebBlackIps
        /// </summary>
        public const string WebBlackIps = "Web:WebBlackIps";

        /// <summary>
        /// SecurityHost
        /// </summary>
        public const string SecurityHost = "Web:SecurityHost";

        /// <summary>
        /// WebAppId
        /// </summary>
        public const string WebAppId = "Web:WebAppId";

        /// <summary>
        /// PageCacheDuration
        /// </summary>
        public const string PageCacheDuration = "Web:PageCache";

        /// <summary>
        /// UseUrl
        /// </summary>
        public const string UseUrl = "Web:UseUrl";

        /// <summary>
        /// WebHost
        /// </summary>
        public const string WebHost = "Web:Host";

        /// <summary>
        /// RedisCoonstr
        /// </summary>
        public const string RedisCoonstr = "Redis:Conn";

        /// <summary>
        /// RedisKeyPrefix
        /// </summary>
        public static string RedisKeyPrefix = "Redis:Fix";

        /// <summary>
        /// DbConnectionCrm
        /// </summary>
        public static string DbConnection = "Db:Connection";

        /// <summary>
        /// WxAppId
        /// </summary>
        public static string Wx_Applet_AppId = "Wx_Applet:AppId";

        /// <summary>
        /// WxAppSecret
        /// </summary>
        public static string Wx_Applet_AppSecret = "Wx_Applet:AppSecret";

        /// <summary>
        /// _configuration
        /// </summary>
        private static readonly IConfigurationRoot Configuration;


        static ConfigHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>取到的值</returns>
        public static string Get(string key)
        {
            return Configuration.GetValue<string>(_fix + key);
        }
    }
}