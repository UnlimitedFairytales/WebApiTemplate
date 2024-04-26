using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Organization.Product.Domain.ValueObjects;
using Organization.Product.Domain.ValueObjects.Configurations;

namespace Organization.Product.ApplicationServices.UseCases.Login
{
    [Aop.Log]
    public class LoginUseCase
    {
        private readonly IConfiguration _configuration;

        public LoginUseCase(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public LoginResultDto Login(LoginRequestDto requestDto, HttpContext httpContext)
        {
            var isValid = requestDto.UserCd == "USER00" && requestDto.Password == "USER00";
            if (!isValid) throw AppException.Create(AppMessage.W5001());

            var authOptions = AuthOptions.Load(this._configuration);
            switch (authOptions.AuthType)
            {
                case AuthOptions.AuthType_IdPasswordCookie:
                    return CookieAuthService.SignIn(requestDto, authOptions, httpContext);
                case AuthOptions.AuthType_IdPasswordJwt:
                    return JwtAuthService.SignIn(requestDto, authOptions);
                default:
                    throw AppException.Create(AppMessage.W5001());
            }
        }

        public LoginResultDto Logout(LoginRequestDto requestDto, HttpContext httpContext)
        {
            var authOptions = AuthOptions.Load(this._configuration);
            switch (authOptions.AuthType)
            {
                case AuthOptions.AuthType_IdPasswordCookie:
                    throw AppException.Create(AppMessage.W5006());
                case AuthOptions.AuthType_IdPasswordJwt:
                    throw AppException.Create(AppMessage.W5006());
                default:
                    throw AppException.Create(AppMessage.W5001());
            }
        }
    }
}
