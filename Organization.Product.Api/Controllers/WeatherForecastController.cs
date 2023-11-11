using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Organization.Product.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        [MapToApiVersion("0.1")]
        public IEnumerable<WeatherForecast> Get_0_1()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet()]
        [MapToApiVersion("1.0")]
        public IEnumerable<WeatherForecast> Get()
        {
            return new WeatherForecast[]
            {
                new WeatherForecast()
                {
                    Date = DateTime.Now,
                    TemperatureC = 100,
                    Summary = "Warm!!!!!!!"
                }
            };
        }
    }
}
