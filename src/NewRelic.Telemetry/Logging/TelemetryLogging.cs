using Microsoft.Extensions.Logging;
using System;

namespace NewRelic.Telemetry
{
     /// <summary>
     /// Manages logging within the Telemetry SDK.
     /// </summary>
    public class TelemetryLogging
    {
        private const string _prefix = "NewRelic Telemetry:";
        private const string _category = "NewRelic.Telemetry";

        private ILogger? _logger;

        private static string MessageFormatter(object state, Exception error)
        {
            return $"{_prefix} {state} {error}".Trim();
        }

        internal TelemetryLogging(ILoggerFactory? loggerFactory)
        {
            if (loggerFactory != null)
            {
                _logger = loggerFactory.CreateLogger(_category);
            }
        }

        internal void Debug(string message)
        {
            Debug(message, null);
        }

        internal void Debug(string message, Exception? exception)
        {
            _logger?.Log(LogLevel.Debug, 0, message, exception, MessageFormatter);
        }

        internal void Error(string message)
        {
            Error(message, null);
        }

        internal void Error(string message, Exception? exception)
        {
            _logger?.Log(LogLevel.Error, 0, message, exception, MessageFormatter);
        }

        internal void Exception(Exception ex)
        {
            _logger?.Log(LogLevel.Error, 0, ex.GetType().Name, ex, MessageFormatter);
        }

        internal void Info(string message)
        {
            Info(message, null);
        }

        internal void Info(string message, Exception? exception)
        {
            _logger?.Log(LogLevel.Information, 0, message, exception, MessageFormatter);
        }

        internal void Warning(string message)
        {
            Warning(message, null);
        }

        internal void Warning(string message, Exception? exception)
        {
            _logger?.Log(LogLevel.Warning, 0, message, exception, MessageFormatter);
        }
    }
}