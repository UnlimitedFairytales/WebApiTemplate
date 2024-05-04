using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Organization.Product.Api.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using AuthOptions_Session = Organization.Product.Api.Configurations.Session;

namespace Organization.Product.Api.Middleware.Auth.Session
{
    public class IdPasswordSession : IAuthMethods
    {
        public void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = new AuthOptions(configuration);
            switch (authOptions.Session.StoreType)
            {
                case AuthOptions_Session.StoreType_InMemory:
                    services.AddDistributedMemoryCache();
                    break;
                case AuthOptions_Session.StoreType_Redis:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = authOptions.Session.ConnectionString;
                        options.InstanceName = authOptions.Session.InstanceName;
                    });
                    break;
                default:
                    break;
            }

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.IdleTimeout = TimeSpan.FromMinutes(authOptions.Session.IdleTimeout_Minute);
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(authOptions.Session.IdleTimeout_Minute * 2);
                    options.SlidingExpiration = true;
                    options.EventsType = typeof(SessionCookieAuthenticationEvents);
                });
            services.AddScoped<SessionCookieAuthenticationEvents>();
        }

        public void AddSecurityDefinition_AddSecurityRequirement(SwaggerGenOptions options)
        {
            var schemeName = CookieAuthenticationDefaults.AuthenticationScheme;
            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Cookie,
                Name = ".AspNetCore.Cookies & .AspNetCore.Session",
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
            app.UseSession();
            app.UseAuthentication();
        }
    }
}
