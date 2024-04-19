using Microsoft.AspNetCore.Mvc.Filters;

namespace Organization.Product.Api.Filters
{
    public class BindModelCacheFilter : IActionFilter
    {
        public static readonly string Key = "BindModelCacheFilterKey. This string can be anything.";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[BindModelCacheFilter.Key] = context.ActionArguments;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
