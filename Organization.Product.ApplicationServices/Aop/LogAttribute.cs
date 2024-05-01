using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;

namespace Organization.Product.ApplicationServices.Aop
{
    // Fodyの特性上、log4netからはメソッド名や行番号が取得できない
    public class LogAttribute : OnMethodBoundaryAspect
    {
        private static ILogger? _logger;

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        private LogLevel _logLevel;
        private LogLevel _exceptionLogLevel;

        public LogAttribute(LogLevel logLevel = LogLevel.Information, LogLevel exceptionLogLevel = LogLevel.Error)
        {
            this._logLevel = logLevel;
            this._exceptionLogLevel = exceptionLogLevel;
        }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            _logger?.Log(this._logLevel, "{FullName}.{Name} OnEntry", arg.Method.ReflectedType?.FullName, arg.Method.Name);
        }

        public override void OnExit(MethodExecutionArgs arg)
        {
            _logger?.Log(this._logLevel, "{FullName}.{Name} OnExit", arg.Method.ReflectedType?.FullName, arg.Method.Name);
        }

        public override void OnException(MethodExecutionArgs arg)
        {
            _logger?.Log(this._exceptionLogLevel, "{FullName}.{Name} OnException", arg.Method.ReflectedType?.FullName, arg.Method.Name);
        }
    }
}
