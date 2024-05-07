using Organization.Product.Api._1_Middleware.Auth.Cookie;
using Organization.Product.Api._1_Middleware.Auth.Jwt;
using Organization.Product.Api._1_Middleware.Auth.Session;
using Organization.Product.Api.Common;
using Organization.Product.Api.Configurations;
using Organization.Product.ApplicationServices.UseCases._Sample;
using Organization.Product.ApplicationServices.UseCases.Login;
using Organization.Product.Domain.Authentications.Repositories;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Gateway.Authentications;

namespace Organization.Product.Api
{
    public static class ProgramDIHelper
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<CommonParams>();
            services.AddTransient<LoginUseCase>();
            var authOption = new AuthOptions(configuration);
            switch (authOption.AuthType)
            {
                case AuthOptions.AuthType_IdPasswordCookie:
                    services.AddTransient<IAppAuthenticationService, CookieAppAuthenticationService>();
                    break;
                case AuthOptions.AuthType_IdPasswordJwt:
                    services.AddTransient<IAppAuthenticationService, JwtAppAuthenticationService>();
                    break;
                case AuthOptions.AuthType_IdPasswordSession:
                    services.AddTransient<IAppAuthenticationService, SessionAppAuthenticationService>();
                    break;
                default:
                    break;
            }
            services.AddTransient<IHasher, Pbkdf2Hasher>();
            services.AddTransient<IAppAuthenticatedUserRepository, Dummy_AppAuthenticatedUserRepository>();

            // Options
            services.AddTransient<AuthOptions>();

            // _Sample
            services.AddTransient<WeatherForecastUseCase>();
        }
    }
}
