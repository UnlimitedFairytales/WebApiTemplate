using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Organization.Product.Api.Utils.Hasher;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Authentications.ValueObjects;
using Organization.Product.Domain.Common.ValueObjects;
using System.Security.Claims;

namespace Organization.Product.Api.Middleware.Auth.Cookie
{
    public class CookieAppAuthenticationService : IAppAuthenticationService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IHasher _hasher;

        public CookieAppAuthenticationService(IHttpContextAccessor httpContextAccessor, IHasher hasher)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._hasher = hasher;
        }

        public AppAuthenticationResult Authenticate(string userCd, string? password)
        {
            IHasher.Validate(userCd, password!, this._hasher);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userCd)
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
            return new AppAuthenticationResult();
        }

        public void SignOut()
        {
            throw AppException.Create(AppMessage.W5006());
        }
    }
}
