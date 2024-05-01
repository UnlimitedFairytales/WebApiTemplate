using Organization.Product.Domain.Authentications.Services;
using Organization.Product.Domain.Common.ValueObjects;

namespace Organization.Product.ApplicationServices.UseCases.Login
{
    [Aop.Log]
    public class LoginUseCase
    {
        readonly IAppAuthenticationService _appAuthenticationService;

        public LoginUseCase(IAppAuthenticationService appAuthenticationService)
        {
            this._appAuthenticationService = appAuthenticationService;
        }

        public LoginResultDto Login(LoginRequestDto requestDto)
        {
            var isValid = requestDto.UserCd == "USER00" && requestDto.Password == "USER00";
            if (!isValid) throw AppException.Create(AppMessage.W5001());
            var result = this._appAuthenticationService.Authenticate(requestDto.UserCd ?? "", requestDto.Password);
            return new LoginResultDto(result.Token);
        }

        public LoginResultDto Logout()
        {
            this._appAuthenticationService.SignOut();
            return new LoginResultDto(null);
        }
    }
}
