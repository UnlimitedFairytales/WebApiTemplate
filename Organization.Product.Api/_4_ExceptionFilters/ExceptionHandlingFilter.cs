using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Organization.Product.Api._6_ActionFilters;
using Organization.Product.ApplicationServices;
using Organization.Product.Domain.Common.ValueObjects;

namespace Organization.Product.Api._4_ExceptionFilters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        readonly ILogger<ExceptionHandlingFilter> _logger;

        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
        {
            this._logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            if (ex != null)
            {
                AppMessage? err = ex is AppException casted ? casted.Error : AppMessage.E9999();
                var requestPayload = "";
                try
                {
                    var actionArguments = context.HttpContext.Items[BindModelCacheFilter.KEY] as IDictionary<string, object?>;
                    if (0 < actionArguments?.Count)
                    {
                        object requestDto = actionArguments.First().Value!;
                        var options = new System.Text.Json.JsonSerializerOptions()
                        {
                            IncludeFields = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                        };
                        requestPayload = System.Text.Json.JsonSerializer.Serialize(requestDto, options);
                    }
                }
                catch
                {
                }
                if (err.ID.StartsWith("W"))
                {
                    this._logger.LogWarning(null, "{ID} {Message}{NewLine}{requestPayload}", err.ID, ex.Message, Environment.NewLine, requestPayload);
                }
                else
                {
                    this._logger.LogError(ex, "{ID} {Message}{NewLine}{requestPayload}{NewLine}", err.ID, ex.Message, Environment.NewLine, requestPayload, Environment.NewLine);
                }

                var errDto = new BaseResultDto() { Error = err };
                context.Result = new JsonResult(errDto);
                context.ExceptionHandled = true;
            }
        }
    }
}
