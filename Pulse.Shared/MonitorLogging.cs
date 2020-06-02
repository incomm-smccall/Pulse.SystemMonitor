using NLog;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pulse.Shared
{
    [Serializable]
    public static class MonitorLogging
    {
        private static readonly Logger ErrorLogger = LogManager.GetLogger("errors");
        private static readonly Logger GenLogger = LogManager.GetLogger("general");
        
        public static StringBuilder LogBuilder { get; set; }

        public static void LogMessage(LoggingLevel loggingLevel, string message, [CallerMemberName] string functionName = "")
        {
            GenLogger.Log(GetLogLevel(loggingLevel), $"[{functionName}] : {DateTime.Now} => {message}");
        }

        public static void LogErrorMessage(string message, Exception exceptionObject, [CallerMemberName] string functionName = "")
        {
            if (exceptionObject != null)
                ErrorLogger.Log(LogLevel.Error, exceptionObject, $"[{functionName}] : {DateTime.Now} => {message}");
            else
                ErrorLogger.Log(LogLevel.Error, $"[{functionName}] : {DateTime.Now} => {message}");
        }

        private static LogLevel GetLogLevel(LoggingLevel loggingLevel)
        {
            switch (loggingLevel)
            {
                case LoggingLevel.Debug:
                    return LogLevel.Debug;
                case LoggingLevel.Info:
                    return LogLevel.Info;
                case LoggingLevel.Warn:
                    return LogLevel.Warn;
                case LoggingLevel.Error:
                    return LogLevel.Error;
                case LoggingLevel.Fatal:
                    return LogLevel.Fatal;
                default: // should never come up
                    return LogLevel.Debug;
            }
        }
    }

    public enum LoggingLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
