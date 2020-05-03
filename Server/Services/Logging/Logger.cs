using MetroLog;
using MetroLog.Targets;
using System;

namespace Services.Logging
{
    public class Logger : ILog
    {
        //Logs Available in %appdata% folder
        //ex:- C:\Users\ankur\AppData\Roaming\Server

        #region Fields

        private static ILogger log = null;
        private static readonly LoggingConfiguration configuration = null;

        #endregion Fields

        #region Constructor

        [Obsolete]
        static Logger()
        {
            configuration = new LoggingConfiguration();
            FileStreamingTarget fileStreamingTarget = new FileStreamingTarget()
            {
                RetainDays = 7
            };

            LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, fileStreamingTarget);
        }

        #endregion Constructor

        #region Public Methods

        public void Log(string message, string className, bool Error = true)
        {
            log = LogManagerFactory.DefaultLogManager.GetLogger(className);
            message = DateTime.Now + " = " + message;
            if (Error)
            {
                log.Error(message);
            }
            else
            {
                log.Info(message);
            }
        }

        #endregion Public Methods
    }
}