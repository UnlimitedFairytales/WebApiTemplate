using Organization.Product.Api.Middleware.Auth.Jwt;
using Organization.Product.Api.Middleware.Auth.Cookie;
using Organization.Product.Domain.Common.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api.Middleware.Auth
{
#pragma warning disable IDE1006
    static class _Extension
#pragma warning restore IDE1006
    {
        static IAuthMethods GetAuthMethods(IConfiguration configuration)
        {
            var authOptions = new AuthOptions(configuration);
            return
                authOptions.AuthType == AuthOptions.AuthType_IdPasswordCookie ? new IdPasswordCookie() :
                authOptions.AuthType == AuthOptions.AuthType_IdPasswordJwt ? new IdPasswordJwt() :
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
