namespace TestTask.Services.Utility
{
    public interface ILogger
    {
        public void Error(string message, string arg = null);
        public void Warning(string message, string arg = null);
        public void Info(string message, string arg = null);
        public void Debug(string message, string arg = null);

    }
}
