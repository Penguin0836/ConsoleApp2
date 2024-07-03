using NLog;

namespace TestTask.Services.Utility
{
    internal class MyLogger : ILogger
    {
        private static MyLogger _instance;
        private static Logger _logger;
        public static MyLogger GetInstance()
        {
            return _instance ??= new MyLogger();
        }
        private static Logger GetLogger(string theLogger)
        {
            return _logger ??= LogManager.GetLogger(theLogger);
        }
        public void Warning(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("appLoggerRules").Warn(message);
            }
            else
            {
                GetLogger("appLoggerRules").Warn(message, arg);
            }
        }
        public void Error(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("appLoggerRules").Error(message);
            }
            else
            {
                GetLogger("appLoggerRules").Error(message, arg);
            }
        }
        public void Info(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("appLoggerRules").Info(message);
            }
            else
            {
                GetLogger("appLoggerRules").Info(message, arg);
            }
        }
        public void Debug(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("appLoggerRules").Debug(message);
            }
            else
            {
                GetLogger("appLoggerRules").Debug(message, arg);
            }
        }
    }
}
