using Organization.Product.Api.Middleware.Auth.Cookie;
using Organization.Product.Api.Middleware.Auth.Jwt;
using Organization.Product.Api.Middleware.Auth.Session;
using Organization.Product.Domain.Common.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api.Middleware.Auth
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        static IAuthMethods GetAuthMethods(IConfiguration configuration)
        {
            var authOptions = new AuthOptions(configuration);
            return
                authOptions.AuthType == AuthOptions.AuthType_IdPasswordCookie ? new IdPasswordCookie() :
                authOptions.AuthType == AuthOptions.AuthType_IdPasswordJwt ? new IdPasswordJwt() :
                authOptions.AuthType == AuthOptions.AuthType_IdPasswordSession ? new IdPasswordSession() :
                throw new Exception("invalid config");
        }

        public static void MyAddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authMethods = GetAuthMethods(configuration);
            authMethods.AddAuthentication(services, configuration);
        }

        public static void MyAddSecurityDefinition_AddSecurityRequirement(this SwaggerGenOptions options, IConfiguration configuration)
        {
            var authMethods = GetAuthMethods(configuration);
            authMethods.AddSecurityDefinition_AddSecurityRequirement(options);
        }

        public static void MyUseAuthentication(this WebApplication app, IConfiguration configuration)
        {
            var authMethods = GetAuthMethods(configuration);
            authMethods.UseAuthentication(app, configuration);
        }
    }
}
