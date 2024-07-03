namespace TestTask.Services.Utility
{
    internal interface ILogger
    {
        public void Error(string message, string arg = null);
        public void Warning(string message, string arg = null);
        public void Info(string message, string arg = null);
        public void Debug(string message, string arg = null);

    }
}
