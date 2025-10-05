using NLog;
using System.Runtime.CompilerServices;

namespace Project.HR.Domain.Helpers
{
    public static class LogErrorHelper
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public enum ErrorLevel
        {
            Info,
            Warn,
            Error,
            Fatal,
            Trace
        }

        public static void LogError(string? message, Exception? ex, ErrorLevel level = ErrorLevel.Error, [CallerMemberName] string callerName = "")
        {
            string logMessage = $"[{callerName}] {message}";

            switch (level)
            {
                case ErrorLevel.Info:
                    _logger.Info(ex, logMessage);
                    break;
                case ErrorLevel.Warn:
                    _logger.Warn(ex, logMessage);
                    break;
                case ErrorLevel.Error:
                    _logger.Error(ex, logMessage);
                    break;
                case ErrorLevel.Fatal:
                    _logger.Fatal(ex, logMessage);
                    break;
                case ErrorLevel.Trace:
                    _logger.Trace(ex, logMessage);
                    break;
                default:
                    _logger.Error(ex, logMessage);
                    break;
            }
        }

    }
}
