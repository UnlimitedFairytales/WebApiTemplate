using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Organization.Product.Domain.ValueObjects.Configurations;
using System.Security.Claims;

namespace Organization.Product.ApplicationServices.UseCases.Login
{
    internal class CookieAuthService
    {
        public static LoginResultDto SignIn(LoginRequestDto requestDto, AuthOptions authOptions, HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, requestDto.UserCd!)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return new LoginResultDto() { };
        }
    }
}
