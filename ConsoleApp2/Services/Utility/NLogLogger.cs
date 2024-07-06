using NLog;

namespace TestTask.Services.Utility
{
    internal class NLogLogger : ILogger
    {
        private static Logger Logger => LogManager.GetLogger("appLoggerRules");
        public void Warning(string message, string arg = null)
        {
            if (arg == null)
            {
                Logger.Warn(message);
            }
            else
            {
                Logger.Warn(message, arg);
            }
        }
        public void Error(string message, string arg = null)
        {
            if (arg == null)
            {
                Logger.Error(message);
            }
            else
            {
                Logger.Error(message, arg);
            }
        }
        public void Info(string message, string arg = null)
        {
            if (arg == null)
            {
                Logger.Info(message);
            }
            else
            {
                Logger.Info(message, arg);
            }
        }
        public void Debug(string message, string arg = null)
        {
            if (arg == null)
            {
                Logger.Debug(message);
            }
            else
            {
                Logger.Debug(message, arg);
            }
        }
    }
}
