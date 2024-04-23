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

        public LoginResultDto Login(LoginRequestDto requestDto)
        {
            var isValid = requestDto.UserCd == "USER00" && requestDto.Password == "USER00";
            if (!isValid)
            {
                var error = AppMessage.W5001();
                throw AppException.Create(error);
            }

            var authOptions = AuthOptions.Load(this._configuration);
            switch (authOptions.AuthType)
            {
                case AuthOptions.AuthType_IdPasswordJwt:
                    return JwtAuthService.SignIn(requestDto, authOptions);
                default:
                    var error = AppMessage.W5001();
                    throw AppException.Create(error);
            }
        }
    }
}
