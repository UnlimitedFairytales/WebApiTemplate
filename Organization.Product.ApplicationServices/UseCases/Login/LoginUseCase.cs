using Organization.Product.Domain.Authentications.Services;

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

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto requestDto)
        {
            var result = await this._appAuthenticationService.AuthenticateAsync(requestDto.UserCd ?? "", requestDto.Password);
            return new LoginResultDto(result.Token);
        }

        public async Task<LoginResultDto> LogoutAsync()
        {
            await this._appAuthenticationService.SignOutAsync();
            return new LoginResultDto(null);
        }
    }
}
