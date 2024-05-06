using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Organization.Product.Api.Utils.Hasher;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Authentications.ValueObjects;
using System.Security.Claims;

namespace Organization.Product.Api.Middleware.Auth.Session
{
    public class SessionAppAuthenticationService : IAppAuthenticationService
    {
        public static readonly string SESSION_TOKEN = "SessionToken";

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IHasher _hasher;

        public SessionAppAuthenticationService(IHttpContextAccessor httpContextAccessor, IHasher hasher)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._hasher = hasher;
        }

        public AppAuthenticationResult Authenticate(string userCd, string? password)
        {
            IHasher.Validate(userCd, password!, this._hasher);
            var sessionToken = Guid.NewGuid().ToString("N");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userCd),
                new Claim(SESSION_TOKEN, sessionToken)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            this._httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            this._httpContextAccessor.HttpContext!.Session.SetString(SESSION_TOKEN, sessionToken);
            return new AppAuthenticationResult();
        }

        public void SignOut()
        {
            this._httpContextAccessor.HttpContext!.Session.Clear();
        }
    }
}
