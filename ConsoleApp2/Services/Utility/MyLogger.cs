using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.Services.Utility
{
    class MyLogger : ILogger
    {
        private static MyLogger instance;
        private static Logger logger;
        public static MyLogger GetInstance()
        {
            if (instance == null)
            {
                instance = new MyLogger();
            }
            return instance;
        }
        private Logger GetLogger(string theLogger)
        {
            if (logger == null)
            {
                logger = LogManager.GetLogger(theLogger);
            }

            return logger;
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
