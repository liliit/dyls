

using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace DYLS.Common.Utils
{
    public static class RedisHelper
    {

        private static string Coonstr = ConfigHelper.Get(ConfigHelper.RedisCoonstr);
        private static string Prefix = ConfigHelper.Get(ConfigHelper.RedisKeyPrefix);

        private static object _locker = new Object();
        private static ConnectionMultiplexer _instance = null;

        /// <summary>
        /// JsApiTicket
        /// </summary>
        public const string JsApiTicket = "JsApiTicket";

        /// <summary>
        /// UpdateContentCountLock
        /// </summary>
        public const string UpdateContentCountLock = "UpdateContentCountLock";

        /// <summary>
        /// UpdateQuestionnaireCountLock
        /// </summary>
        public const string UpdateQuestionnaireCountLock = "UpdateQuestionnaireCountLock";



        /// <summary>
        /// UserInfoMark
        /// </summary>
        public const string UserInfoMark = "UserInfoMark";

        /// <summary>
        /// FacilitatorWeight
        /// </summary>
        public const string FacilitatorWeight = "FacilitatorWeight";

        /// <summary>
        /// UrlKey
        /// </summary>
        public const string UrlKey = "UrlKey";

        /// <summary>
        /// JsApiTicketLock
        /// </summary>
        public const string JsApiTicketLock = "JsApiTicketLock";

        /// <summary>
        /// WxAccessToken
        /// </summary>
        public const string WxAccessToken = "WxAccessToken";

        /// <summary>
        /// WxAccessTokenLock
        /// </summary>
        public const string WxAccessTokenLock = "WxAccessTokenLock";

        /// <summary>
        /// MenuList
        /// </summary>
        public const string MenuList = "MenuList";

        /// <summary>
        /// MenuListLock
        /// </summary>
        public const string MenuListLock = "MenuListLock";

        /// <summary>
        /// AdminUser
        /// </summary>
        public const string AdminUser = "AdminUser";

        /// <summary>
        /// WebUser
        /// </summary>
        public const string WebUser = "WebUser";

        /// <summary>
        /// AdminId
        /// </summary>
        public const string AdminId = "AdminId";

        /// <summary>
        /// GameId
        /// </summary>
        public const string GameId = "GameId";

        /// <summary>
        /// UserTryCount
        /// </summary>
        public const string UserTryCount = "UserTryCount";

        /// <summary>
        /// Pwd1TryCount
        /// </summary>
        public const string Pwd1TryCount = "Pwd1TryCount";

        /// <summary>
        /// RoleControllers
        /// </summary>
        public const string RoleControllers = "RoleControllers";


        /// <summary>
        /// WebRoleVal
        /// </summary>
        public const string WebRoleVal = "WebRoleVal";

        /// <summary>
        /// WxUser
        /// </summary>
        public const string WxUser = "WxUser";

        /// <summary>
        /// CategoryMenuList
        /// </summary>
        public const string CategoryMenuList = "CategoryMenuList";

        /// <summary>
        /// CategoryMenuListLock
        /// </summary>
        public const string CategoryMenuListLock = "CategoryMenuListLock";

        /// <summary>
        /// CategoryRoleControllers
        /// </summary>
        public const string CategoryRoleControllers = "CategoryRoleControllers";

        /// <summary>
        /// ManageCacheSign
        /// </summary>
        public const string ManageCacheSignMark = "ManageCacheSignMark";

        /// <summary>
        /// WebCacheSignMark
        /// </summary>
        public const string WebCacheSignMark = "WebCacheSignMark";

        /// <summary>
        /// AllJobs
        /// </summary>
        public const string AllJobs = "AllJobs";

        /// <summary>
        /// AllCategorysByGuid
        /// </summary>
        public const string AllCategorysByGuid = "AllCategorysByGuid";

        /// <summary>
        /// AllCategorysById
        /// </summary>
        public const string AllCategorysById = "AllCategorysById";

        /// <summary>
        /// PageCacheLock
        /// </summary>
        public const string PageCacheLock = "PageCacheLock";

        /// <summary>
        /// MpTimeChk
        /// </summary>
        public const string MpTimeChk = "MpTimeChk";

        /// <summary>
        /// ApplyRecord
        /// </summary>
        public const string ApplyRecord = "ApplyRecord";




        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        private static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = ConnectionMultiplexer.Connect(Coonstr);
                        }
                    }
                }
                //注册如下事件
                _instance.ConnectionFailed += MuxerConnectionFailed;
                _instance.ConnectionRestored += MuxerConnectionRestored;
                _instance.ErrorMessage += MuxerErrorMessage;
                _instance.ConfigurationChanged += MuxerConfigurationChanged;
                _instance.HashSlotMoved += MuxerHashSlotMoved;
                _instance.InternalError += MuxerInternalError;
                return _instance;
            }
        }


        static RedisHelper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        /// <summary>
        /// 获取一个加了前缀的key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKey(string key)
        {
            return MergeKey(key);
        }

        /// <summary>
        /// 加锁, 返回true加锁成功
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Lock(string key)
        {
            key = MergeKey(key);
            return GetDatabase().LockTake(key, ConfigHelper.Get(ConfigHelper.WebAppId), new TimeSpan(0, 0, 10)); //锁的最长时间为5秒, 防止死锁
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool LockRelease(string key)
        {
            key = MergeKey(key);
            return GetDatabase().LockRelease(key, ConfigHelper.Get(ConfigHelper.WebAppId));
        }

        /// <summary>
        /// 这里的 MergeKey 用来拼接 Key 的前缀，具体不同的业务模块使用不同的前缀。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string MergeKey(string key)
        {
            return Prefix + key;
        }
        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            key = MergeKey(key);
            return Deserialize<T>(GetDatabase().StringGet(key));
        }

        /// <summary>
        /// 执行一个命令,返回数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static RedisResult Execute(string cmd, object[] args)
        {
            return GetDatabase().Execute(cmd, args);
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            key = MergeKey(key);
            return Deserialize<object>(GetDatabase().StringGet(key));
        }

        /// <summary>
        /// 更新一个key的过期时间 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        public static void UpdateExpire(string key, TimeSpan expire)
        {
            key = MergeKey(key);
            GetDatabase().KeyExpire(key, expire);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            key = MergeKey(key);
            GetDatabase().StringSet(key, Serialize(value));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        public static void Set(string key, object value, TimeSpan expiry)
        {
            key = MergeKey(key);
            GetDatabase().StringSet(key, Serialize(value), expiry);
        }


        /// <summary>
        /// 判断在缓存中是否存在该key的缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyExists(key);  //可直接调用
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 异步设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task SetAsync(string key, object value)
        {
            key = MergeKey(key);
            await GetDatabase().StringSetAsync(key, Serialize(value));
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<object> GetAsync(string key)
        {
            key = MergeKey(key);
            object value = await GetDatabase().StringGetAsync(key);
            return value;
        }

        /// <summary>
        /// 实现递增
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Increment(string key)
        {
            key = MergeKey(key);
            //三种命令模式
            //Sync,同步模式会直接阻塞调用者，但是显然不会阻塞其他线程。
            //Async,异步模式直接走的是Task模型。
            //Fire - and - Forget,就是发送命令，然后完全不关心最终什么时候完成命令操作。
            //即发即弃：通过配置 CommandFlags 来实现即发即弃功能，在该实例中该方法会立即返回，如果是string则返回null 如果是int则返回0.这个操作将会继续在后台运行，一个典型的用法页面计数器的实现：
            return GetDatabase().StringIncrement(key, flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 实现递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Decrement(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().HashDecrement(key, value, flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //LogHelper.WriteInfoLog("Configuration changed: " + e.EndPoint);
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //LogHelper.WriteInfoLog("ErrorMessage: " + e.Message);
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //LogHelper.WriteInfoLog("ConnectionRestored: " + e.EndPoint);
        }
        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //LogHelper.WriteInfoLog("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }
        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //LogHelper.WriteInfoLog("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }
        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            //LogHelper.WriteInfoLog("InternalError:Message" + e.Exception.Message);
        }

        //场景不一样，选择的模式便会不一样，大家可以按照自己系统架构情况合理选择长连接还是Lazy。
        //建立连接后，通过调用ConnectionMultiplexer.GetDatabase 方法返回对 Redis Cache 数据库的引用。从 GetDatabase 方法返回的对象是一个轻量级直通对象，不需要进行存储。

        /// <summary>
        /// 使用的是Lazy，在真正需要连接时创建连接。
        /// 延迟加载技术
        /// 微软azure中的配置 连接模板
        /// </summary>
        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    //var options = ConfigurationOptions.Parse(constr);
        //    ////options.ClientName = GetAppName(); // only known at runtime
        //    //options.AllowAdmin = true;
        //    //return ConnectionMultiplexer.Connect(options);
        //    ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Coonstr);
        //    muxer.ConnectionFailed += MuxerConnectionFailed;
        //    muxer.ConnectionRestored += MuxerConnectionRestored;
        //    muxer.ErrorMessage += MuxerErrorMessage;
        //    muxer.ConfigurationChanged += MuxerConfigurationChanged;
        //    muxer.HashSlotMoved += MuxerHashSlotMoved;
        //    muxer.InternalError += MuxerInternalError;
        //    return muxer;
        //});


        #region  当作消息代理中间件使用 一般使用更专业的消息队列来处理这种业务场景
        /// <summary>
        /// 当作消息代理中间件使用
        /// 消息组建中,重要的概念便是生产者,消费者,消息中间件。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(string channel, string message)
        {
            ISubscriber sub = Instance.GetSubscriber();
            //return sub.Publish("messages", "hello");
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 在消费者端得到该消息并输出
        /// </summary>
        /// <param name="channelFrom"></param>
        /// <returns></returns>
        public static void Subscribe(string channelFrom)
        {
            ISubscriber sub = Instance.GetSubscriber();
            sub.Subscribe(channelFrom, (channel, message) =>
            {
                Console.WriteLine((string)message);
            });
        }
        #endregion

        /// <summary>
        /// GetServer方法会接收一个EndPoint类或者一个唯一标识一台服务器的键值对
        /// 有时候需要为单个服务器指定特定的命令
        /// 使用IServer可以使用所有的shell命令，比如：
        /// DateTime lastSave = server.LastSave();
        /// ClientInfo[] clients = server.ClientList();
        /// 如果报错在连接字符串后加 ,allowAdmin=true;
        /// </summary>
        /// <returns></returns>
        public static IServer GetServer(string host, int port)
        {
            IServer server = Instance.GetServer(host, port);
            return server;
        }

        /// <summary>
        /// 获取全部终结点
        /// </summary>
        /// <returns></returns>
        public static EndPoint[] GetEndPoints()
        {
            EndPoint[] endpoints = Instance.GetEndPoints();
            return endpoints;
        }

    }
}
