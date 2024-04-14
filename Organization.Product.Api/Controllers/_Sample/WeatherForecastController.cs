using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Organization.Product.ApplicationServices.UseCases._Sample;

namespace Organization.Product.Api.Controllers._Sample
{
    [ApiController]
    [ApiVersion("0.1")]
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
            var usecase = new WeatherForecastUseCase();
            var result = usecase.GetWeatherForecastForNext5Days();
            return result;
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