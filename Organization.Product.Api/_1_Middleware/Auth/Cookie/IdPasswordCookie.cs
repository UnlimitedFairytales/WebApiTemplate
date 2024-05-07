using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Organization.Product.Api.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api._1_Middleware.Auth.Cookie
{
    public class IdPasswordCookie : IAuthMethods
    {
        public void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = new AuthOptions(configuration);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(authOptions.Cookie.Expire_Minute);
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToLogout = context => Task.CompletedTask;
                    options.Events.OnRedirectToReturnUrl = context => Task.CompletedTask;
                });
        }

        public void AddSecurityDefinition_AddSecurityRequirement(SwaggerGenOptions options)
        {
            var schemeName = CookieAuthenticationDefaults.AuthenticationScheme;
            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Cookie,
                Name = ".AspNetCore.Cookies",
                Description = "!!ATTENTION!! Cookie is a forbidden header name! \"Authorize\" button is not work!"
            });
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                        {
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = schemeName
                                }
                            },
                            Array.Empty<string>()
                        }
                });
        }

        public void UseAuthentication(WebApplication app, IConfiguration configuration)
        {
            app.UseAuthentication();
        }
    }
}
