using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Product.ApplicationServices.UseCases.Login;

namespace Organization.Product.Api.Controllers
{
    [ApplicationServices.Aop.Log]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginUseCase _useCase;

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

        [AllowAnonymous]
        [HttpPost]
        public LoginResultDto Login(LoginRequestDto requestDto)
        {
            return _useCase.Login(requestDto, this.HttpContext);
        }

        [HttpPost]
        public LoginResultDto Logout(LoginRequestDto requestDto)
        {
            return _useCase.Logout(requestDto, this.HttpContext);
        }
    }
}
