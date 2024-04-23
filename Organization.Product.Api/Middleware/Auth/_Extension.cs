using Organization.Product.Domain.ValueObjects.Configurations;

namespace Organization.Product.Api.Middleware.Auth
{
#pragma warning disable IDE1006
    static class _Extension
#pragma warning restore IDE1006
    {
        private static IAuthMethods GetAuthMethods(IConfiguration configuration)
        {
            var authOptions = AuthOptions.Load(configuration);
            return
                authOptions.AuthType == AuthOptions.AuthType_IdPasswordJwt ? new IdPasswordJwt() :
                throw new Exception("invalid config");
        }

        public static void MyAddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authMethods = GetAuthMethods(configuration);
            authMethods.AddAuthentication(services, configuration);
        }

        public static void MyUseAuthentication(this WebApplication app, IConfiguration configuration)
        {
            var authMethods = GetAuthMethods(configuration);
            authMethods.UseAuthentication(app, configuration);
        }
    }
}
