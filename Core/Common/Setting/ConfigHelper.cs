using System.IO;
using Microsoft.Extensions.Configuration;

namespace Kachannel.Tencentads.Common.Utils
{
    public static class ConfigHelper
    {

        private static string _fix = "Consumer:";
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
        /// DbConnectionKa
        /// </summary>
        public static string DbConnection = "Db:Connection";

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