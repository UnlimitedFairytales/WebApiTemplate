using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Organization.Product.ApplicationServices.UseCases._Sample;
using Organization.Product.Domain.Common.ValueObjects;

namespace Organization.Product.Api.Controllers._Sample
{
    [ApplicationServices.Aop.Log]
    [ApiController]
    [ApiVersion("0.1")]
    [ApiVersion("0.2")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        readonly WeatherForecastUseCase _useCase;
        readonly ILogger<WeatherForecastController> _logger;
        readonly ICommonParams _commonParams;

        public WeatherForecastController(WeatherForecastUseCase useCase, ILogger<WeatherForecastController> logger, ICommonParams commonParams)
        {
            this._useCase = useCase;
            this._logger = logger;
            this._commonParams = commonParams;
        }

        [HttpGet()]
        [MapToApiVersion("0.1")]
        public WeatherForecastResultDto Get_0_1()
        {
            this._logger.LogTrace($"{nameof(this.Get_0_1)} Begin 日本語");
            this._logger.LogDebug($"{nameof(this.Get_0_1)} Begin");
#pragma warning disable CA2254 // テンプレートは静的な式にする必要があります
            this._logger.LogInformation($"{nameof(this.Get_0_1)} Begin {this.ControllerContext.HttpContext.Connection.RemoteIpAddress}");
            this._logger.LogWarning($"{nameof(this.Get_0_1)} Begin User={this._commonParams.User}, Prog={this._commonParams.Prog}, Term={this._commonParams.Term}");
#pragma warning restore CA2254 // テンプレートは静的な式にする必要があります
            this._logger.LogError($"{nameof(this.Get_0_1)} Begin");
            this._logger.LogCritical($"{nameof(this.Get_0_1)} Begin");

            var result = this._useCase.GetWeatherForecastForNext5Days();

            this._logger.LogInformation($"{nameof(this.Get_0_1)} End");
            return result;
        }

        [HttpGet()]
        [MapToApiVersion("0.2")]
        public WeatherForecastResultDto Get_0_2([FromQuery] WeatherForecastRequestDto _)
        {
            return this._useCase.ThrowException();
        }

        [HttpPost()]
        [MapToApiVersion("0.2")]
        public JsonResult Post_0_2(WeatherForecastRequestDto requestDto)
        {
            var result = this._useCase.Parrot(requestDto, true);
            return new JsonResult(result, new System.Text.Json.JsonSerializerOptions()); // null properties exists
        }

        [HttpPut]
        [MapToApiVersion("0.2")]
        public WeatherForecastResultDto Put_0_2(WeatherForecastRequestDto _)
        {
            return this._useCase.ThrowAppException();
        }

        [HttpGet()]
        [MapToApiVersion("1.0")]
        public WeatherForecastResultDto Get([FromQuery] WeatherForecastRequestDto requestDto)
        {
            return this._useCase.Parrot(requestDto, false);
        }

        [HttpPost()]
        [MapToApiVersion("1.0")]
        public WeatherForecastResultDto Post(WeatherForecastRequestDto requestDto)
        {
            return this._useCase.Parrot(requestDto, true);
        }
    }
}
