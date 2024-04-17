using log4net.Core;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Entities;

namespace Organization.Product.Api.Middleware.Log4Net
{
    public class CustomLoggingEventFactory : ILog4NetLoggingEventFactory
    {
        protected string RawDefaultLoggerName { get; set; }
        protected string ReplacedLoggerName { get; set; }

        public CustomLoggingEventFactory(string rawDefaultLoggerName, string replacedLoggerName)
        {
            this.RawDefaultLoggerName = rawDefaultLoggerName;
            this.ReplacedLoggerName = replacedLoggerName;
        }

        public LoggingEvent? CreateLoggingEvent<TState>(in MessageCandidate<TState> messageCandidate, log4net.Core.ILogger logger, Log4NetProviderOptions options, IExternalScopeProvider scopeProvider)
        {
            Type callerStackBoundaryDeclaringType = typeof(LoggerExtensions); // default
            string message = messageCandidate.Formatter(messageCandidate.State, messageCandidate.Exception); // default
            Level logLevel = options.LogLevelTranslator.TranslateLogLevel(messageCandidate.LogLevel, options); // default
            if (logLevel == null || (string.IsNullOrEmpty(message) && messageCandidate.Exception == null)) return null; // default
            string loggerName = logger.Name; // default

            // Custom
            if (logger.Name == this.RawDefaultLoggerName)
            {
                loggerName = this.ReplacedLoggerName;
                callerStackBoundaryDeclaringType = null!;
            }

            var loggingEvent = new LoggingEvent(
                callerStackBoundaryDeclaringType: callerStackBoundaryDeclaringType,
                repository: logger.Repository,
                loggerName: loggerName,
                level: logLevel,
                message: message,
                exception: messageCandidate.Exception);
            return loggingEvent;
        }
    }
}
