using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Product.Api._1_Middleware.AntiCsrf;
using Organization.Product.ApplicationServices.UseCases.Login;

namespace Organization.Product.Api.Controllers
{
    [ApplicationServices.Aop.Log]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        readonly LoginUseCase _useCase;

        public LoginController(LoginUseCase useCase)
        {
            this._useCase = useCase;
        }

        [HttpPost]
        public Task<LoginResultDto> GetDialogMode()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<LoginResultDto> SetUserPassword()
        {
            throw new NotImplementedException();
        }

        [IgnoreAntiforgery]
        [AllowAnonymous]
        [HttpPost]
        public async Task<LoginResultDto> Login(LoginRequestDto requestDto)
        {
            return await this._useCase.LoginAsync(requestDto);
        }

        [IgnoreAntiforgery]
        [HttpPost]
        public async Task<LoginResultDto> Logout(LoginRequestDto _)
        {
            return await this._useCase.LogoutAsync();
        }
    }
}
