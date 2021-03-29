using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace TestTask.Services.Utility
{
    interface ILogger
    {
        public void Error(string message, string arg = null);
        public void Warning(string message, string arg = null);
        public void Info(string message, string arg = null);
        public void Debug(string message, string arg = null);

    }
}
