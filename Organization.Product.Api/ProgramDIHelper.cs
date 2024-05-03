using Organization.Product.Api.Middleware.Auth.Cookie;
using Organization.Product.Api.Middleware.Auth.Jwt;
using Organization.Product.Api.Middleware.Auth.Session;
using Organization.Product.ApplicationServices.UseCases._Sample;
using Organization.Product.ApplicationServices.UseCases.Login;
using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Common.Configurations;

namespace Organization.Product.Api
{
    public static class ProgramDIHelper
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
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

            // Options
            services.AddTransient<AuthOptions>();

            // _Sample
            services.AddTransient<WeatherForecastUseCase>();
        }
    }
}
