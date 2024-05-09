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
        public LoginResultDto GetDialogMode()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public LoginResultDto SetUserPassword()
        {
            throw new NotImplementedException();
        }

        [IgnoreAntiforgery]
        [AllowAnonymous]
        [HttpPost]
        public LoginResultDto Login(LoginRequestDto requestDto)
        {
            return this._useCase.Login(requestDto);
        }

        [IgnoreAntiforgery]
        [HttpPost]
        public LoginResultDto Logout(LoginRequestDto _)
        {
            return this._useCase.Logout();
        }
    }
}
