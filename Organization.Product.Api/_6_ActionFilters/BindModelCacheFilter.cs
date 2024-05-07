using Microsoft.AspNetCore.Mvc.Filters;

namespace Organization.Product.Api._6_ActionFilters
{
    public class BindModelCacheFilter : IActionFilter
    {
        public static readonly string KEY = "BindModelCacheFilterKey. This string can be anything.";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[KEY] = context.ActionArguments;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
