using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Organization.Product.ApplicationServices;
using Organization.Product.Domain.ValueObjects;

namespace Organization.Product.Api.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;

        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
        {
            _logger = logger;
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
                    var actionArguments = context.HttpContext.Items[BindModelCacheFilter.Key] as IDictionary<string, object?>;
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
                _logger.LogError(ex, "{ID} {Message}{NewLine}{requestPayload}", err.ID, ex.Message, Environment.NewLine, requestPayload);

                var errDto = new BaseResultDto() { Error = err };
                context.Result = new JsonResult(errDto);
                context.ExceptionHandled = true;
            }
        }
    }
}
