using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Organization.Product.Api._1_Middleware.Auth.Session
{
    public class SessionCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public SessionCookieAuthenticationEvents(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            return Task.CompletedTask;
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            return Task.CompletedTask;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal!;
            var sessionToken_cookie = (from c in userPrincipal.Claims
                                       where c.Type == SessionAppAuthenticationService.SESSION_TOKEN
                                       select c.Value).FirstOrDefault();
            var sessionToken_session = this._httpContextAccessor.HttpContext?.Session.GetString(SessionAppAuthenticationService.SESSION_TOKEN);
            if (string.IsNullOrEmpty(sessionToken_cookie) ||
                string.IsNullOrEmpty(sessionToken_session) ||
                sessionToken_cookie != sessionToken_session)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
