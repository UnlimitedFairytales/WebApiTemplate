using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Organization.Product.Shared.Configurations;

namespace Organization.Product.Api._6_ActionFilters
{
    public class AntiForgeryTokenGenerationFilter : IActionFilter
    {
        readonly IAntiforgery _antiforgery;
        readonly AntiCsrfOption _antiCsrfOption;

        public AntiForgeryTokenGenerationFilter(IAntiforgery antiforgery, AntiCsrfOption antiCsrfOption)
        {
            this._antiforgery = antiforgery;
            this._antiCsrfOption = antiCsrfOption;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var tokens = this._antiforgery.GetAndStoreTokens(context.HttpContext);
            context.HttpContext.Response.Cookies.Append(this._antiCsrfOption.RequestTokenName, tokens.RequestToken!, new CookieOptions { HttpOnly = false });
        }
    }
}
