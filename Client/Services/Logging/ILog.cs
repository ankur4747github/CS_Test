namespace Services.Logging
{
    internal interface ILog
    {
        void Log(string message, string className, bool Error = true);
    }
}