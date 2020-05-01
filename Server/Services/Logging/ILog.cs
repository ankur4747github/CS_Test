namespace Services.Logging
{
    public interface ILog
    {
        void Log(string message, string className, bool Error = true);
    }
}