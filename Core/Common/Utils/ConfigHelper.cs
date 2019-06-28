

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
        /// InternalApiHost
        /// </summary>
        public const string InternalApiHost = "Web:InternalApiHost";

        /// <summary>
        /// WebTitle
        /// </summary>
        public const string WebTitle = "Web:WebTitle";

        /// <summary>
        /// SecurityHost
        /// </summary>
        public const string SecurityHost = "Web:SecurityHost";

        /// <summary>
        /// LoginUrl
        /// </summary>
        public const string WebLoginUrl = "Web:LoginUrl";

        /// <summary>
        /// WebAutoHttps
        /// </summary>
        public const string WebAutoHttps = "Web:AutoHttps";

        /// <summary>
        /// WebBlackIps
        /// </summary>
        public const string WebBlackIps = "Web:WebBlackIps";


        /// <summary>
        /// WebAppId
        /// </summary>
        public const string WebAppId = "Web:WebAppId";


        /// <summary>
        /// DevMobile
        /// </summary>
        public const string DevMobile = "Dev:Mobile";

        /// <summary>
        /// WebKeywords
        /// </summary>
        public const string WebKeywords = "Web:WebKeywords";

        /// <summary>
        /// FileServerUrl
        /// </summary>
        public const string FileServerUrl = "Web:FileServerUrl";

        /// <summary>
        /// WebDescription
        /// </summary>
        public const string WebDescription = "Web:WebDescription";

        /// <summary>
        /// PageCacheDuration
        /// </summary>
        public const string PageCacheDuration = "Web:PageCache";

        /// <summary>
        /// ShareTitle
        /// </summary>
        public const string ShareTitle = "Web:ShareTitle";

        /// <summary>
        /// ShareDesc
        /// </summary>
        public const string ShareDesc = "Web:ShareDesc";

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
        /// LoginTryMaxNum
        /// </summary>
        public static string LoginTryMaxNum = "ManageLogin:TryMaxNum";

        /// <summary>
        /// LoginLockDuration
        /// </summary>
        public static string LoginLockDuration = "ManageLogin:LockDuration";

        /// <summary>
        /// FileUploadSize
        /// </summary>
        public static string FileUploadSize = "Upload:FileSize";

        /// <summary>
        /// ImageUploadSize
        /// </summary>
        public static string ImageUploadSize = "Upload:ImageSize";

        /// <summary>
        /// FileSavePos
        /// </summary>
        public static string FileSavePos = "Upload:Pos";

        /// <summary>
        /// WxAppId
        /// </summary>
        public static string WxAppId = "Wx:AppId";

        /// <summary>
        /// RemoteAgency
        /// </summary>
        public static string WxRemoteAgency = "Wx:RemoteAgency";

        /// <summary>
        /// AuthorizationHost
        /// </summary>
        public static string WxAuthorizationHost = "Wx:AuthorizationHost";

        /// <summary>
        /// WxAppSecret
        /// </summary>
        public static string WxAppSecret = "Wx:AppSecret";

        /// <summary>
        /// WxMpToken
        /// </summary>
        public static string WxMpToken = "Wx:MpToken";

        /// <summary>
        /// WxIsOnline
        /// </summary>
        public static string IsOnline = "Wx:IsOnline";

        /// <summary>
        /// WxOpenId
        /// </summary>
        public static string WxOpenId = "Wx:OpenId";


        public static string MailFrom = "Mail:From";
        public static string MailFromMail = "Mail:FromMail";
        public static string MailSmtp = "Mail:Smtp";
        public static string MailSmtpAccount = "Mail:SmtpAccount";
        public static string MailSmtpPwd = "Mail:SmtpPwd";


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