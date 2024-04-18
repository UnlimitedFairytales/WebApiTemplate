using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Organization.Product.ApplicationServices.UseCases._Sample;

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
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        [MapToApiVersion("0.1")]
        public WeatherForecastResultDto Get_0_1()
        {
            _logger.LogTrace($"{nameof(this.Get_0_1)} Begin 日本語");
            _logger.LogDebug($"{nameof(this.Get_0_1)} Begin");
            _logger.LogInformation($"{nameof(this.Get_0_1)} Begin {this.ControllerContext.HttpContext.Connection.RemoteIpAddress}");
            _logger.LogWarning($"{nameof(this.Get_0_1)} Begin");
            _logger.LogError($"{nameof(this.Get_0_1)} Begin");
            _logger.LogCritical($"{nameof(this.Get_0_1)} Begin");

            var usecase = new WeatherForecastUseCase();
            var result = usecase.GetWeatherForecastForNext5Days();

            _logger.LogInformation($"{nameof(this.Get_0_1)} End");
            return result;
        }

        [HttpGet()]
        [MapToApiVersion("0.2")]
        public WeatherForecastResultDto Get_0_2()
        {
            var usecase = new WeatherForecastUseCase();
            var result = usecase.ThrowException();
            return result;
        }

        [HttpPost()]
        [MapToApiVersion("0.2")]
        public JsonResult Post_0_2(WeatherForecastRequestDto requestDto)
        {
            var usecase = new WeatherForecastUseCase();
            var result = usecase.Parrot(requestDto, true);
            // Return result with new JsonSerializerOptions() (null properties exists)
            return new JsonResult(result, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet()]
        [MapToApiVersion("1.0")]
        public WeatherForecastResultDto Get([FromQuery] WeatherForecastRequestDto requestDto)
        {
            var usecase = new WeatherForecastUseCase();
            var result = usecase.Parrot(requestDto, false);
            return result;
        }

        [HttpPost()]
        [MapToApiVersion("1.0")]
        public WeatherForecastResultDto Post(WeatherForecastRequestDto requestDto)
        {
            var usecase = new WeatherForecastUseCase();
            var result = usecase.Parrot(requestDto, true);
            return result;
        }
    }
}
