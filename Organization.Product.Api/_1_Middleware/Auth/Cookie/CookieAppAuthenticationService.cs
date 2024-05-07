using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Organization.Product.Domain.Authentications.Repositories;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Authentications.ValueObjects;
using Organization.Product.Domain.Common.ValueObjects;
using System.Security.Claims;

namespace Organization.Product.Api._1_Middleware.Auth.Cookie
{
    public class CookieAppAuthenticationService : IAppAuthenticationService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IAppAuthenticatedUserRepository _appAuthenticatedUserRepository;

        public CookieAppAuthenticationService(IHttpContextAccessor httpContextAccessor, IAppAuthenticatedUserRepository appAuthenticatedUserRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._appAuthenticatedUserRepository = appAuthenticatedUserRepository;
        }

        public AppAuthenticationResult Authenticate(string userCd, string? password)
        {
            var user = this._appAuthenticatedUserRepository.FindBy(userCd, password!);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserCd ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Version, (user.Ver ?? 0).ToString()),
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
