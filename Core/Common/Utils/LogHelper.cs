

using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace DYLS.Common.Utils
{
    public static class LogHelper
    {
        private static ILog _log;

        static LogHelper()
        {
            if (_log == null)
            {
                StartLog4();
            }
        }

        private static void StartLog4()
        {
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));
            _log = LogManager.GetLogger(repository.Name, "");
            _log.Debug("日志系统启动!");
        }


        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="msg">记录内容</param>
        public static void Debug(string msg)
        {
            _log.Debug(msg);
        }


        /// <summary>
        ///     记录错误信息
        /// </summary>
        /// <param name="msg">记录内容</param>
        public static void Error(string msg)
        {
            _log.Error(msg);
        }

        /// <summary>
        ///     记录提示信息
        /// </summary>
        /// <param name="msg">记录内容</param>
        public static void Info(string msg)
        {
            _log.Info(msg);
        }

        /// <summary>
        ///     记录警告信息
        /// </summary>
        /// <param name="msg">记录内容</param>
        public static void Warn(string msg)
        {
            _log.Warn(msg);
        }
    }
}