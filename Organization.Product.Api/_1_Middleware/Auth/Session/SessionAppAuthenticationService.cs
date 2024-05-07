﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Organization.Product.Domain.Authentications.Repositories;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Authentications.ValueObjects;
using System.Security.Claims;

namespace Organization.Product.Api._1_Middleware.Auth.Session
{
    public class SessionAppAuthenticationService : IAppAuthenticationService
    {
        public static readonly string SESSION_TOKEN = "SessionToken";

        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IAppAuthenticatedUserRepository _appAuthenticatedUserRepository;

        public SessionAppAuthenticationService(IHttpContextAccessor httpContextAccessor, IAppAuthenticatedUserRepository appAuthenticatedUserRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._appAuthenticatedUserRepository = appAuthenticatedUserRepository;
        }

        public AppAuthenticationResult Authenticate(string userCd, string? password)
        {
            var user = this._appAuthenticatedUserRepository.FindBy(userCd, password!);
            var sessionToken = Guid.NewGuid().ToString("N");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserCd ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Version, (user.Ver ?? 0).ToString()),
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