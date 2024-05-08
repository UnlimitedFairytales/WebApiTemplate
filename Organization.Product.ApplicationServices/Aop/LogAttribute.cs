using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;

namespace Organization.Product.ApplicationServices.Aop
{
    // Logger機能でソースの位置を出しても意味がないため、Aop箇所を自前で出力
    public class LogAttribute : OnMethodBoundaryAspect
    {
        static ILogger? _logger;

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        // static
        // ----------------------------------------
        // instance

        readonly LogLevel _logLevel;
        readonly LogLevel _exceptionLogLevel;

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
            // _logger?.Log(this._exceptionLogLevel, "{FullName}.{Name} OnException", arg.Method.ReflectedType?.FullName, arg.Method.Name);
        }
    }
}
