using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public static  void LogError(string? message, Exception? ex, ErrorLevel level = ErrorLevel.Error, [CallerMemberName] string callerName = "")
        {
            var logMessage = $"[{callerName}] {message} - Exception: {ex.Message}";
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
