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
            Fatal
        }

        public static void LogError(string? message, Exception? ex, ErrorLevel level = ErrorLevel.Error, [CallerMemberName] string callerName = "")
        {
            String logMessage = "";

            if (ex is not null)
            {
                logMessage = $"[{callerName}] {message} - Exception: {ex.Message}";

            }
            else
            {
                logMessage = $"[{callerName}] {message}";

            }
            switch (level)
                {
                    case ErrorLevel.Info:
                        _logger.Info(logMessage);
                        break;
                    case ErrorLevel.Warn:
                        _logger.Warn(logMessage);
                        break;
                    case ErrorLevel.Error:
                        _logger.Error(logMessage);
                        break;
                    case ErrorLevel.Fatal:
                        _logger.Fatal(logMessage);
                        break;
                    default:
                        _logger.Error(logMessage);
                        break;
                }
        }

    }
}
