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

        public async Task<AppAuthenticationResult> AuthenticateAsync(string userCd, string? password)
        {
            var user = this._appAuthenticatedUserRepository.FindBy(userCd, password!);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserCd ?? ""),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.Version, (user.Ver ?? 0).ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            
            await this._httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimPrincipal,
                authProperties);
            this._httpContextAccessor.HttpContext!.User = claimPrincipal;
            return new AppAuthenticationResult();
        }

        public Task SignOutAsync()
        {
            throw AppException.Create(AppMessage.W5006());
        }
    }
}
